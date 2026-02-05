using RadFramework.Libraries.Ioc.Builder;
using RadFramework.Libraries.Ioc.ConstructionLambdaFactory;
using RadFramework.Libraries.Reflection.Caching;

namespace RadFramework.Libraries.Ioc.Registrations
{
    public class SingletonInstanceContainer<TIocKey> : InstanceContainerBase<TIocKey> where TIocKey : ICloneable<TIocKey>
    {
        private Patterns.Lazy<object> singleton;

        private Func<TypeOnlyIocContainer, object> factoryFunc;

        private IocDependency<TIocKey> dependency;
        
        public override void Initialize(IocDependency<TIocKey> dependency)
        {
            dependency = this.dependency;
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
            return new SingletonInstanceContainer<TIocKey>()
                {
                    IocDependency = dependency.Clone()
                };
        }
    }
}