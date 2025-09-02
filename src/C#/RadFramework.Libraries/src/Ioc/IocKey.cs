using RadFramework.Libraries.Abstractions;
using RadFramework.Libraries.Reflection.Caching;

namespace RadFramework.Libraries.Ioc;

public class IocKey : ICloneable<IocKey>
{
    public CachedType KeyType { get; set; }
    public string KeyName { get; set; }
    
    protected bool Equals(IocKey other)
    {
        return KeyName == other.KeyName && other.KeyType == KeyType;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(KeyName, KeyType);
    }

    public IocKey Clone()
    {
        return new IocKey
        {
            KeyName = KeyName,
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
    
    public static bool operator ==(IocKey a, IocKey b)
    {
        return b.Equals(a);
    }

    public static bool operator !=(IocKey a, IocKey b)
    {
        return !b.Equals(a);
    }
}