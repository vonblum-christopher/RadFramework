namespace RadFramework.Libraries.Ioc.Core;

public class IocNamedDependencyAttribute : Attribute
{
    public IocKey Key { get; set; }
    
    public IocNamedDependencyAttribute()
    {
    }
    
    public IocNamedDependencyAttribute(string key)
    {
        
    }
    
    public IocNamedDependencyAttribute(Type keyType, string key)
    {
        // resolve by key overridable
        
        //add additional dependencies dictionary for register (child container)
    }
}