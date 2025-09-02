using RadFramework.Libraries.Ioc.Builder;

namespace RadFramework.Libraries.Ioc;

public class IocDependencyAttribute : Attribute
{
    public IocKey Key { get; set; } = new IocKey();
    
    public IocDependencyAttribute()
    {
    }
    
    public IocDependencyAttribute(string key)
    {
        Key.KeyName = key;
    }
    
    public IocDependencyAttribute(Type keyType, string key)
    {
        Key.KeyName = key;
        Key.KeyType = keyType;
    }
}