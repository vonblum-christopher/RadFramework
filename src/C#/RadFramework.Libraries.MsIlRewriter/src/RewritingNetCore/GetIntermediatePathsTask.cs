using System.IO;
using Microsoft.Build.Framework;

namespace RewritingNetCore
{
    public class GetIntermediatePathsTask : ITask
    {
        public string IntermediateOutputPath { get; set; }
        
        [Output]
        public string IntermediateAssemblyExtension => Path.GetExtension(IntermediateAssemblyPath);

        [Output]
        public string IntermediateAssemblyName => Path.GetFileNameWithoutExtension(IntermediateAssemblyPath);
        
        [Output]
        public string IntermediateAssemblyPath => $"{IntermediateOutputPath}{Path.DirectorySeparatorChar}{IntermediateAssemblyName}.{IntermediateAssemblyExtension}";
        
        public bool Execute()
        {
            return true;
        }

        public IBuildEngine BuildEngine { get; set; }
        public ITaskHost HostObject { get; set; }
    }
}