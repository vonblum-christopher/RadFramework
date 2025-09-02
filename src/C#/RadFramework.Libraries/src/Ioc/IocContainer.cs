using System.Collections.Immutable;
using RadFramework.Libraries.Abstractions;
using RadFramework.Libraries.Ioc.Builder;
using RadFramework.Libraries.Ioc.ConstructionMethodBuilders;
using RadFramework.Libraries.Ioc.Registrations;
using RadFramework.Libraries.Reflection.Caching.Queries;

namespace RadFramework.Libraries.Ioc;

public class IocContainer : IIocContainer, ICloneable<IocContainer>, IServiceProvider
{
    public List<IocContainer> ParentContainers { get; set; } = new();
    public InjectionOptions InjectionOptions;
    public IocRegistry Registry { get; private set; } = new IocRegistry();
    public IEnumerable<IocServiceRegistration> ServiceList
    {
        get
        {
            return Registry.Registrations.Values;
        }
    }

    public IImmutableDictionary<IocKey, IocServiceRegistration> ServiceLookup
    {
        get
        {
            return Registry.Registrations.Values
                .ToImmutableDictionary(
                    k => k.Key, 
                    v => v);
        }
            
    }
    
    public IocContainer(InjectionOptions injectionOptions)
    {
        this.InjectionOptions = injectionOptions;
    }

    public IocContainer(
        IEnumerable<IocContainer> fallbackResolvers,
        InjectionOptions injectionOptions)
    {
        this.ParentContainers = fallbackResolvers.ToList();
        this.InjectionOptions = injectionOptions;
    }

    public IocContainer(IocContainerBuilder builder) : this()
    {
        Registry = builder.IocRegistry.Clone();
    }

    public IocContainer()
    {
        this.InjectionOptions = new InjectionOptions
        {
            ChooseInjectionConstructor = ctors => ctors
                .OrderByDescending(c => c.Query(MethodBaseQueries.GetParameters).Length)
                .First(),
                
            ConstructorParameterInjection = infos => infos
        };
    }
    
    public IocContainer CreateNestedContainer()
    {
        return new IocContainer(new List<IocContainer> { this }, InjectionOptions.Clone());
    }
    
    public bool HasService(Type t)
    {
        return HasService(new IocKey() { KeyType = t });
    }

    public bool HasService(string key, Type t)
    {
        return HasService(new IocKey() { KeyType = t });
    }

    public bool HasService(IocKey key)
    {
        return Registry.Registrations.ContainsKey(key);
    }
    
    public object Resolve(string key, Type t)
    {
        if (!Registry.Registrations.ContainsKey(new IocKey { KeyType = t, KeyName = key}))
        {
            throw new RegistrationNotFoundException(t);
        }
            
        return Resolve(t, key);
    }

    public T Activate<T>(InjectionOptions injectionOptions = null)
    {
        return (T)Activate(typeof(T), injectionOptions);
    }

    public object Activate(Type t, InjectionOptions injectionOptions = null)
    {
        var key = new IocKey { KeyType = t };

        IocServiceRegistration reg = new IocServiceRegistration()
        {
            ImplementationType = t,
            InjectionOptions = InjectionOptions,
            Key = key,
            IocLifecycle = IocLifecycles.Transient
        };

        TransientRegistration registration = new TransientRegistration()
        {
            IocServiceRegistration = reg
        };
        
        return registration.ResolveService(this, reg);
    }

    public T Resolve<T>()
    {
        return (T)Resolve(typeof(T));
    }

    public object Resolve(Type t)
    {
        return Resolve(t, null);
    }

    public object Resolve(Type t, string key)
    {
        var iocKey = new IocKey { KeyType = t, KeyName = key };
            
        return ResolveDependency(iocKey);
    }

    public object Resolve(IocKey key)
    {
        return ResolveDependency(key);
    }

    public object GetService(Type serviceType)
    {
        return Resolve(serviceType);
    }

    private object ResolveDependency(IocKey key)
    {
        if (this.HasService(key))
        {
            IocServiceRegistration regEntry = this.Registry[key];
            
            return (regEntry.FactoryFunc 
                    ?? ServiceFactoryLambdaGenerator
                        .DefaultInstance
                        .CreateTypeFactoryLambda(regEntry))
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
        return new IocContainer(InjectionOptions)
        {
            ParentContainers = ParentContainers,
            InjectionOptions = InjectionOptions.Clone(),
            Registry = Registry.Clone()
        };
    }
}