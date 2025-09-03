using RadFramework.Libraries.Abstractions;
using RadFramework.Libraries.Reflection.Caching;

namespace RadFramework.Libraries.Pipelines.Builder
{
    public class PipeDefinition : ICloneable<PipeDefinition>
    {
        public string Key { get; private set; }
        
        public CachedType Type { get; }

        public PipeDefinition(CachedType type) : this(type, null)
        {
        }

        public PipeDefinition(CachedType type, string key = null)
        {
            Type = type;
            Key = key;
        }

        public PipeDefinition Clone()
        {
            return new PipeDefinition(Type, Key);
        }
    }
}