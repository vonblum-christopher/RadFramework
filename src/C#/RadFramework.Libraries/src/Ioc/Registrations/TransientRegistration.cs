using System.Collections.Concurrent;
using RadFramework.Libraries.Ioc.Builder;
using RadFramework.Libraries.Ioc.ConstructionLambdaFactory;
using RadFramework.Libraries.Reflection.Caching;

namespace RadFramework.Libraries.Ioc.Registrations
{
    public class TransientRegistration : RegistrationBase
    {
        private DataTypes.Lazy<Func<IocContainer, object>> construct;
        
        public override void Initialize(IocDependency dependency)
        {
            this.construct = new DataTypes.Lazy<Func<IocContainer, object>>(
                () => 
                    dependency.FactoryFunc 
                    ?? ServiceFactoryLambdaGenerator.DefaultInstance.CreateTypeFactoryLambda(dependency));
        }

        public override object ResolveService(IocContainer container, IocDependency dependency)
        {
            return construct.Value(container);
        }

        public override RegistrationBase Clone()
        {
            return new TransientRegistration()
            {
                IocDependency = IocDependency.Clone()
            };
        }
    }
}