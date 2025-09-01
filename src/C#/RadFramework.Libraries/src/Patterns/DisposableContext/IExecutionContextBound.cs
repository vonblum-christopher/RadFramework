namespace RadFramework.Libraries.Patterns.DisposableContext
{
    public interface IExecutionContextBound : IDisposable
    {
        Action<IExecutionContextBound> OnDispose { get; set; }
    }
}