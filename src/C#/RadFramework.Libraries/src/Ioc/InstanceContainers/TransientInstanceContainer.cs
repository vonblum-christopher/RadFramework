using System.Collections.Concurrent;
using RadFramework.Libraries.Ioc.Builder;
using RadFramework.Libraries.Ioc.ConstructionLambdaFactory;
using RadFramework.Libraries.Reflection.Caching;

namespace RadFramework.Libraries.Ioc.Registrations
{
    public class TransientInstanceContainer : InstanceContainerBase
    {
        private Patterns.Lazy<Func<IocContainer, object>> construct;
        
        public override void Initialize(IocDependency dependency)
        {
            this.construct = new Patterns.Lazy<Func<IocContainer, object>>(
                () => 
                    dependency.FactoryFunc 
                    ?? ServiceFactoryLambdaGenerator.DefaultInstance.CreateTypeFactoryLambda(dependency));
        }

        public override object ResolveService(IocContainer container, IocDependency dependency)
        {
            return construct.Value(container);
        }

        public override InstanceContainerBase Clone()
        {
            return new TransientInstanceContainer()
            {
                IocDependency = IocDependency.Clone()
            };
        }
    }
}