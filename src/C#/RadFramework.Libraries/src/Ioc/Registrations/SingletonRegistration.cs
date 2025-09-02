using RadFramework.Libraries.Ioc.Builder;
using RadFramework.Libraries.Ioc.ConstructionMethodBuilders;
using RadFramework.Libraries.Reflection.Caching;

namespace RadFramework.Libraries.Ioc.Registrations
{
    public class SingletonRegistration : RegistrationBase
    {
        private DataTypes.Lazy<object> singleton;

        private Func<IocContainer, object> factoryFunc;

        private IocServiceRegistration serviceRegistration;
        
        public override void Initialize(IocServiceRegistration serviceRegistration)
        {
            serviceRegistration = this.serviceRegistration;
            factoryFunc = serviceRegistration.FactoryFunc ?? ServiceFactoryLambdaGenerator.DefaultInstance.CreateInstanceFactoryMethod(serviceRegistration);
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
            return new SingletonRegistration()
                {
                    IocServiceRegistration = serviceRegistration.Clone()
                };
        }
    }
}