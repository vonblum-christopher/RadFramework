using System.Reflection;
using System.Runtime.Loader;
using System.Runtime.Versioning;
using Mono.Cecil;
using Newtonsoft.Json;
using RewritingNetCore.Compiler.Contracts;
using AssemblyDefinition = System.Reflection.Metadata.AssemblyDefinition;
using CustomAttribute = System.Reflection.Metadata.CustomAttribute;

namespace RewritingNetCore.Compiler
{
    public class MiddlewareRunner
    {
        public CompilerResult RunMiddlewarePlugins(CompilerArgs args)
        {
            var isolation = new BuildIsolationAssemlbyLoadContext(args);

            string entryPointPath =
                $"{args.LibFolder}{Path.DirectorySeparatorChar}{Path.GetFileName(new Uri(this.GetType().Assembly.CodeBase).LocalPath)}";

            Type isolatedTaskType = isolation.LoadEquivalent(this.GetType());

            MethodInfo executeInnerInfo = isolatedTaskType.GetMethod(nameof(ExecuteInner));

            object isolatedTask = Activator.CreateInstance(isolatedTaskType);

            CompilerResult result = new CompilerResult();
            
            try
            {
                result = (CompilerResult)executeInnerInfo.Invoke(isolatedTask, new object[] { args });
            }
            catch(Exception ex)
            {
                return new CompilerResult() { Errors = new List<string>() { ex.ToString() } };
            }

            return result;
        }
        
        public CompilerResult ExecuteInner(CompilerArgs args)
        {
            // isolate away from calling compiler
            var isolation = new BuildIsolationAssemlbyLoadContext(args);

            string entryPointPath = $"{args.LibFolder}{Path.DirectorySeparatorChar}{Path.GetFileName(new Uri(this.GetType().Assembly.CodeBase).LocalPath)}";

            Type isolatedTaskType = isolation.LoadEquivalent(this.GetType());

            MethodInfo executeInnerInfo = isolatedTaskType.GetMethod(nameof(ExecuteInner));

            object isolatedTask = Activator.CreateInstance(isolatedTaskType);
            
            foreach (var propertyInfo in isolatedTaskType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (propertyInfo.GetSetMethod(true) == null)
                {
                    continue;
                }
                
                propertyInfo.SetValue(isolatedTask, this.GetType().GetProperty(propertyInfo.Name).GetValue(this));
            }
            
            return (bool)executeInnerInfo.Invoke(isolatedTask, new object[0]);
        }
        
        public bool ExecuteInner()
        {
            isolationContext = AssemblyLoadContext.GetLoadContext(this.GetType().Assembly);

            AssemblyDefinition assembly = null;
            
            assembly = AssemblyDefinition.ReadAssembly(IntermediateAssemblyPath, new ReaderParameters(ReadingMode.Immediate) { ReadSymbols = true });

            AssemblyDefinition gAssembly = CreateGAssembly(assembly);
            
            MemReloadAssembly(ref gAssembly, GAssemblyPath);
            
            RunMiddleware(ref assembly, ref gAssembly);
            
            return true;
        }

        private void MemReloadAssembly(ref AssemblyDefinition definition, string path, bool symbols = false)
        {
            using (var save = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                definition.Write(save, new WriterParameters{ WriteSymbols =  symbols});
                save.Flush();
                save.Close();
            }
            
            definition.Dispose();
            
            definition = AssemblyDefinition.ReadAssembly(path, new ReaderParameters(ReadingMode.Immediate){ReadSymbols = symbols, InMemory = true});
        }
        
        private void RunMiddleware(ref AssemblyDefinition assembly, ref AssemblyDefinition gAssembly)
        {
            var serviceCollection = BuildMiddlewareServiceCollection(out var coreRewritingMiddlewares, out var config);

            IServiceProvider provider = serviceCollection.BuildServiceProvider();

            RunCoreMiddlewares(ref assembly, ref gAssembly, coreRewritingMiddlewares, provider);

            RunConfigMiddlewares(ref assembly, ref gAssembly, config, provider);
        }

        MethodInfo loadFromPathMethod;

        private Assembly LoadFromAssemblyPath(string path)
        {
            if(loadFromPathMethod == null)
            {
                loadFromPathMethod = isolationContext.GetType().GetMethod("LoadFromAssemblyPath");
            }

            return (Assembly)loadFromPathMethod.Invoke(isolationContext, new object[] { path });
        }

        MethodInfo loadEquivalentMethod;

        private Type LoadEquivalent(Type type)
        {
            if (loadEquivalentMethod == null)
            {
                loadEquivalentMethod = isolationContext.GetType().GetMethod("LoadEquivalent");
            }

            return (Type)loadEquivalentMethod.Invoke(isolationContext, new object[] { type });
        }

        private void RunConfigMiddlewares(ref AssemblyDefinition assembly, ref AssemblyDefinition gAssembly, JsonConfig config,
            IServiceProvider provider)
        {
            foreach (MiddlewareDefinition userMiddleware in config.Middlewares)
            {
                Type middlewareType = LoadFromAssemblyPath(userMiddleware.Assembly).GetType(userMiddleware.Type);

                ((IRewritingMiddleware) provider.GetService(middlewareType)).Process(assembly, gAssembly);
                MemReloadAssembly(ref assembly, IntermediateAssemblyPath, true);
                MemReloadAssembly(ref gAssembly, GAssemblyPath);
            }
        }

