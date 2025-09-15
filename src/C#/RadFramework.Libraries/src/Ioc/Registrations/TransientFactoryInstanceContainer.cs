using RadFramework.Libraries.Ioc.Builder;
using RadFramework.Libraries.Ioc.ConstructionLambdaFactory;

namespace RadFramework.Libraries.Ioc.Registrations
{
    public class TransientFactoryInstanceContainer : InstanceContainerBase
    {
        private Func<IocContainer, object> factoryFunc;
        
        public override void Initialize(IocDependency dependency)
        {
            factoryFunc = dependency.FactoryFunc
                          ?? ServiceFactoryLambdaGenerator.DefaultInstance.CreateTypeFactoryLambda(dependency);
        }

        public override object ResolveService(IocContainer container, IocDependency dependency)
        {
            return factoryFunc(container);
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