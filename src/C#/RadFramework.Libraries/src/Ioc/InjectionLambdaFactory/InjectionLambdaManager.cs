using System.Collections.Concurrent;
using RadFramework.Libraries.Abstractions;
using RadFramework.Libraries.Ioc.Builder;

namespace RadFramework.Libraries.Ioc.ConstructionLambdaFactory;

public class InjectionLambdaManager<TIocKey> where TIocKey : ICloneable<TIocKey>
{
    private static ConcurrentDictionary<TIocKey, Func<TypeOnlyIocContainer, object>> factoryCache = new();
    
    public static Func<TypeOnlyIocContainer, object> GetOrCreateConstructionFunc(TIocKey key, IocDependency<TIocKey> registration)
    {
        return factoryCache.GetOrAdd(
            key,
            tuple => ServiceFactoryLambdaGenerator.DefaultInstance.CreateTypeFactoryLambda(registration));
    }
}