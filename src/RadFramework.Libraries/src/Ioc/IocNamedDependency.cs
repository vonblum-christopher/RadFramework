namespace RadFramework.Libraries.Ioc;

public class IocNamedDependency : Attribute
{
    public IocKey Key { get; set; }
    
    public IocNamedDependency()
    {
    }
    
    public IocNamedDependency(string key)
    {
        
    }
    
    public IocNamedDependency(Type keyType, string key)
    {
        // resolve by key overridable
        
        //add additional dependencies dictionary for register (child container)
    }
}