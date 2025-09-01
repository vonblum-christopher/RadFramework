namespace RewritingNetCore.Compiler.Contracts
{
    public struct CompilerArgs : ICompilerArgs
    {
        public string LibFolder { get; set; }
        public string CoreRewritingMiddleware { get; set; }
        public string CoreRewritingDependencies { get; set; }
        public string MSBuildProjectDirectory { get; set; }
        public string MSBuildProjectFile { get; set; }
        public string IntermediateOutputPath { get; set; }
        public string IntermediateAssemblyName { get; set; }
        public string IntermediateAssemblyExtension { get; set; }
    }
}