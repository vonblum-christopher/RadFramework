using RadFramework.Libraries.Ioc.Builder;
using RadFramework.Libraries.Ioc.ConstructionLambdaFactory;
using RadFramework.Libraries.Reflection.Caching;

namespace RadFramework.Libraries.Ioc.Registrations
{
    public class SingletonRegistration : RegistrationBase
    {
        private DataTypes.Lazy<object> singleton;

        private Func<IocContainer, object> factoryFunc;

        private IocDependency dependency;
        
        public override void Initialize(IocDependency dependency)
        {
            dependency = this.dependency;
            factoryFunc = dependency.FactoryFunc ?? ServiceFactoryLambdaGenerator.DefaultInstance.CreateTypeFactoryLambda(dependency);
        }

        public override object ResolveService(IocContainer container, IocDependency dependency)
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
                    IocDependency = dependency.Clone()
                };
        }
    }
}