using RadFramework.Libraries.Reflection.Caching;

namespace RadFramework.Libraries.Patterns.Pipeline
{
    [Serializable]
    public class PipeDefinition
    {
        public string Key { get; }
        
        public CachedType Type { get; }

        public PipeDefinition(CachedType type, string key = null)
        {
            Type = type;
            Key = key ?? (type.InnerMetaData.MetadataToken + type.InnerMetaData.AssemblyQualifiedName);
        }
    }
}