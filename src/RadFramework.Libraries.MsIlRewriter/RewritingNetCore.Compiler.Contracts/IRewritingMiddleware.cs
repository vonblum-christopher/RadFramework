namespace RewritingNetCore.Compiler.Contracts
{
    public interface IRewritingMiddleware
    {
        void ApplyRewriting(ICompilerArgs compilerArgs);
    }
}