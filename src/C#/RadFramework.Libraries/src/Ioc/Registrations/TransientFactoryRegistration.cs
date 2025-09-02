using RadFramework.Libraries.Ioc.Core;

namespace RadFramework.Libraries.Ioc.Registrations
{
    public class TransientFactoryRegistration : RegistrationBase
    {
        private readonly Func<Core.IocContainer, object> factoryFunc;

        public TransientFactoryRegistration(Func<Core.IocContainer, object> factoryFunc)
        {
            this.factoryFunc = factoryFunc;
        }
        
        public override object ResolveService(IocContainer container)
        {
            return factoryFunc(container);
        }

        public override object Clone()
        {
            return new TransientFactoryRegistration(factoryFunc);
        }
    }
}