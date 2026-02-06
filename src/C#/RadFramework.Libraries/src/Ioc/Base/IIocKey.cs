using System.Collections.Immutable;
using RadFramework.Libraries.Abstractions;
using RadFramework.Libraries.Reflection.Caching;

namespace RadFramework.Libraries.Ioc.Base;

public interface IIocKey : ICloneable<IIocKey>
{
    SortedDictionary<string, object> KeyProperties { get; }
}