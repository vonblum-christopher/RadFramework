using RadFramework.Libraries.Ioc.Core;
using RadFramework.Libraries.Ioc.Factory;
using RadFramework.Libraries.Reflection.Caching;

namespace RadFramework.Libraries.Ioc.Registrations
{
    public class SingletonRegistration : TransientRegistration
    {
        private DataTypes.Lazy<object> singleton;
        public SingletonRegistration(IocKey key,
            CachedType tImplementation,
            ServiceFactoryLambdaGenerator lambdaGenerator,
            Core.IocContainer iocContainer) : 
                base(key,
                    tImplementation,
                    lambdaGenerator,
                    iocContainer)
        {
            singleton = new DataTypes.Lazy<object>(() => base.ResolveService());
        }
        
        public override object ResolveService()
        {
            return singleton.Value;
        }
    }
}