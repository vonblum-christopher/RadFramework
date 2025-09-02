using System.Collections.Concurrent;
using RadFramework.Libraries.Ioc.Builder;

namespace RadFramework.Libraries.Ioc.ConstructionMethodBuilders;

public class ResolveFuncManager
{
    private ConcurrentDictionary<IocKey, Func<IocContainer, object>> factoryCache = new();
    
    public Func<IocContainer, object> GetOrCreateFunc(IocKey key, IocServiceRegistration registration)
    {
        return factoryCache.GetOrAdd(
            key,
            tuple => ServiceFactoryLambdaGenerator.DefaultInstance.CreateTypeFactoryLambda(registration));
    }
}