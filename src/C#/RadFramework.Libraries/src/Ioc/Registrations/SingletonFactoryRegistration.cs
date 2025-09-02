using RadFramework.Libraries.Ioc.Core;

namespace RadFramework.Libraries.Ioc.Registrations
{
    public class SingletonFactoryRegistration : TransientFactoryRegistration
    {
        private DataTypes.Lazy<object> singleton;

        public SingletonFactoryRegistration(Func<Core.IocContainer, object> factoryFunc, Core.IocContainer iocContainer) : base(factoryFunc, iocContainer)
        {
        }

        public override object ResolveService(IocContainer container)
        {
            if (singleton == null)
            {
                singleton = new DataTypes.Lazy<object>(() => base.ResolveService(container));
            }
            
            return singleton.Value;
        }
    }
}