namespace RadFramework.Libraries.Patterns.Pipeline;

public class ExtensionPipeContextBase : ICloneable
{
    public Dictionary<string, object> ExtensionPipeData { get; set; } = new Dictionary<string, object>();
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

    public object Clone()
    {
        throw new NotImplementedException();
    }
}