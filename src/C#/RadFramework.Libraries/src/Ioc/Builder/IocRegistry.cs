using System.Collections.Concurrent;
using RadFramework.Libraries.Abstractions;
using RadFramework.Libraries.Ioc.Registrations;

namespace RadFramework.Libraries.Ioc.Core;

public class IocRegistry : ICloneable<IocRegistry>
{
    public ConcurrentDictionary<IocKey, IocService> Registrations =
        new ConcurrentDictionary<IocKey, IocService>();

    public IocRegistry Clone()
    {
        return new IocRegistry()
        {
            Registrations = new ConcurrentDictionary<IocKey, IocService>(
                (IEnumerable<KeyValuePair<IocKey, IocService>>)
                Registrations
                    .ToDictionary(
                        k => k.Key.Clone(),
                        v => v.Value.Clone()))
        };
    }

    public IocService this[IocKey iocKey]
    {
        get => Registrations[iocKey];
        set => Registrations[iocKey] = value;
    }
}