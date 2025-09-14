using System.Collections.Immutable;
using RadFramework.Libraries.Abstractions;
using RadFramework.Libraries.Ioc.Builder;
using RadFramework.Libraries.Ioc.ConstructionLambdaFactory;
using RadFramework.Libraries.Ioc.Registrations;
using RadFramework.Libraries.Reflection.Caching.Queries;

namespace RadFramework.Libraries.Ioc;

public class IocContainer : IIocContainer, ICloneable<IocContainer>, IServiceProvider
{
    public IEnumerable<IocContainer> ParentContainers { get; private set; } = new List<IocContainer>();
    public InjectionOptions InjectionOptions { get; private set; } = new InjectionOptions();
    public IocRegistry Registry { get; private set; } = new();
    public ImmutableList<IocDependency> ServiceList 
        => Registry.Registrations.Values.ToImmutableList();

    public IImmutableDictionary<IocKey, IocDependency> ServiceLookup
    {
        get =>
            Registry.Registrations.Values
                .ToImmutableDictionary(
                    k => k.Key, 
                    v => v);
    }
    
    public IocContainer(IocRegistry iocRegistry)
    {
        Registry = iocRegistry.Clone();
        InjectionOptions = InjectionOptions.Clone();
    }

    public IocContainer(IocContainerBuilder builder)
    {
        Registry = builder.IocRegistry.Clone();
        InjectionOptions = builder.InjectionOptions.Clone();
    }
    public bool HasService(Type t)
    {
        return HasService(new IocKey() { KeyType = t });
    }

    public bool HasService(string key, Type t)
    {
        return HasService(new IocKey() { KeyType = t, KeyName = key});
    }
    
    public bool HasService(IocKey key)
    {
        return Registry.Registrations.ContainsKey(key);
    }
    
    public object Resolve(string key, Type t)
    {
        return ResolveDependency(new IocKey { KeyType = t, KeyName = key});
    }

    public T Activate<T>()
    {
        return (T)Activate(typeof(T));
    }

    public object Activate(Type t)
    {
        var key = new IocKey { KeyType = t };

        IocDependency iocDependency = new()
        {
            ImplementationType = t,
            InjectionOptions = InjectionOptions,
            Key = key,
            IocLifecycle = IocLifecycles.Transient
        };

        TransientRegistration registration = new()
        {
            IocDependency = iocDependency,
        };
        
        registration.Initialize(iocDependency);
        
        return registration.ResolveService(this, iocDependency);
    }

    public T Resolve<T>()
    {
        return (T)ResolveDependency(new IocKey()
        {
            KeyType = typeof(T)
        });
    }

    public object Resolve(Type tInterface)
    {
        return ResolveDependency(new IocKey()
        {
            KeyType = tInterface
        });
    }

    public object Resolve(Type tInterface, string key)
    {
        var iocKey = new IocKey { KeyType = tInterface, KeyName = key };
            
        return ResolveDependency(iocKey);
    }

    public object Resolve(IocKey key)
    {
        return ResolveDependency(key);
    }

    public object GetService(Type serviceType)
    {
        return ResolveDependency(new IocKey()
        {
            KeyType = serviceType
        });
    }

    private object ResolveDependency(IocKey key)
    {
        if (this.HasService(key))
        {
            IocDependency dependency = this.Registry[key];
            
            return (dependency.FactoryFunc 
                    ?? InjectionLambdaManager.GetOrCreateConstructionFunc(key, dependency))
                        (this);
        }

        foreach (IocContainer fallbackResolver in ParentContainers)
        {
            if (fallbackResolver.HasService(key))
            {
                return fallbackResolver.Resolve(key);
            }
        }

        throw new RegistrationNotFoundException(key.KeyType);
    }

    public IocContainer Clone()
    {
        return new IocContainer(Registry)
        {
            ParentContainers = ParentContainers,
            InjectionOptions = InjectionOptions.Clone()
        };
    }
}