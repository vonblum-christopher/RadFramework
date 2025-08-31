namespace RadFramework.Libraries.Patterns.Pipeline;

public class ExtensionPipeContext
{
    public bool ShouldReturn { get; set; }

    public Dictionary<string, object> ExtensionPipeTags { get; set; } = new Dictionary<string, object>();
    
    public void Return()
    {
        ShouldReturn = true;
    }
}