using RadFramework.Libraries.Abstractions;
using RadFramework.Libraries.Ioc.Builder;
using RadFramework.Libraries.Ioc.ConstructionLambdaFactory;

namespace RadFramework.Libraries.Ioc.Registrations
{
    public class SingletonFactoryInstanceContainer : InstanceContainerBase
    {
        private Patterns.Lazy<object> singleton;
        private Func<IocDependency, IIocContainer, object> factoryFunc;
        
        public override void Initialize(IocDependency dependency)
        {
            factoryFunc = dependency.FactoryFunc ?? ServiceFactoryLambdaGenerator.DefaultInstance.CreateTypeFactoryLambda(dependency);
        }

        public override object ResolveService(TypeOnlyIocContainer container, IocDependency dependency)
        {
            if (singleton == null)
            {
                singleton = new Patterns.Lazy<object>(() => factoryFunc(dependency, container));
            }
            
            return singleton.Value;
        }

        public override InstanceContainerBase Clone()
        {
            return new TransientInstanceContainer()
            {
                IocDependency = IocDependency.Clone(),
            };
        }
    }
}