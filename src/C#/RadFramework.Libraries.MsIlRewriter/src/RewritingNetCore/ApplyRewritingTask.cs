using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.Build.Framework;
using Newtonsoft.Json;
using RewritingNetCore.Compiler.Contracts;

namespace RewritingNetCore
{
    public class ApplyRewritingTask : ITask, ICompilerArgs
    {
        public IBuildEngine BuildEngine { get; set; }
        public ITaskHost HostObject { get; set; }
        private object isolationContext { get; set; }
        public string LibFolder { get; set; }
        public string CoreRewritingMiddleware { get; set; }
        public string CoreRewritingDependencies { get; set; }
        public string MSBuildProjectDirectory { get; set; }
        public string MSBuildProjectFile { get; set; }
        public string IntermediateOutputPath { get; set; }
        public string IntermediateAssemblyName { get; set; }
        public string IntermediateAssemblyExtension { get; set; }
        public string SolutionPath { get; set; }

        [Output]
        public string GAssemblyPath { get; set; }
        
        public string IntermediateAssemblyPath { get; set; }

        public bool Execute()
        {
            Process compilationProcess = new Process();

            string compilerArgsFile = $"{IntermediateOutputPath}{Path.DirectorySeparatorChar}RewrititingNetCore.json";
            string compilerOutputFile =
                $"{IntermediateOutputPath}{Path.DirectorySeparatorChar}RewrititingNetCore-result.json";

            compilationProcess.StartInfo.FileName =
                $"{LibFolder}{Path.DirectorySeparatorChar}RewritingNetCore.Compiler.dll";
            compilationProcess.StartInfo.Arguments = IntermediateAssemblyPath;

            File.WriteAllText(
                compilerArgsFile,
                JsonConvert.SerializeObject(this, typeof(ICompilerArgs),
                    new JsonSerializerSettings() { Formatting = Formatting.Indented }));

            compilationProcess.Start();
            compilationProcess.WaitForExit();

            CompilerResult result = JsonConvert.DeserializeObject<CompilerResult>(File.ReadAllText(compilerOutputFile));
            
            foreach (string error in result.Errors)
            {
                BuildEngine.LogErrorEvent(
                    new BuildErrorEventArgs(
                        "",
                        "",
                        "",
                        0,
                        0,
                        0,
                        0,
                        error,
                        "",
                        ""));
            }

            foreach (string warning in result.Warnings)
            {
                BuildEngine.LogWarningEvent(
                    new BuildWarningEventArgs(
                        "",
                        "",
                        "",
                        0,
                        0,
                        0,
                        0,
                        warning,
                        "",
                        ""));
            }

            return !result.Errors.Any();
        }
    }
}