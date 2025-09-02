using System.Collections.Concurrent;
using RadFramework.Libraries.Ioc.Factory;
using RadFramework.Libraries.Ioc.Registrations;

namespace RadFramework.Libraries.Ioc.Core;

public class IocContainerBuilder : ICloneable
{
    public InjectionOptions InjectionOptions { get; set; }
    private ConcurrentDictionary<IocKey, RegistrationBase> registrations = new();

    private ServiceFactoryLambdaGenerator lambdaGenerator;
    
    public IocContainerBuilder RegisterTransient(Type tInterface, Type tImplementation)
    {
        var key = new IocKey { RegistrationKeyType = tInterface };
        
        registrations[key] = new TransientRegistration(key, tImplementation, lambdaGenerator, this)
        {
            InjectionOptions = InjectionOptions.Clone()
        }.InjectionOptions;

        return this;
    }

    public IocContainerBuilder RegisterTransient<TInterface, TImplementation>()
    {
        var key = new IocKey { RegistrationKeyType = typeof(TInterface) };
        
        return registrations[key] = new TransientRegistration(key, typeof(TImplementation), lambdaGenerator, this)
        {
            InjectionOptions = InjectionOptions.Clone()
        }.InjectionOptions;
        return this;
    }

    public IocContainerBuilder RegisterTransient(Type tImplementation)
    {
        var key = new IocKey { RegistrationKeyType = tImplementation };
        return registrations[key] = new TransientRegistration(key, tImplementation, lambdaGenerator, this)
        {
            InjectionOptions = InjectionOptions.Clone()
        }.InjectionOptions;
        return this;
    }
    
    public IocContainerBuilder RegisterTransient<TImplementation>()
    {
        var key = new IocKey { RegistrationKeyType = typeof(TImplementation) };
        registrations[new IocKey { RegistrationKeyType = key.RegistrationKeyType }] = new TransientRegistration(key, key.RegistrationKeyType, lambdaGenerator, this)
        {
            InjectionOptions = InjectionOptions.Clone()
        }.InjectionOptions;
        return this;
    }

    public IocContainerBuilder RegisterSemiAutomaticTransient(Type tImplementation, Func<Core.IocContainer, object> construct)
    {
        registrations[new IocKey { RegistrationKeyType = tImplementation }] = new TransientFactoryRegistration(construct, this);
        return this;
    }
    
    public IocContainerBuilder RegisterSemiAutomaticTransient<TImplementation>(Func<Core.IocContainer, object> construct)
    {
        registrations[new IocKey { RegistrationKeyType = typeof(TImplementation) }] = new TransientFactoryRegistration(construct, this);
        return this;
    }
    
    public IocContainerBuilder RegisterSingleton(Type tInterface, Type tImplementation)
    {
        var key = new IocKey { RegistrationKeyType = tInterface };
        
        registrations[key] = new SingletonRegistration(key,tImplementation, lambdaGenerator, this)
        {
            InjectionOptions = InjectionOptions.Clone()
        }.InjectionOptions;
        return this;
    }

    public IocContainerBuilder RegisterSingleton<TInterface, TImplementation>()
    {
        var key = new IocKey { RegistrationKeyType = typeof(TInterface) };
        
        registrations[key] = new SingletonRegistration(key, typeof(TImplementation), lambdaGenerator, this)
        {
            InjectionOptions = InjectionOptions.Clone()
        }.InjectionOptions;
        return this;
    }

    public IocContainerBuilder RegisterSingleton(Type tImplementation)
    {
        var key = new IocKey { RegistrationKeyType = tImplementation };
        
        registrations[key] = new SingletonRegistration(key, tImplementation, lambdaGenerator, this)
        {
            InjectionOptions = InjectionOptions.Clone()
        }.InjectionOptions;
        return this;
    }
    
    public IocContainerBuilder RegisterSingleton<TImplementation>()
    {
        Type tImplementation = typeof(TImplementation);
        var key = new IocKey { RegistrationKeyType = tImplementation};
        registrations[key] = new SingletonRegistration(key, tImplementation, lambdaGenerator, this)
        {
            InjectionOptions = InjectionOptions.Clone()
        }.InjectionOptions;
        return this;
    }

    public IocContainerBuilder RegisterSemiAutomaticSingleton(Type tImplementation, Func<Core.IocContainer, object> construct)
    {
        registrations[new IocKey(){ RegistrationKeyType = tImplementation}] = new SingletonFactoryRegistration(construct, this);
        return this;
    }
    
    public IocContainerBuilder RegisterSemiAutomaticSingleton<TImplementation>(Func<Core.IocContainer, object> construct)
    {
        registrations[new IocKey(){ RegistrationKeyType = typeof(TImplementation)}] = new SingletonFactoryRegistration(construct, this);
        return this;
    }

    public object Clone()
    {
        return new IocContainerBuilder()
        {
            InjectionOptions = InjectionOptions.Clone(),
            lambdaGenerator = lambdaGenerator,
            registrations = new ConcurrentDictionary<IocKey, RegistrationBase>(
                (IEnumerable<KeyValuePair<IocKey, RegistrationBase>>)
                registrations
                    .ToDictionary(
                        k => k.Key.Clone(),
                        v => v.Value.Clone()))
        };
    }
}