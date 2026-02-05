using RadFramework.Libraries.Abstractions;
using RadFramework.Libraries.Ioc.Builder;
using RadFramework.Libraries.Ioc.ConstructionLambdaFactory;

namespace RadFramework.Libraries.Ioc.Registrations
{
    public class SingletonFactoryInstanceContainer<TIocKey> : InstanceContainerBase<TIocKey> where TIocKey : ICloneable<TIocKey>
    {
        private Patterns.Lazy<object> singleton;
        private Func<TypeOnlyIocContainer, object> factoryFunc;
        
        public override void Initialize(IocDependency<TIocKey> dependency)
        {
            factoryFunc = dependency.FactoryFunc ?? ServiceFactoryLambdaGenerator.DefaultInstance.CreateTypeFactoryLambda(dependency);
        }

        public override object ResolveService(TypeOnlyIocContainer container, IocDependency<TIocKey> dependency)
        {
            if (singleton == null)
            {
                singleton = new Patterns.Lazy<object>(() => factoryFunc(container));
            }
            
            return singleton.Value;
        }

        public override InstanceContainerBase<TIocKey> Clone()
        {
            return new TransientInstanceContainer<TIocKey>()
            {
                IocDependency = IocDependency.Clone(),
            };
        }
    }
}