        private void RunCoreMiddlewares(ref AssemblyDefinition assembly, ref AssemblyDefinition gAssembly,
            MiddlewareDefinition[] coreRewritingMiddlewares, IServiceProvider provider)
        {
            foreach (MiddlewareDefinition coreMiddleware in coreRewritingMiddlewares)
            {
                var x = coreMiddleware.RuntimeAssembly.GetType(coreMiddleware.Type.Contains(",") 
                    ? coreMiddleware.Type.Substring(0, coreMiddleware.Type.IndexOf(",")) 
                    : coreMiddleware.Type);
                
                ((IRewritingMiddleware) provider.GetService(x)).Process(assembly,
                    gAssembly);
                MemReloadAssembly(ref assembly, IntermediateAssemblyPath, true);
                 MemReloadAssembly(ref gAssembly, GAssemblyPath);
            }
        }

        private IServiceCollection BuildMiddlewareServiceCollection(out MiddlewareDefinition[] coreRewritingMiddlewares,
            out JsonConfig config)
        {
            IServiceCollection serviceCollection = new ServiceCollection();

            RewritingDependencyDefinition[] coreRewritingDependencies =
                JsonConvert.DeserializeObject<RewritingDependencyDefinition[]>(CoreRewritingDependencies);

            foreach (RewritingDependencyDefinition rewritingDependencyDefinition in coreRewritingDependencies)
            {
                Type type = LoadEquivalent(Type.GetType(rewritingDependencyDefinition.Type));
                
                serviceCollection.Add(ServiceDescriptor.Transient(type,
                    LoadEquivalent(Type.GetType(rewritingDependencyDefinition.Implementation))));
            }

            coreRewritingMiddlewares = JsonConvert.DeserializeObject<MiddlewareDefinition[]>(CoreRewritingMiddleware);

            foreach (MiddlewareDefinition rewritingMiddlewareDefinition in coreRewritingMiddlewares)
            {
                var type = Type.GetType(rewritingMiddlewareDefinition.Type);
                
                Type coreMiddleware = LoadEquivalent(type);
                
                rewritingMiddlewareDefinition.RuntimeAssembly =coreMiddleware.Assembly;
                
                serviceCollection.Add(ServiceDescriptor.Transient(coreMiddleware, coreMiddleware));
            }

            config = JsonConvert.DeserializeObject<JsonConfig>(
                File.ReadAllText($"{MSBuildProjectDirectory}{Path.DirectorySeparatorChar}rewriting.json"));

            foreach (RewritingDependencyDefinition userDependency in config.Dependencies)
            {
                var dependencyAssembly = LoadFromAssemblyPath(userDependency.Assembly);
                
                var dependencyType = dependencyAssembly.GetType(userDependency.Type);

                var implementationType = dependencyAssembly.GetType(userDependency.Implementation);

                serviceCollection.Add(ServiceDescriptor.Transient(LoadEquivalent(dependencyType), LoadEquivalent(implementationType)));
            }

            foreach (MiddlewareDefinition userMiddleware in config.Middlewares)
            {
                var dependencyAssembly = LoadFromAssemblyPath(userMiddleware.Assembly);

                var dependencyType = dependencyAssembly.GetType(userMiddleware.Type);

                serviceCollection.Add(ServiceDescriptor.Transient(dependencyType, dependencyType));
            }

            return serviceCollection;
        }

        private AssemblyDefinition CreateGAssembly(
            AssemblyDefinition assembly)
        {
            GAssemblyPath = $"{IntermediateOutputPath}{Path.DirectorySeparatorChar}{IntermediateAssemblyName}.g{IntermediateAssemblyExtension}";

            if (File.Exists(GAssemblyPath))
            {
                File.Delete(GAssemblyPath);
            }
            
            AssemblyNameDefinition gAssemblyName = new AssemblyNameDefinition(assembly.FullName, assembly.Name.Version);

            gAssemblyName.Name = assembly.Name.Name + ".g";
            
            AssemblyDefinition gAssembly =
                AssemblyDefinition.CreateAssembly(gAssemblyName, assembly.MainModule.Name, assembly.MainModule.Kind);
            
            var targetFrameworkAttribute = new CustomAttribute(gAssembly.MainModule.ImportReference(typeof(TargetFrameworkAttribute).GetConstructor(new[]{typeof(string)})));
            
            targetFrameworkAttribute.ConstructorArguments.Add(new CustomAttributeArgument(gAssembly.MainModule.TypeSystem.String, ".NETCoreApp,Version=v2.1"));
            
            gAssembly.CustomAttributes.Add(targetFrameworkAttribute);
            
            gAssembly.MainModule.AssemblyReferences.Add(AssemblyNameReference.Parse(assembly.FullName));
            
            return gAssembly;
        }
    }
}