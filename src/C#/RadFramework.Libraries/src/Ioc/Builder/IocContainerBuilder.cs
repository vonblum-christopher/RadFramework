using System.Collections.Concurrent;
using RadFramework.Libraries.Abstractions;
using RadFramework.Libraries.Ioc.Factory;
using RadFramework.Libraries.Ioc.Registrations;
using RadFramework.Libraries.Reflection.Caching.Queries;

namespace RadFramework.Libraries.Ioc.Core;

public class IocContainerBuilder : ICloneable<IocContainerBuilder>
{
    private IocRegistry registrations = new();

    private ServiceFactoryLambdaGenerator lambdaGenerator = new ServiceFactoryLambdaGenerator();
    
    public InjectionOptions InjectionOptions { get; set; }  = new InjectionOptions
    {
        ChooseInjectionConstructor = ctors => ctors
            .OrderByDescending(c => c.Query(MethodBaseQueries.GetParameters).Length)
            .First(),
                
        ConstructorParameterInjection = infos => infos
    };
    
    public IocContainerBuilder RegisterTransient(Type tInterface, Type tImplementation)
    {
        var key = new IocKey { RegistrationKeyType = tInterface };

        registrations.Registrations[key] = new IocService()
        {
            Key = key,
            InjectionOptions = InjectionOptions.Clone(),
            ImplementationType = tImplementation,
              
        };

        return this;
    }

    public IocContainerBuilder RegisterTransient<TInterface, TImplementation>()
    {
        var key = new IocKey { RegistrationKeyType = typeof(TInterface) };
        
        return registrations[key] = new TransientRegistration(key, typeof(TImplementation), lambdaGenerator, this)
        {
            InjectionOptions = InjectionOptions.Clone()
        };
        return this;
    }

    public IocContainerBuilder RegisterTransient(Type tImplementation)
    {
        var key = new IocKey { RegistrationKeyType = tImplementation };
        return registrations[key] = new TransientRegistration(key, tImplementation, lambdaGenerator, this)
        {
            InjectionOptions = InjectionOptions.Clone()
        };
        return this;
    }
    
    public IocContainerBuilder RegisterTransient<TImplementation>()
    {
        var key = new IocKey { RegistrationKeyType = typeof(TImplementation) };
        registrations[new IocKey { RegistrationKeyType = key.RegistrationKeyType }] = new TransientRegistration(key, key.RegistrationKeyType, lambdaGenerator, this)
        {
            InjectionOptions = InjectionOptions.Clone()
        };
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
        };
        return this;
    }

    public IocContainerBuilder RegisterSingleton<TInterface, TImplementation>()
    {
        var key = new IocKey { RegistrationKeyType = typeof(TInterface) };
        
        registrations[key] = new SingletonRegistration(key, typeof(TImplementation), lambdaGenerator, this)
        {
            InjectionOptions = InjectionOptions.Clone()
        };
        return this;
    }

    public IocContainerBuilder RegisterSingleton(Type tImplementation)
    {
        var key = new IocKey { RegistrationKeyType = tImplementation };
        
        registrations[key] = new SingletonRegistration(key, tImplementation, lambdaGenerator, this)
        {
            InjectionOptions = InjectionOptions.Clone()
        };
        return this;
    }
    
    public IocContainerBuilder RegisterSingleton<TImplementation>()
    {
        Type tImplementation = typeof(TImplementation);
        var key = new IocKey { RegistrationKeyType = tImplementation};
        registrations[key] = new SingletonRegistration(key, tImplementation, lambdaGenerator, this)
        {
            InjectionOptions = InjectionOptions.Clone()
        };
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

    public IocContainerBuilder Clone()
    {
        return new IocContainerBuilder()
        {
            InjectionOptions = InjectionOptions.Clone(),
            lambdaGenerator = lambdaGenerator,
            registrations = registrations.Clone()
        };
    }
}