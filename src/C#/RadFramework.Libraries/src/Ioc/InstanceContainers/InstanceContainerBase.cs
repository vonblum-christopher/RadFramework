using RadFramework.Libraries.Abstractions;
using RadFramework.Libraries.Ioc.Builder;
using RadFramework.Libraries.Reflection.Caching;

namespace RadFramework.Libraries.Ioc.Registrations
{
    public abstract class InstanceContainerBase<TIocKey> : ICloneable<InstanceContainerBase<TIocKey>> where TIocKey : ICloneable<TIocKey>
    {
        public IocDependency<TIocKey> IocDependency { get; set; }

        public virtual void Initialize(IocDependency<TIocKey> dependency)
        {
            IocDependency = dependency;
        }
        
        public abstract object ResolveService(TypeOnlyIocContainer container, IocDependency<TIocKey> dependency);
        
        public abstract InstanceContainerBase<TIocKey> Clone();
    }
}