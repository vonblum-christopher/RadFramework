using RadFramework.Libraries.Abstractions;
using RadFramework.Libraries.Reflection.Caching;

namespace RadFramework.Libraries.Ioc.Core;

public class IocKey : ICloneable<IocKey>
{
    public CachedType KeyType { get; set; }
    public string KeyString { get; set; }
    
    protected bool Equals(IocKey other)
    {
        return KeyString == other.KeyString && other.KeyType == KeyType;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(KeyString, KeyType);
    }

    public IocKey Clone()
    {
        return new IocKey()
        {
            KeyString = KeyString,
            KeyType = KeyType
        };
    }

    public override bool Equals(object obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((IocKey)obj);
    }
}