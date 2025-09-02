using System.Collections.Concurrent;
using RadFramework.Libraries.Abstractions;

namespace RadFramework.Libraries.Ioc.Builder;

public class IocRegistry : ICloneable<IocRegistry>
{
    public ConcurrentDictionary<IocKey, IocDependency> Registrations = new();

    public IocRegistry Clone()
    {
        return new IocRegistry
        {
            Registrations = new ConcurrentDictionary<IocKey, IocDependency>(
                (IEnumerable<KeyValuePair<IocKey, IocDependency>>)
                Registrations
                    .ToDictionary(
                        k => k.Key.Clone(),
                        v => v.Value.Clone()))
        };
    }

    public IocDependency this[IocKey iocKey]
    {
        get => Registrations[iocKey];
        set => Registrations[iocKey] = value;
    }
}