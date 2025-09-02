using System.Collections.Immutable;
using RadFramework.Libraries.Abstractions;
using RadFramework.Libraries.Ioc.Builder;
using RadFramework.Libraries.Ioc.ConstructionMethodBuilders;

namespace RadFramework.Libraries.Ioc
{
    public interface IIocContainer
    {
        IEnumerable<IocServiceRegistration> ServiceList { get; }
        IImmutableDictionary<IocKey, IocServiceRegistration> ServiceLookup { get; }

        bool HasService(Type t);
        bool HasService(string key, Type t);
        bool HasService(IocKey key);
        
        object Resolve(Type t);
        object Resolve(string key, Type t);
        object Resolve(IocKey key);

        public T Activate<T>(InjectionOptions injectionOptions = null);
        public object Activate(Type t, InjectionOptions injectionOptions = null);
    }
}