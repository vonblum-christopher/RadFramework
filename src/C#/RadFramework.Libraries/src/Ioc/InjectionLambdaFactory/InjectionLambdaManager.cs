using System.Collections.Concurrent;
using RadFramework.Libraries.Ioc.Builder;

namespace RadFramework.Libraries.Ioc.ConstructionLambdaFactory;

public class InjectionLambdaManager
{
    private static ConcurrentDictionary<IocKey, Func<IocContainer, object>> factoryCache = new();
    
    public static Func<IocContainer, object> GetOrCreateConstructionFunc(IocKey key, IocDependency registration)
    {
        return factoryCache.GetOrAdd(
            key,
            tuple => ServiceFactoryLambdaGenerator.DefaultInstance.CreateTypeFactoryLambda(registration));
    }
}