using RadFramework.Libraries.Ioc.Registrations;

namespace RadFramework.Libraries.Ioc.Core;

public class IocService
{
    public IocKey Key { get; set; }
    public RegistrationBase RegistrationBase { get; set; }
}