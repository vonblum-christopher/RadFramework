using System.Collections.Concurrent;
using System.Collections.Immutable;
using RadFramework.Libraries.Ioc.Factory;
using RadFramework.Libraries.Ioc.Registrations;
using RadFramework.Libraries.Reflection.Caching.Queries;

namespace RadFramework.Libraries.Ioc.Core;

public class IocContainer : ICloneable
{
    private List<IocContainer> fallbackResolvers = new();
    public InjectionOptions InjectionOptions;
    protected ConcurrentDictionary<IocKey, RegistrationBase> registrations = new ConcurrentDictionary<IocKey, RegistrationBase>();

    protected ServiceFactoryLambdaGenerator LambdaGenerator { get; } = new ServiceFactoryLambdaGenerator();

    public IEnumerable<IocService> ServiceList
    {
        get
        {
            return registrations.Select(r => 
                new IocService
                {
                    Key = r.Key,
                    RegistrationBase = r.Value
                });
        }
    }

    public IImmutableDictionary<IocKey, IocService> ServiceLookup
    {
        get
        {
            return registrations.Select(r =>
                    new IocService
                    {
                        Key = r.Key,
                        RegistrationBase = r.Value
                    })
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
    
    // not yet
    /*public IocContainer CreateNestedContainer()
    {
        return new IocContainer(new List<IocContainer> { this }, InjectionOptions.Clone());
    }*/
    
    public bool HasService(Type t)
    {
        return HasService(new IocKey() { RegistrationKeyType = t });
    }

    public bool HasService(string key, Type t)
    {
        return HasService(new IocKey() { RegistrationKeyType = t });
    }

    public bool HasService(IocKey key)
    {
        return registrations.ContainsKey(key);
    }
    
    public object Resolve(string key, Type t)
    {
        if (!registrations.ContainsKey(new IocKey { RegistrationKeyType = t, Key = key}))
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
        var key = new IocKey { RegistrationKeyType = t };
            
        return new TransientRegistration(key, t, LambdaGenerator, this)
        {
            InjectionOptions = injectionOptions ?? this.InjectionOptions
        }.ResolveService();
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
        var iocKey = new IocKey { RegistrationKeyType = t, Key = key };
            
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
        if (fallbackResolvers.Count == 0 || this.HasService(key))
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

        throw new RegistrationNotFoundException(key.RegistrationKeyType);
    }

    public object Clone()
    {
        return new IocContainer(InjectionOptions)
        {
            registrations = 
        };
    }
}