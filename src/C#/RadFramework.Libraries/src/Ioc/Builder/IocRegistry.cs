using System.Collections.Concurrent;
using RadFramework.Libraries.Abstractions;
using RadFramework.Libraries.Ioc.Registrations;

namespace RadFramework.Libraries.Ioc.Builder;

public class IocRegistry : ICloneable<IocRegistry>
{
    public ConcurrentDictionary<IocKey, (IocDependency dependency, InstanceContainerBase containerBase)> Registrations = new();

    public IocRegistry Clone()
    {
        return new IocRegistry
        {
            Registrations = new ConcurrentDictionary<IocKey, (IocDependency dependency, InstanceContainerBase containerBase)>(
                (IEnumerable<KeyValuePair<IocKey, (IocDependency dependency, InstanceContainerBase containerBase)>>)
                Registrations
                    .ToDictionary(
                        k => k.Key.Clone(),
                        v => (v.Value.dependency.Clone(), v.Value.containerBase))
        };
    }

    public IocDependency this[IocKey iocKey]
    {
        get => Registrations[iocKey].dependency;
        set => Registrations[iocKey] = value;
    }
}