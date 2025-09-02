using RadFramework.Libraries.Ioc.Builder;
using RadFramework.Libraries.Ioc.ConstructionMethodBuilders;

namespace RadFramework.Libraries.Ioc.Registrations
{
    public class SingletonFactoryRegistration : RegistrationBase
    {
        private DataTypes.Lazy<object> singleton;
        private Func<IocContainer, object> factoryFunc;
        
        public override void Initialize(IocServiceRegistration serviceRegistration)
        {
            factoryFunc = serviceRegistration.FactoryFunc ?? ServiceFactoryLambdaGenerator.DefaultInstance.CreateTypeFactoryLambda(serviceRegistration);
        }

        public override object ResolveService(IocContainer container, IocServiceRegistration serviceRegistration)
        {
            if (singleton == null)
            {
                singleton = new DataTypes.Lazy<object>(() => factoryFunc(container));
            }
            
            return singleton.Value;
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