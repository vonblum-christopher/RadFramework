using RadFramework.Libraries.Abstractions;
using RadFramework.Libraries.Ioc.ConstructionLambdaFactory;
using RadFramework.Libraries.Reflection.Caching.Queries;

namespace RadFramework.Libraries.Ioc.Builder;

public class IocContainerBuilder<TIocKey> : ICloneable<IocContainerBuilder<TIocKey>> where TIocKey : ICloneable<TIocKey>
{
    private IocBuilderRegistry<TIocKey> registrations = new();
    
    public InjectionOptions InjectionOptions { get; set; } = new()
    {
        ChooseInjectionConstructor = ctors => ctors
            .OrderByDescending(c => c.Query(MethodBaseQueries.GetParameters).Length)
            .First(),
                
        ConstructorParameterInjection = infos => infos
    };

    public IocBuilderRegistry<TIocKey> IocBuilderRegistry => registrations;

    public IocContainerBuilder<TIocKey> RegisterTransient(TIocKey key, Type tImplementation)
    {
        registrations[key] = new IocDependency<TIocKey>
        {
            Key = key,
            ImplementationType = tImplementation,
            InjectionOptions = InjectionOptions,
            IocLifecycle = IocLifecycles.Transient
        };

        return this;
    }

    public IocContainerBuilder<TIocKey> RegisterTransient<TInterface, TImplementation>()
    {
        TIocKey key = new TIocKey { KeyType = typeof(TInterface)};
        
        registrations[key] = new IocDependency<TIocKey>
        {
            Key = key,
            ImplementationType = typeof(TImplementation),
            InjectionOptions = InjectionOptions,
            IocLifecycle = IocLifecycles.Transient
        };
        
        return this;
    }

    public IocContainerBuilder<TIocKey> RegisterTransient(Type tImplementation)
    {
        var key = new NamedIocKey { KeyType = tImplementation};
        
        registrations[key] = new IocDependency
        {
            Key = key,
            ImplementationType = tImplementation,
            InjectionOptions = InjectionOptions,
            IocLifecycle = IocLifecycles.Transient
        };
        
        return this;
    }
    
    public IocContainerBuilder<TIocKey> RegisterTransient<TImplementation>()
    {
        var key = new NamedIocKey { KeyType = typeof(TImplementation)};
        
        registrations[key] = new IocDependency
        {
            Key = key,
            ImplementationType = key.KeyType,
            InjectionOptions = InjectionOptions,
            IocLifecycle = IocLifecycles.Transient
        };
        
        return this;
    }

    public IocContainerBuilder<TIocKey> RegisterSemiAutomaticTransient(Type tImplementation, Func<TypeOnlyIocContainer, object> construct)
    {
        var key = new NamedIocKey { KeyType = tImplementation};
        
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
    
    public IocContainerBuilder<TIocKey> RegisterSemiAutomaticTransient<TImplementation>(Func<TypeOnlyIocContainer, object> construct)
    {
        Type tImplementation = typeof(TImplementation);
        
        var key = new NamedIocKey { KeyType = tImplementation};
        
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
    
    public IocContainerBuilder<TIocKey> RegisterSingleton(Type tInterface, Type tImplementation)
    {
        var key = new NamedIocKey { KeyType = tInterface};
        
        registrations[key] = new IocDependency
        {
            Key = key,
            ImplementationType = tImplementation,
            InjectionOptions = InjectionOptions,
            IocLifecycle = IocLifecycles.Singleton
        };
        
        return this;
    }

    public IocContainerBuilder<TIocKey> RegisterSingleton<TInterface, TImplementation>()
    {
        var key = new NamedIocKey { KeyType = typeof(TInterface)};
        
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

    public IocContainerBuilder<TIocKey> RegisterSingleton(Type tImplementation)
    {
        var key = new NamedIocKey { KeyType = tImplementation};
        
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
    
    public IocContainerBuilder<TIocKey> RegisterSingleton<TImplementation>()
    {
        Type tImplementation = typeof(TImplementation);
        
        var key = new NamedIocKey { KeyType = tImplementation};
        
        registrations[key] = new IocDependency
        {
                Key = key,
                ImplementationType = tImplementation,
                InjectionOptions = InjectionOptions,
                IocLifecycle = IocLifecycles.Singleton
            };
        
        return this;
    }

    public IocContainerBuilder<TIocKey> RegisterSemiAutomaticSingleton(Type tImplementation, Func<TypeOnlyIocContainer, object> construct)
    {
        var key = new NamedIocKey { KeyType = tImplementation};
        
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
    
    public IocContainerBuilder<TIocKey> RegisterSemiAutomaticSingleton<TImplementation>(Func<TypeOnlyIocContainer, object> construct)
    {
        var key = new NamedIocKey { KeyType = typeof(TImplementation)  };
        
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

    public IocContainerBuilder<TIocKey> Clone()
    {
        return new IocContainerBuilder<TIocKey>
        {
            InjectionOptions = InjectionOptions.Clone(),
            registrations = registrations.Clone()
        };
    }
}