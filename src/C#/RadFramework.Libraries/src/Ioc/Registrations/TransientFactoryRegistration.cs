using RadFramework.Libraries.Ioc.Core;
using RadFramework.Libraries.Ioc.Factory;

namespace RadFramework.Libraries.Ioc.Registrations
{
    public class TransientFactoryRegistration : RegistrationBase
    {
        private Func<Core.IocContainer, object> factoryFunc;
        
        public override void Initialize(IocServiceRegistration serviceRegistration)
        {
            factoryFunc = serviceRegistration.FactoryFunc ?? ServiceFactoryLambdaGenerator.DefaultInstance.CreateInstanceFactoryMethod(serviceRegistration);
        }

        public override object ResolveService(IocContainer container, IocServiceRegistration serviceRegistration)
        {
            return factoryFunc(container);
        }

        public override RegistrationBase Clone()
        {
            return new TransientRegistration()
            {
                IocServiceRegistration = IocServiceRegistration.Clone(),
            };
        }
    }
}