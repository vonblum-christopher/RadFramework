using System.Collections.Concurrent;
using RadFramework.Libraries.Abstractions;
using RadFramework.Libraries.Ioc.Base;
using RadFramework.Libraries.Ioc.Registrations;

namespace RadFramework.Libraries.Ioc.Builder;

public class IocBuilderRegistry : ICloneable<IocBuilderRegistry>
{
    public ConcurrentDictionary<IIocKey, IocDependency> Registrations = new();

    public IocBuilderRegistry Clone()
    {
        return new IocBuilderRegistry()
        {
            Registrations = new ConcurrentDictionary<IIocKey, IocDependency>
            (Registrations
                .ToDictionary(
                    k => k.Key.Clone(),
                    v => v.Value.Clone()))
        };
    }

    public IocDependency this[IIocKey namedIocKey]
    {
        get => Registrations[namedIocKey];
        set => Registrations[namedIocKey] = value;
    }
}