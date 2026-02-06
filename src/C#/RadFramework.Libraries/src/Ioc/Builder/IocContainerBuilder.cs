using RadFramework.Libraries.Abstractions;
using RadFramework.Libraries.Ioc.Base;
using RadFramework.Libraries.Ioc.ConstructionLambdaFactory;
using RadFramework.Libraries.Reflection.Caching.Queries;

namespace RadFramework.Libraries.Ioc.Builder;

public class IocContainerBuilder : ICloneable<IocContainerBuilder>
{
    private IocBuilderRegistry registrations = new();
    
    public InjectionOptions InjectionOptions { get; set; } = new()
    {
        ChooseInjectionConstructor = ctors => ctors
            .OrderByDescending(c => c.Query(MethodBaseQueries.GetParameters).Length)
            .First(),
                
        ConstructorParameterInjection = infos => infos
    };

    public IocBuilderRegistry IocBuilderRegistry => registrations;

    public IocContainerBuilder RegisterTransient(IIocKey key, Type tImplementation)
    {
        registrations[key] = new IocDependency
        {
            Key = key,
            ImplementationType = tImplementation,
            InjectionOptions = InjectionOptions,
            IocLifecycle = IocLifecycles.Transient
        };

        return this;
    }

    public IocContainerBuilder RegisterTransient<TInterface, TImplementation>()
    {
        IIocKey key = new TypeIocKey() { KeyType = typeof(TInterface)};
        
        registrations[key] = new IocDependency
        {
            Key = key,
            ImplementationType = typeof(TImplementation),
            InjectionOptions = InjectionOptions,
            IocLifecycle = IocLifecycles.Transient
        };
        
        return this;
    }

    public IocContainerBuilder RegisterTransient(Type tImplementation)
    {
        var key = new TypeIocKey() { KeyType = tImplementation};
        
        registrations[key] = new IocDependency
        {
            Key = key,
            ImplementationType = tImplementation,
            InjectionOptions = InjectionOptions,
            IocLifecycle = IocLifecycles.Transient
        };
        
        return this;
    }
    
    public IocContainerBuilder RegisterTransient<TImplementation>()
    {
        var key = new TypeIocKey() { KeyType = typeof(TImplementation)};
        
        registrations[key] = new IocDependency
        {
            Key = key,
            ImplementationType = key.KeyType,
            InjectionOptions = InjectionOptions,
            IocLifecycle = IocLifecycles.Transient
        };
        
        return this;
    }

    public IocContainerBuilder RegisterSemiAutomaticTransient(Type tImplementation, Func<IocDependency, IIocContainer, object> construct)
    {
        var key = new TypeIocKey() { KeyType = tImplementation};
        
        registrations[key] = new IocDependency
        {
            Key = key,
            ImplementationType = tImplementation,
            InjectionOptions = InjectionOptions,
            IocLifecycle = IocLifecycles.Transient,
            FactoryFunc = construct
        };
        
        return this;
    }
    
    public IocContainerBuilder RegisterSemiAutomaticTransient<TImplementation>(Func<IocDependency, IIocContainer, object> construct)
    {
        Type tImplementation = typeof(TImplementation);
        
        var key = new TypeIocKey() { KeyType = tImplementation};
        
        registrations[key] = new IocDependency
        {
            Key = key,
            ImplementationType = tImplementation,
            InjectionOptions = InjectionOptions,
            IocLifecycle = IocLifecycles.Transient,
            FactoryFunc = construct
        };
        
        return this;
    }
    
    public IocContainerBuilder RegisterSingleton(Type tInterface, Type tImplementation)
    {
        var key = new TypeIocKey { KeyType = tInterface};
        
        registrations[key] = new IocDependency
        {
            Key = key,
            ImplementationType = tImplementation,
            InjectionOptions = InjectionOptions,
            IocLifecycle = IocLifecycles.Singleton
        };
        
        return this;
    }

    public IocContainerBuilder RegisterSingleton<TInterface, TImplementation>()
    {
        var key = new TypeIocKey { KeyType = typeof(TInterface)};
        
        registrations[key] =
            new IocDependency
            {
                Key = key,
                ImplementationType = typeof(TImplementation),
                InjectionOptions = InjectionOptions,
                IocLifecycle = IocLifecycles.Singleton
            };
        
        return this;
    }

    public IocContainerBuilder RegisterSingleton(Type tImplementation)
    {
        var key = new TypeIocKey { KeyType = tImplementation};
        
        registrations[key] =
            new IocDependency
            {
                Key = key,
                ImplementationType = key.KeyType,
                InjectionOptions = InjectionOptions,
                IocLifecycle = IocLifecycles.Singleton
            };
        
        return this;
    }
    
    public IocContainerBuilder RegisterSingleton<TImplementation>()
    {
        Type tImplementation = typeof(TImplementation);
        
        var key = new TypeIocKey { KeyType = tImplementation};
        
        registrations[key] = new IocDependency
        {
                Key = key,
                ImplementationType = tImplementation,
                InjectionOptions = InjectionOptions,
                IocLifecycle = IocLifecycles.Singleton
            };
        
        return this;
    }

    public IocContainerBuilder RegisterSemiAutomaticSingleton(Type tImplementation, Func<IocDependency, IIocContainer, object> construct)
    {
        var key = new TypeIocKey { KeyType = tImplementation};
        
        registrations[key] =
            new IocDependency
            {
                Key = key,
                ImplementationType = key.KeyType,
                InjectionOptions = InjectionOptions,
                IocLifecycle = IocLifecycles.Singleton,
                FactoryFunc = construct
            };
        
        return this;
    }
    
    public IocContainerBuilder RegisterSemiAutomaticSingleton<TImplementation>(Func<IocDependency, IIocContainer, object> construct)
    {
        var key = new TypeIocKey { KeyType = typeof(TImplementation)  };
        
        registrations[key] =
            new IocDependency
            {
                Key = key,
                ImplementationType = key.KeyType,
                InjectionOptions = InjectionOptions,
                IocLifecycle = IocLifecycles.Singleton,
                FactoryFunc = construct
            };
        
        return this;
    }

    public TypeOnlyIocContainer CreateContainer()
    {
        return new TypeOnlyIocContainer(this);
    }

    public IocContainerBuilder Clone()
    {
        return new IocContainerBuilder
        {
            InjectionOptions = InjectionOptions.Clone(),
            registrations = registrations.Clone()
        };
    }
}