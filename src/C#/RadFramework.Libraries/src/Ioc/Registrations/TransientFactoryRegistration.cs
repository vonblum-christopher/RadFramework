using RadFramework.Libraries.Ioc.Builder;
using RadFramework.Libraries.Ioc.ConstructionMethodBuilders;

namespace RadFramework.Libraries.Ioc.Registrations
{
    public class TransientFactoryRegistration : RegistrationBase
    {
        private Func<IocContainer, object> factoryFunc;
        
        public override void Initialize(IocServiceRegistration serviceRegistration)
        {
            factoryFunc = serviceRegistration.FactoryFunc
                          ?? ServiceFactoryLambdaGenerator.DefaultInstance.CreateTypeFactoryLambda(serviceRegistration);
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