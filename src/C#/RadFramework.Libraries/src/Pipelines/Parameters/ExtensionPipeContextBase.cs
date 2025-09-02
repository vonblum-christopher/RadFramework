namespace RadFramework.Libraries.Pipelines.Parameters;

public class ExtensionPipeContextBase
{
    public Dictionary<string, object> ExtensionPipeData { get; set; } = new();
    public bool ShouldReturn { get; set; }
    public Exception Exception { get; set; }
    public bool Errored { get; set; }
    public object ReturnValue { get; set; }
    public void Return()
    {
        ShouldReturn = true;
    }

    public void Error(Exception e = null)
    {
        Exception = e;
        Errored = true;
        Return();
    }
}