using RadFramework.Libraries.Pipelines.Base;

namespace RadFramework.Libraries.Pipelines;

public class ExtensionPipeContext<TOutput> : ExtensionPipeContextBase
{
    public TOutput ReturnValue { get; set; }

    public void Return(TOutput returnValue)
    {
        ShouldReturn = true;
        ReturnValue = returnValue;
    }
}