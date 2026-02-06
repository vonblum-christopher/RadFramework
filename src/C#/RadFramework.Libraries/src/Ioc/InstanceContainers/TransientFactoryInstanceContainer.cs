using RadFramework.Libraries.Abstractions;
using RadFramework.Libraries.Ioc.Builder;
using RadFramework.Libraries.Ioc.ConstructionLambdaFactory;

namespace RadFramework.Libraries.Ioc.Registrations
{
    public class TransientFactoryInstanceContainer : InstanceContainerBase
    {
        private Func<IocDependency, IIocContainer, object> factoryFunc;
        
        public override void Initialize(IocDependency dependency)
        {
            factoryFunc = dependency.FactoryFunc
                          ?? ServiceFactoryLambdaGenerator.DefaultInstance.CreateTypeFactoryLambda(dependency);
        }

        public override object ResolveService(TypeOnlyIocContainer container, IocDependency dependency)
        {
            return factoryFunc(dependency, container);
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