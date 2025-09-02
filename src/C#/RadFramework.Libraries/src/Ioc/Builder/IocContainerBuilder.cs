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
        var key = new IocKey { KeyType = tInterface };
        
        registrations[key] = new IocService()
        {
            Key = key,
            ImplementationType = tImplementation,
            InjectionOptions = InjectionOptions,
            IocLifecycles = IocLifecycles.Transient
        };

        return this;
    }

    public IocContainerBuilder RegisterTransient<TInterface, TImplementation>()
    {
        var key = new IocKey { KeyType = typeof(TInterface)};
        
        registrations[key] = new IocService()
        {
            Key = key,
            ImplementationType = typeof(TImplementation),
            InjectionOptions = InjectionOptions,
            IocLifecycles = IocLifecycles.Transient
        };
        
        return this;
    }

    public IocContainerBuilder RegisterTransient(Type tImplementation)
    {
        var key = new IocKey { KeyType = tImplementation};
        
        registrations[key] = new IocService()
        {
            Key = key,
            ImplementationType = tImplementation,
            InjectionOptions = InjectionOptions,
            IocLifecycles = IocLifecycles.Transient
        };
        
        return this;
    }
    
    public IocContainerBuilder RegisterTransient<TImplementation>()
    {
        var key = new IocKey { KeyType = typeof(TImplementation)};
        
        registrations[key] = new IocService()
        {
            Key = key,
            ImplementationType = key.KeyType,
            InjectionOptions = InjectionOptions,
            IocLifecycles = IocLifecycles.Transient
        };
        
        return this;
    }

    public IocContainerBuilder RegisterSemiAutomaticTransient(Type tImplementation, Func<Core.IocContainer, object> construct)
    {
        var key = new IocKey { KeyType = tImplementation};
        
        registrations[key] = new IocService()
        {
            Key = key,
            ImplementationType = tImplementation,
            InjectionOptions = InjectionOptions,
            IocLifecycles = IocLifecycles.Transient
        };
        
        return this;
    }
    
    public IocContainerBuilder RegisterSemiAutomaticTransient<TImplementation>(Func<Core.IocContainer, object> construct)
    {
        Type tImplementation = typeof(TImplementation);
        
        var key = new IocKey { KeyType = tImplementation};
        
        registrations[key] = new IocService()
        {
            Key = key,
            ImplementationType = tImplementation,
            InjectionOptions = InjectionOptions,
            IocLifecycles = IocLifecycles.Transient
        };
        
        return this;
    }
    
    public IocContainerBuilder RegisterSingleton(Type tInterface, Type tImplementation)
    {
        var key = new IocKey { KeyType = tInterface};
        
        registrations[key] = new IocService()
        {
            Key = key,
            ImplementationType = tImplementation,
            InjectionOptions = InjectionOptions,
            IocLifecycles = IocLifecycles.Singleton
        };
        
        return this;
    }

    public IocContainerBuilder RegisterSingleton<TInterface, TImplementation>()
    {
        var key = new IocKey { KeyType = typeof(TInterface)};
        
        registrations[key] =
            new IocService()
            {
                Key = key,
                ImplementationType = typeof(TImplementation),
                InjectionOptions = InjectionOptions,
                IocLifecycles = IocLifecycles.Singleton
            };
        
        return this;
    }

    public IocContainerBuilder RegisterSingleton(Type tImplementation)
    {
        var key = new IocKey { KeyType = tImplementation};
        
        registrations[key] =
            new IocService()
            {
                Key = key,
                ImplementationType = key.KeyType,
                InjectionOptions = InjectionOptions,
                IocLifecycles = IocLifecycles.Singleton
            };
        
        return this;
    }
    
    public IocContainerBuilder RegisterSingleton<TImplementation>()
    {
        Type tImplementation = typeof(TImplementation);
        
        var key = new IocKey { KeyType = tImplementation};
        
        registrations[key] = new IocService()
            {
                Key = key,
                ImplementationType = tImplementation,
                InjectionOptions = InjectionOptions,
                IocLifecycles = IocLifecycles.Singleton
            };
        
        return this;
    }

    public IocContainerBuilder RegisterSemiAutomaticSingleton(Type tImplementation, Func<Core.IocContainer, object> construct)
    {
        var key = new IocKey { KeyType = tImplementation};
        
        registrations[key] =
            new IocService()
            {
                Key = key,
                ImplementationType = key.KeyType,
                InjectionOptions = InjectionOptions,
                IocLifecycles = IocLifecycles.Singleton
            };
        
        return this;
    }
    
    public IocContainerBuilder RegisterSemiAutomaticSingleton<TImplementation>(Func<Core.IocContainer, object> construct)
    {
        var key = new IocKey { KeyType = typeof(TImplementation)  };
        
        registrations[key] =
            new IocService()
            {
                Key = key,
                ImplementationType = key.KeyType,
                InjectionOptions = InjectionOptions,
                IocLifecycles = IocLifecycles.Singleton
            };
        
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