using System;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using RewritingNetCore.Compiler.Contracts;

namespace RewritingNetCore
{
    public class BuildIsolationAssemblyLoader
    {
        
    }
    public class BuildIsolationAssemlbyLoadContext : AssemblyLoadContext
    {
        private readonly ICompilerArgs compilerArgs;
        
        internal BuildIsolationAssemlbyLoadContext(ICompilerArgs compilerArgs)
        {
            this.compilerArgs = compilerArgs;
        }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            Assembly assembly = this.LoadAssemblyByName(assemblyName);
            if ((object) assembly != null)
                return assembly;
            return AssemblyLoadContext.Default.LoadFromAssemblyName(assemblyName);
        }
        
        private Assembly LoadAssemblyByName(AssemblyName assemblyName)
        {
            if (assemblyName.Name.StartsWith("Microsoft.Build", StringComparison.OrdinalIgnoreCase)
             || assemblyName.Name.StartsWith("System.", StringComparison.OrdinalIgnoreCase))
            {
                return (Assembly) null;
            }
            
            string probingFilePath = Path.Combine(this.compilerArgs.LibFolder, assemblyName.Name) + ".dll";
            
            if (File.Exists(probingFilePath))
            {
                return this.LoadFromAssemblyPath(probingFilePath);
            }
            
            return (Assembly) null;
        }

        public Type LoadEquivalent(Type getType)
        {
            var assembly = LoadAssemblyByName(getType.Assembly.GetName());

            return assembly?.GetType(getType.FullName);
        }
    }
}