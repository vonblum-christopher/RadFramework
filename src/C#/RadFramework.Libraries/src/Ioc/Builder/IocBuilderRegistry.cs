using System.Collections.Concurrent;
using RadFramework.Libraries.Abstractions;
using RadFramework.Libraries.Ioc.Registrations;

namespace RadFramework.Libraries.Ioc.Builder;

public class IocBuilderRegistry<TIocKey> : ICloneable<IocBuilderRegistry<TIocKey>> where TIocKey : ICloneable<TIocKey>
{
    public ConcurrentDictionary<TIocKey, IocDependency<TIocKey>> Registrations = new();

    public IocBuilderRegistry<TIocKey> Clone()
    {
        return new IocBuilderRegistry<TIocKey>()
        {
            Registrations = new ConcurrentDictionary<TIocKey, IocDependency<TIocKey>>
            (Registrations
                .ToDictionary(
                    k => k.Key.Clone(),
                    v => v.Value.Clone()))
        };
    }

    public IocDependency<TIocKey> this[TIocKey namedIocKey]
    {
        get => Registrations[namedIocKey];
        set => Registrations[namedIocKey] = value;
    }
}