using RadFramework.Libraries.Reflection.Caching;

namespace RadFramework.Libraries.Ioc.Base;

public class NameAndTypeIocKey : IIocKey
{
    public string KeyName { get; set; }
    public CachedType KeyType { get; set; }
    
    public SortedDictionary<string, object> KeyProperties =>
        new()
        {
            { nameof(KeyType), KeyType },
            { nameof(KeyName), KeyName }
        };

    public IIocKey Clone()
    {
        return new NameAndTypeIocKey
        {
            KeyName = KeyName,
            KeyType = KeyType
        };
    }
}