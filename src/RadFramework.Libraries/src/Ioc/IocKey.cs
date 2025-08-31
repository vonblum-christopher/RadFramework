using RadFramework.Libraries.Reflection.Caching;

namespace RadFramework.Libraries.Ioc;

public class IocKey
{
    public string Key { get; set; }
    public CachedType RegistrationKeyType { get; set; }
    
    protected bool Equals(IocKey other)
    {
        return Key == other.Key && other.RegistrationKeyType == RegistrationKeyType;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Key, RegistrationKeyType);
    }

    public override bool Equals(object obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((IocKey)obj);
    }
}