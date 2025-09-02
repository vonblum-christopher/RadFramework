using System.Collections.Concurrent;
using RadFramework.Libraries.Abstractions;
using RadFramework.Libraries.Ioc.Registrations;

namespace RadFramework.Libraries.Ioc.Core;

public class IocRegistry : ICloneable<IocRegistry>
{
    public ConcurrentDictionary<IocKey, IocServiceRegistration> Registrations =
        new ConcurrentDictionary<IocKey, IocServiceRegistration>();

    public IocRegistry Clone()
    {
        return new IocRegistry()
        {
            Registrations = new ConcurrentDictionary<IocKey, IocServiceRegistration>(
                (IEnumerable<KeyValuePair<IocKey, IocServiceRegistration>>)
                Registrations
                    .ToDictionary(
                        k => k.Key.Clone(),
                        v => v.Value.Clone()))
        };
    }

    public IocServiceRegistration this[IocKey iocKey]
    {
        get => Registrations[iocKey];
        set => Registrations[iocKey] = value;
    }
}