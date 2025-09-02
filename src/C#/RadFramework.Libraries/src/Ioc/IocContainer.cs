using System.Collections.Concurrent;
using System.Collections.Immutable;
using RadFramework.Libraries.Abstractions;
using RadFramework.Libraries.Ioc.Factory;
using RadFramework.Libraries.Ioc.Registrations;
using RadFramework.Libraries.Reflection.Caching.Queries;

namespace RadFramework.Libraries.Ioc.Core;

public class IocContainer : IIocContainer, ICloneable<IocContainer>
{
    private List<IocContainer> fallbackResolvers = new();
    public InjectionOptions InjectionOptions;
    private IocRegistry iocRegistry = new IocRegistry();

    protected ServiceFactoryLambdaGenerator LambdaGenerator { get; } = new ServiceFactoryLambdaGenerator();

    public IEnumerable<IocServiceRegistration> ServiceList
    {
        get
        {
            return iocRegistry.Registrations.Values;
        }
    }

    public IImmutableDictionary<IocKey, IocServiceRegistration> ServiceLookup
    {
        get
        {
            return iocRegistry.Registrations.Values
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
        this.fallbackResolvers = fallbackResolvers.ToList();
        this.InjectionOptions = injectionOptions;
    }

    public IocContainer(IocContainerBuilder builder) : this()
    {
        iocRegistry = builder.Clone().IocRegistry;
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
        return iocRegistry.Registrations.ContainsKey(key);
    }
    
    public object Resolve(string key, Type t)
    {
        if (!iocRegistry.Registrations.ContainsKey(new IocKey { KeyType = t, Key = key}))
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
        var iocKey = new IocKey { KeyType = t, Key = key };
            
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
        if (fallbackResolvers.Count == 0 && this.HasService(key))
        {
            return this.Resolve(key);
        }

        foreach (IocContainer fallbackResolver in fallbackResolvers)
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
            fallbackResolvers = fallbackResolvers,
            InjectionOptions = InjectionOptions.Clone(),
            iocRegistry = iocRegistry.Clone()
        };
    }
}