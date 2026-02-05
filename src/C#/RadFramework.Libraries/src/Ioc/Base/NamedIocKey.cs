using RadFramework.Libraries.Abstractions;
using RadFramework.Libraries.Reflection.Caching;

namespace RadFramework.Libraries.Ioc;

public struct NamedIocKey : ICloneable<NamedIocKey>
{
    public CachedType KeyType { get; set; }
    public string KeyName { get; set; }
    
    public bool Equals(NamedIocKey other)
    {
        return KeyName == other.KeyName && other.KeyType == KeyType;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(KeyName, KeyType);
    }

    public NamedIocKey Clone()
    {
        return new NamedIocKey
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
        return Equals((NamedIocKey)obj);
    }
    
    public static bool operator ==(NamedIocKey a, NamedIocKey b)
    {
        return b.Equals(a);
    }

    public static bool operator !=(NamedIocKey a, NamedIocKey b)
    {
        return !b.Equals(a);
    }
}