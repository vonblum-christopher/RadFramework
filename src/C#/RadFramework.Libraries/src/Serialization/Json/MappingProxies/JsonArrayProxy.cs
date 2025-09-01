using System.Collections;
using RadFramework.Libraries.Serialization.Json.Dom;

namespace RadFramework.Libraries.Serialization.Json.MappingProxies;

public class JsonArrayProxy<TEntry> : IEnumerable<TEntry>, IJsonArrayProxyInternal
{
    private IEnumerable<TEntry> DataAsEnumerableOfEntry => Data.OfType<TEntry>();

    public IEnumerator<TEntry> GetEnumerator()
    {
        return DataAsEnumerableOfEntry.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return DataAsEnumerableOfEntry.GetEnumerator();
    }

    public JsonArray Data { get; set; }
}