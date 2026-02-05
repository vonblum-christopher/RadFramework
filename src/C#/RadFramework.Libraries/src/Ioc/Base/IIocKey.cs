using System.Collections.Immutable;
using RadFramework.Libraries.Reflection.Caching;

namespace RadFramework.Libraries.Ioc.Base;

public interface IIocKey
{
    IImmutableDictionary<string, object> KeyProperties { get; set; }
}