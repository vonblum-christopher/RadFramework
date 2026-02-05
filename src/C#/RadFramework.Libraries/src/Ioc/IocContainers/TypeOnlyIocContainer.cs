using System.Collections.Immutable;
using RadFramework.Libraries.Abstractions;
using RadFramework.Libraries.Ioc.Builder;
using RadFramework.Libraries.Ioc.ConstructionLambdaFactory;
using RadFramework.Libraries.Ioc.Registrations;
using RadFramework.Libraries.Reflection.Caching.Queries;

namespace RadFramework.Libraries.Ioc;

public class TypeOnlyIocContainer : ITypeOnlyIocContainer, ICloneable<TypeOnlyIocContainer>, IServiceProvider
{
    public IEnumerable<TypeOnlyIocContainer> ParentContainers { get; private set; } = new List<TypeOnlyIocContainer>();
    public InjectionOptions InjectionOptions { get; private set; } = new InjectionOptions();
    public InstanceContainerRegistry<Type> BuilderRegistry { get; private set; }
    
    public ImmutableList<IocDependency<Type>> ServiceList 
        => BuilderRegistry.Registrations.ToImmutableList();

    public IImmutableDictionary<NamedIocKey, IocDependency> ServiceLookup
    {
        get =>
            BuilderRegistry.Registrations
                .ToImmutableDictionary(
                    k => k.Key, 
                    v => v);
    }
    
    public TypeOnlyIocContainer(IocBuilderRegistry<Type> iocBuilderRegistry)
    {
        BuilderRegistry = new InstanceContainerRegistry<Type>(iocBuilderRegistry);
        InjectionOptions = InjectionOptions.Clone();
    }

    public TypeOnlyIocContainer(IocContainerBuilder builder)
    {
        BuilderRegistry = builder.IocBuilderRegistry.Clone();
        InjectionOptions = builder.InjectionOptions.Clone();
    }
    public bool HasService(Type t)
    {
        return HasService(new NamedIocKey() { KeyType = t });
    }

    public bool HasService(string key, Type t)
    {
        return HasService(new NamedIocKey() { KeyType = t, KeyName = key});
    }
    
    public bool HasService(NamedIocKey key)
    {
        return BuilderRegistry.Registrations.ContainsKey(key);
    }
    
    public object Resolve(string key, Type t)
    {
        return ResolveDependency(new NamedIocKey { KeyType = t, KeyName = key});
    }

    public T Activate<T>()
    {
        return (T)Activate(typeof(T));
    }

    public object Activate(Type t)
    {
        var key = new NamedIocKey { KeyType = t };

        IocDependency iocDependency = new()
        {
            ImplementationType = t,
            InjectionOptions = InjectionOptions,
            Key = key,
            IocLifecycle = IocLifecycles.Transient
        };

        TransientInstanceContainer instanceContainer = new()
        {
            IocDependency = iocDependency,
        };
        
        instanceContainer.Initialize(iocDependency);
        
        return instanceContainer.ResolveService(this, iocDependency);
    }

    public T Resolve<T>()
    {
        return (T)ResolveDependency(new NamedIocKey()
        {
            KeyType = typeof(T)
        });
    }

    public object Resolve(Type tInterface)
    {
        return ResolveDependency(new NamedIocKey()
        {
            KeyType = tInterface
        });
    }

    public object Resolve(Type tInterface, string key)
    {
        var iocKey = new NamedIocKey { KeyType = tInterface, KeyName = key };
            
        return ResolveDependency(iocKey);
    }

    public object Resolve(NamedIocKey key)
    {
        return ResolveDependency(key);
    }

    public object GetService(Type serviceType)
    {
        return ResolveDependency(new NamedIocKey()
        {
            KeyType = serviceType
        });
    }

    private object ResolveDependency(NamedIocKey key)
    {
        if (this.HasService(key))
        {
            IocDependency dependency = this.BuilderRegistry[key];
            
            return (dependency.FactoryFunc 
                    ?? InjectionLambdaManager.GetOrCreateConstructionFunc(key, dependency))
                        (this);
        }

        foreach (TypeOnlyIocContainer fallbackResolver in ParentContainers)
        {
            if (fallbackResolver.HasService(key))
            {
                return fallbackResolver.Resolve(key);
            }
        }

        throw new RegistrationNotFoundException(key.KeyType);
    }

    private InstanceContainerBase GetContainer(IocDependency dependency)
    {
        switch (dependency.IocLifecycle)
        {
            case IocLifecycles.Singleton:
                return new SingletonFactoryInstanceContainer();
                break;
        }
    }

    public TypeOnlyIocContainer Clone()
    {
        return new TypeOnlyIocContainer(BuilderRegistry)
        {
            ParentContainers = ParentContainers,
            InjectionOptions = InjectionOptions.Clone()
        };
    }
}