using System.Collections.Concurrent;
using RadFramework.Libraries.Ioc.Factory;
using RadFramework.Libraries.Ioc.Registrations;

namespace RadFramework.Libraries.Ioc.Core;

public class IocContainerBuilder
{
    private ConcurrentDictionary<IocKey, RegistrationBase> registrations = new();

    private ServiceFactoryLambdaGenerator gen;
    
    public IocContainerBuilder RegisterTransient(Type tInterface, Type tImplementation)
    {
        var key = new IocKey { RegistrationKeyType = tInterface };
        
        return (registrations[key] = new TransientRegistration(key, tImplementation, LambdaGenerator, this)
        {
            InjectionOptions = InjectionOptions.Clone()
        }).InjectionOptions;

        return this;
    }

    public IocContainerBuilder RegisterTransient<TInterface, TImplementation>()
    {
        var key = new IocKey { RegistrationKeyType = typeof(TInterface) };
        
        return (registrations[key] = new TransientRegistration(key, typeof(TImplementation), LambdaGenerator, this)
        {
            InjectionOptions = InjectionOptions.Clone()
        }).InjectionOptions;
    }

    public IocContainerBuilder RegisterTransient(Type tImplementation)
    {
        var key = new IocKey { RegistrationKeyType = tImplementation };
        return (registrations[key] = new TransientRegistration(key, tImplementation, LambdaGenerator, this)
        {
            InjectionOptions = InjectionOptions.Clone()
        }).InjectionOptions;
    }
    
    public IocContainerBuilder RegisterTransient<TImplementation>()
    {
        var key = new IocKey { RegistrationKeyType = typeof(TImplementation) };
        return (registrations[new IocKey { RegistrationKeyType = key.RegistrationKeyType }] = new TransientRegistration(key, key.RegistrationKeyType, LambdaGenerator, this)
        {
            InjectionOptions = InjectionOptions.Clone()
        }).InjectionOptions;
    }

    public IocContainerBuilder RegisterSemiAutomaticTransient(Type tImplementation, Func<Core.IocContainer, object> construct)
    {
        registrations[new IocKey { RegistrationKeyType = tImplementation }] = new TransientFactoryRegistration(construct, this);
    }
    
    public IocContainerBuilder RegisterSemiAutomaticTransient<TImplementation>(Func<Core.IocContainer, object> construct)
    {
        registrations[new IocKey { RegistrationKeyType = typeof(TImplementation) }] = new TransientFactoryRegistration(construct, this);
    }

    
    public IocContainerBuilder RegisterSingleton(Type tInterface, Type tImplementation)
    {
        var key = new IocKey { RegistrationKeyType = tInterface };
        
        return (registrations[key] = new SingletonRegistration(key,tImplementation, LambdaGenerator, this)
        {
            InjectionOptions = InjectionOptions.Clone()
        }).InjectionOptions;
    }

    public IocContainerBuilder RegisterSingleton<TInterface, TImplementation>()
    {
        var key = new IocKey { RegistrationKeyType = typeof(TInterface) };
        
        return (registrations[key] = new SingletonRegistration(key, typeof(TImplementation), LambdaGenerator, this)
        {
            InjectionOptions = InjectionOptions.Clone()
        }).InjectionOptions;
    }

    public IocContainerBuilder RegisterSingleton(Type tImplementation)
    {
        var key = new IocKey { RegistrationKeyType = tImplementation };
        
        return (registrations[key] = new SingletonRegistration(key, tImplementation, LambdaGenerator, this)
        {
            InjectionOptions = InjectionOptions.Clone()
        }).InjectionOptions;
    }
    
    public IocContainerBuilder RegisterSingleton<TImplementation>()
    {
        Type tImplementation = typeof(TImplementation);
        var key = new IocKey { RegistrationKeyType = tImplementation};
        return (registrations[key] = new SingletonRegistration(key, tImplementation, LambdaGenerator, this)
        {
            InjectionOptions = InjectionOptions.Clone()
        }).InjectionOptions;
    }

    public IocContainerBuilder RegisterSemiAutomaticSingleton(Type tImplementation, Func<Core.IocContainer, object> construct)
    {
        registrations[new IocKey(){ RegistrationKeyType = tImplementation}] = new SingletonFactoryRegistration(construct, this);
    }
    
    public IocContainerBuilder RegisterSemiAutomaticSingleton<TImplementation>(Func<Core.IocContainer, object> construct)
    {
        registrations[new IocKey(){ RegistrationKeyType = typeof(TImplementation)}] = new SingletonFactoryRegistration(construct, this);
    }

    // Deprecated. Impossible to cleanly clone such an container
    /*public void RegisterSingletonInstance(Type tInterface, object instance)
    {
        registrations[new IocKey(){ RegistrationKeyType = tInterface}] = new SingletonInstanceRegistration(instance);
    }
    
    public void RegisterSingletonInstance<TInterface>(object instance)
    { //everything thats not IocKey Driven turns extensionMethod
        registrations[new IocKey(){ RegistrationKeyType = typeof(TInterface)}] = new SingletonInstanceRegistration(instance);
    }*/
}