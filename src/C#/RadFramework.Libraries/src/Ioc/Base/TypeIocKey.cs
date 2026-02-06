using RadFramework.Libraries.Abstractions;
using RadFramework.Libraries.Reflection.Caching;

namespace RadFramework.Libraries.Ioc.Base;

public class TypeIocKey : IIocKey
{
    public CachedType KeyType { get; set; }
    
    public SortedDictionary<string, object> KeyProperties =>
        new()
        {
            { nameof(KeyType), KeyType }
        }; 
    
    public IIocKey Clone() =>
        new TypeIocKey()
            {
                KeyType = KeyType
            };
}