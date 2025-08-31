using System.Collections.Immutable;
using RadFramework.Libraries.Ioc.Factory;

namespace RadFramework.Libraries.Ioc
{
    public interface IIocContainer : IServiceProvider
    {
        IEnumerable<IocService> ServiceList { get; }
        IImmutableDictionary<IocKey, IocService> ServiceLookup { get; }

        object Resolve(Type t);
        object Resolve(string key, Type t);
        object Resolve(IocKey key);

        public T Activate<T>(InjectionOptions injectionOptions = null);

        public object Activate(Type t, InjectionOptions injectionOptions = null);
    }
}