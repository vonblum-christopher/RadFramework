using RadFramework.Libraries.Ioc.Builder;
using RadFramework.Libraries.Ioc.ConstructionLambdaFactory;
using RadFramework.Libraries.Reflection.Caching;

namespace RadFramework.Libraries.Ioc.Registrations
{
    public class SingletonInstanceContainer : InstanceContainerBase
    {
        private Patterns.Lazy<object> singleton;

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
                singleton = new Patterns.Lazy<object>(() => factoryFunc(container));
            }
            
            return singleton.Value;
        }

        public override InstanceContainerBase Clone()
        {
            return new SingletonInstanceContainer()
                {
                    IocDependency = dependency.Clone()
                };
        }
    }
}