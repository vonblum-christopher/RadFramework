using System.Collections.Immutable;
using RadFramework.Libraries.Ioc.Factory;

namespace RadFramework.Libraries.Ioc.Core
{
    public interface IIocContainer : IServiceProvider
    {
        IEnumerable<IocService> ServiceList { get; }
        IImmutableDictionary<IocKey, IocService> ServiceLookup { get; }

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