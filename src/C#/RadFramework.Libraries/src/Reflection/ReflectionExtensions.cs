using System.Collections.Immutable;
using System.Reflection;
using RadFramework.Libraries.Reflection.Caching;

namespace RadFramework.Libraries;

public static class ReflectionExtensions
{
    private class ObjectDictionary : Dictionary<string, object>
    {
    }
    
    public static IDictionary<CachedPropertyInfo, Func<object>> SerializeToDictionary(this object o)
    {
        return ((CachedType)o.GetType())
            .Query(t => t.GetProperties()
                .Select(p => (CachedPropertyInfo)p))
            .Select(p => new KeyValuePair<CachedPropertyInfo, Func<object>>(p, () => p.InnerMetaData.GetValue(o)))
            .ToDictionary();
    }

    public static IDictionary<string, object> FlattenAndSerializeToDictionary(this object o)
    {
        IDictionary<CachedPropertyInfo, object> properties = SerializeToDictionary(o);
        
        foreach (KeyValuePair<CachedPropertyInfo, object> property in properties)
        {
            if (property.Key.InnerMetaData.PropertyType.IsValueType || property.Key.InnerMetaData.PropertyType == typeof(string))
            {
                properties[property.Key] = property.Value;
                continue;
            }

            properties[property.Key] = FlattenAndSerializeToDictionary(property.Value);
        }

        return properties;
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