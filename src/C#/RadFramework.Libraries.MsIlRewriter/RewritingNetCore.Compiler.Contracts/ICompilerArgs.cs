namespace RewritingNetCore.Compiler.Contracts
{
    public interface ICompilerArgs
    {
        string LibFolder { get; set; }
        string CoreRewritingMiddleware { get; set; }
        string CoreRewritingDependencies { get; set; }
        string MSBuildProjectDirectory { get; set; }
        string MSBuildProjectFile { get; set; }
        string IntermediateOutputPath { get; set; }
        string IntermediateAssemblyName { get; set; }
        string IntermediateAssemblyExtension { get; set; }
        
        string SolutionPath { get; set; }
    }
}