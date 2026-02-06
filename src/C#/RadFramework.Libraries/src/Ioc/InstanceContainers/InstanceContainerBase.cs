using RadFramework.Libraries.Abstractions;
using RadFramework.Libraries.Ioc.Builder;
using RadFramework.Libraries.Reflection.Caching;

namespace RadFramework.Libraries.Ioc.Registrations
{
    public abstract class InstanceContainerBase : ICloneable<InstanceContainerBase>
    {
        public IocDependency IocDependency { get; set; }

        public virtual void Initialize(IocDependency dependency)
        {
            IocDependency = dependency;
        }
        
        public abstract object ResolveService(TypeOnlyIocContainer container, IocDependency dependency);
        
        public abstract InstanceContainerBase Clone();
    }
}