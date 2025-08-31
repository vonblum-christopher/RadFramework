namespace RadFramework.Libraries.Patterns.Pipeline;

public interface IExtensionPipe<TContext>
{
    void Process(TContext connection, ExtensionPipeContext pipeContext);
}