using RadFramework.Libraries.Ioc.Core;

namespace RadFramework.Libraries.Ioc.Registrations
{
    public class SingletonFactoryRegistration : TransientFactoryRegistration
    {
        private DataTypes.Lazy<object> singleton;

        public SingletonFactoryRegistration(Func<Core.IocContainer, object> factoryFunc, Core.IocContainer iocContainer) : base(factoryFunc, iocContainer)
        {
            singleton = new DataTypes.Lazy<object>(() => base.ResolveService());
        }

        public override object ResolveService(IocContainer container)
        {
            return singleton.Value;
        }
    }
}