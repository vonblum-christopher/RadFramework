using RadFramework.Libraries.Ioc.Core;
using RadFramework.Libraries.Ioc.Factory;
using RadFramework.Libraries.Reflection.Caching;

namespace RadFramework.Libraries.Ioc.Registrations
{
    public class SingletonRegistration
    {
        private DataTypes.Lazy<object> singleton;
        public SingletonRegistration(IocKey key,
            CachedType tImplementation,
            ServiceFactoryLambdaGenerator lambdaGenerator) : 
                base(key,
                    tImplementation,
                    lambdaGenerator)
        {
            singleton = new DataTypes.Lazy<object>(() => base.ResolveService());
        }
        
        public override object ResolveService(IocContainer container)
        {
            return singleton.Value;
        }
    }
}