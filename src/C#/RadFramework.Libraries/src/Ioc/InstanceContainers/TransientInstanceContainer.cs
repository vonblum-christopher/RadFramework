using System.Collections.Concurrent;
using RadFramework.Libraries.Abstractions;
using RadFramework.Libraries.Ioc.Builder;
using RadFramework.Libraries.Ioc.ConstructionLambdaFactory;
using RadFramework.Libraries.Reflection.Caching;

namespace RadFramework.Libraries.Ioc.Registrations
{
    public class TransientInstanceContainer : InstanceContainerBase
    {
        private Patterns.Lazy<Func<IocDependency, IIocContainer, object>> construct;
        
        public override void Initialize(IocDependency dependency)
        {
            this.construct = new Patterns.Lazy<Func<IocDependency, IIocContainer, object>>(
                () => 
                    dependency.FactoryFunc 
                    ?? ServiceFactoryLambdaGenerator.DefaultInstance.CreateTypeFactoryLambda(dependency));
        }

        public override object ResolveService(TypeOnlyIocContainer container, IocDependency dependency)
        {
            return construct.Value(dependency, container);
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