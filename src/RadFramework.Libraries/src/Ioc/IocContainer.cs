using System.Collections.Concurrent;
using System.Collections.Immutable;
using RadFramework.Libraries.Ioc.Factory;
using RadFramework.Libraries.Ioc.Registrations;
using RadFramework.Libraries.Reflection.Caching.Queries;

namespace RadFramework.Libraries.Ioc
{
    public partial class IocContainer : IIocContainer
    {
        public IEnumerable<IocService> ServiceList
        {
            get
            {
                return Enumerable.Select<KeyValuePair<IocKey, RegistrationBase>, IocService>(registrations, r => 
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
                return Enumerable.Select<KeyValuePair<IocKey, RegistrationBase>, IocService>(registrations, r =>
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
    }
}