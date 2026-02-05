using System.Collections.Concurrent;
using RadFramework.Libraries.Ioc.Builder;
using RadFramework.Libraries.Ioc.ConstructionLambdaFactory;
using RadFramework.Libraries.Reflection.Caching;

namespace RadFramework.Libraries.Ioc.Registrations
{
    public class TransientInstanceContainer<TIocKey> : InstanceContainerBase<TIocKey> where TIocKey : ICloneable<TIocKey>
    {
        private Patterns.Lazy<Func<TypeOnlyIocContainer, object>> construct;
        
        public override void Initialize(IocDependency<TIocKey> dependency)
        {
            this.construct = new Patterns.Lazy<Func<TypeOnlyIocContainer, object>>(
                () => 
                    dependency.FactoryFunc 
                    ?? ServiceFactoryLambdaGenerator.DefaultInstance.CreateTypeFactoryLambda(dependency));
        }

        public override object ResolveService(TypeOnlyIocContainer container, IocDependency<TIocKey> dependency)
        {
            return construct.Value(container);
        }

        public override InstanceContainerBase<TIocKey> Clone()
        {
            return new TransientInstanceContainer<TIocKey>()
            {
                IocDependency = IocDependency.Clone()
            };
        }
    }
}