using System.Collections.Immutable;
using System.Reflection;
using RadFramework.Libraries.Reflection.Caching;

namespace RadFramework.Libraries;

public static class ReflectionExtensions
{
    public static IDictionary<CachedPropertyInfo, Func<object>> ToLiveDictionary(this object o)
    {
        return ((CachedType)o.GetType())
            .Query(t => t.GetProperties()
                .Select(p => (CachedPropertyInfo)p))
            .Select(p => new KeyValuePair<CachedPropertyInfo, Func<object>>(
                p, 
                () =>
                {
                    object value = p.InnerMetaData.GetValue(o);

                    if (value == null)
                    {
                        return null;
                    }
                    
                    return p.InnerMetaData.PropertyType.IsPrimitive || p.InnerMetaData.PropertyType == typeof(string)
                        ? () => value
                        : () => ToLiveDictionary(value);
                }))
            .ToDictionary();
    }

    /*object GetProperty(object o, string property)
    {
        
    }
    
    object dictionary provider
    {
        interceptor on object returns to indexer
    }
    to js object*/
}