using RadFramework.Libraries.Ioc.Registrations;

namespace RadFramework.Libraries.Ioc;

public class IocService
{
    public IocKey Key { get; set; }
    public RegistrationBase RegistrationBase { get; set; }
    public Func<object> InstanceResolver { get; set; }
}