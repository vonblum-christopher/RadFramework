using RadFramework.Libraries.Abstractions;

namespace RadFramework.Libraries.Pipelines.Builder;

public class PipeRegistry : ICloneable<PipeRegistry>
{
    public List<PipeDefinition> PipeDefinitions = new();

    public PipeRegistry Clone()
    {
        return new PipeRegistry
        {
            PipeDefinitions = PipeDefinitions.Select(d => d.Clone()).ToList()
        };
    }
}