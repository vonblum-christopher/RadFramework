namespace RadFramework.Libraries.Ioc.Registrations
{
    public class TransientFactoryRegistration : RegistrationBase
    {
        private readonly Func<Core.IocContainer, object> factoryFunc;
        private readonly Core.IocContainer iocContainer;

        public TransientFactoryRegistration(Func<Core.IocContainer, object> factoryFunc, Core.IocContainer iocContainer)
        {
            this.factoryFunc = factoryFunc;
            this.iocContainer = iocContainer;
        }
        
        public override object ResolveService()
        {
            return factoryFunc(iocContainer);
        }
    }
}