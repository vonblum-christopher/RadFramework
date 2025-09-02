using System.Collections.Immutable;
using RadFramework.Libraries.Ioc;
using RadFramework.Libraries.Ioc.Builder;

namespace RadFramework.Libraries.Abstractions
{
    public interface IIocContainer
    {
        ImmutableList<IocDependency> ServiceList { get; }
        IImmutableDictionary<IocKey, IocDependency> ServiceLookup { get; }

        bool HasService(Type t);
        bool HasService(string key, Type t);
        bool HasService(IocKey key);
        
        object Resolve(Type tInterface);
        object Resolve(string key, Type t);
        object Resolve(IocKey key);

        public T Activate<T>();
        public object Activate(Type t);
    }
}