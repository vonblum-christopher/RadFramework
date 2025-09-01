namespace RadFramework.Libraries.Pipelines.Parameters;

public class ExtensionPipeContext<TOutput> : ExtensionPipeContextBase
{
    public TOutput ReturnValue { get; set; }

    public void Return(TOutput returnValue)
    {
        ShouldReturn = true;
        ReturnValue = returnValue;
    }
}