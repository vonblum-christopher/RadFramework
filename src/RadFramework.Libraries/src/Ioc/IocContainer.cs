using System.Collections.Concurrent;
using System.Collections.Immutable;
using RadFramework.Libraries.Ioc.Factory;
using RadFramework.Libraries.Ioc.Registrations;
using RadFramework.Libraries.Reflection.Caching.Queries;

namespace RadFramework.Libraries.Ioc
{
    public class IocContainer : IIocContainer
    {

        public IEnumerable<IocService> ServiceList
        {
            get
            {
                return registrations.Select(r => 
                    new IocService
                    {
                        Key = r.Key, 
                        InstanceResolver = r.Value.ResolveService,
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
                        InstanceResolver = r.Value.ResolveService,
                        RegistrationBase = r.Value
                    })
                    .ToImmutableDictionary(
                        k => k.Key, 
                        v => v);
            }
            
        }
        
        public readonly InjectionOptions InjectionOptions;
        protected ServiceFactoryLambdaGenerator LambdaGenerator { get; } = new ServiceFactoryLambdaGenerator();
        
        private ConcurrentDictionary<IocKey, RegistrationBase> registrations = new ConcurrentDictionary<IocKey, RegistrationBase>();

        public IocContainer(InjectionOptions injectionOptions)
        {
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
        
        public InjectionOptions RegisterTransient(Type tInterface, Type tImplementation)
        {
            var key = new IocKey { RegistrationKeyType = tInterface };
            
            return (registrations[key] = new TransientRegistration(key, tImplementation, LambdaGenerator, this)
            {
                InjectionOptions = InjectionOptions.Clone()
            }).InjectionOptions;
        }

        public InjectionOptions RegisterTransient<TInterface, TImplementation>()
        {
            var key = new IocKey { RegistrationKeyType = typeof(TInterface) };
            
            return (registrations[key] = new TransientRegistration(key, typeof(TImplementation), LambdaGenerator, this)
            {
                InjectionOptions = InjectionOptions.Clone()
            }).InjectionOptions;
        }

        public InjectionOptions RegisterTransient(Type tImplementation)
        {
            var key = new IocKey { RegistrationKeyType = tImplementation };
            return (registrations[key] = new TransientRegistration(key, tImplementation, LambdaGenerator, this)
            {
                InjectionOptions = InjectionOptions.Clone()
            }).InjectionOptions;
        }
        
        public InjectionOptions RegisterTransient<TImplementation>()
        {
            var key = new IocKey { RegistrationKeyType = typeof(TImplementation) };
            return (registrations[new IocKey { RegistrationKeyType = key.RegistrationKeyType }] = new TransientRegistration(key, key.RegistrationKeyType, LambdaGenerator, this)
            {
                InjectionOptions = InjectionOptions.Clone()
            }).InjectionOptions;
        }

        public void RegisterSemiAutomaticTransient(Type tImplementation, Func<IocContainer, object> construct)
        {
            registrations[new IocKey { RegistrationKeyType = tImplementation }] = new TransientFactoryRegistration(construct, this);
        }
        
        public void RegisterSemiAutomaticTransient<TImplementation>(Func<IocContainer, object> construct)
        {
            registrations[new IocKey { RegistrationKeyType = typeof(TImplementation) }] = new TransientFactoryRegistration(construct, this);
        }

        
        public InjectionOptions RegisterSingleton(Type tInterface, Type tImplementation)
        {
            var key = new IocKey { RegistrationKeyType = tInterface };
            
            return (registrations[key] = new SingletonRegistration(key,tImplementation, LambdaGenerator, this)
            {
                InjectionOptions = InjectionOptions.Clone()
            }).InjectionOptions;
        }

        public InjectionOptions RegisterSingleton<TInterface, TImplementation>()
        {
            var key = new IocKey { RegistrationKeyType = typeof(TInterface) };
            
            return (registrations[key] = new SingletonRegistration(key, typeof(TImplementation), LambdaGenerator, this)
            {
                InjectionOptions = InjectionOptions.Clone()
            }).InjectionOptions;
        }

        public InjectionOptions RegisterSingleton(Type tImplementation)
        {
            var key = new IocKey { RegistrationKeyType = tImplementation };
            
            return (registrations[key] = new SingletonRegistration(key, tImplementation, LambdaGenerator, this)
            {
                InjectionOptions = InjectionOptions.Clone()
            }).InjectionOptions;
        }
        
        public InjectionOptions RegisterSingleton<TImplementation>()
        {
            Type tImplementation = typeof(TImplementation);
            var key = new IocKey { RegistrationKeyType = tImplementation};
            return (registrations[key] = new SingletonRegistration(key, tImplementation, LambdaGenerator, this)
            {
                InjectionOptions = InjectionOptions.Clone()
            }).InjectionOptions;
        }

        public void RegisterSemiAutomaticSingleton(Type tImplementation, Func<IocContainer, object> construct)
        {
            registrations[new IocKey(){ RegistrationKeyType = tImplementation}] = new SingletonFactoryRegistration(construct, this);
        }
        
        public void RegisterSemiAutomaticSingleton<TImplementation>(Func<IocContainer, object> construct)
        {
            registrations[new IocKey(){ RegistrationKeyType = typeof(TImplementation)}] = new SingletonFactoryRegistration(construct, this);
        }

        public void RegisterSingletonInstance(Type tInterface, object instance)
        {
            registrations[new IocKey(){ RegistrationKeyType = tInterface}] = new SingletonInstanceRegistration(instance);
        }
        
        public void RegisterSingletonInstance<TInterface>(object instance)
        {
            registrations[new IocKey(){ RegistrationKeyType = typeof(TInterface)}] = new SingletonInstanceRegistration(instance);
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
            if (!registrations.ContainsKey(new IocKey { RegistrationKeyType = t}))
            {
                throw new RegistrationNotFoundException(t);
            }
            
            return Resolve(t, null);
        }
        
        public object Resolve(Type t, string key)
        {
            var iocKey = new IocKey { RegistrationKeyType = t, Key = key };
            if (!registrations.ContainsKey(iocKey))
            {
                throw new RegistrationNotFoundException(t);
            }
            
            return Resolve(iocKey);
        }
        
        public object Resolve(IocKey key)
        {
            return registrations[key].ResolveService();
        }

        public object GetService(Type serviceType)
        {
            return Resolve(serviceType);
        }
    }
}