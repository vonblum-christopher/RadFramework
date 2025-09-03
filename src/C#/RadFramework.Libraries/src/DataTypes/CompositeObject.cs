using System.Collections.Concurrent;

namespace RadFramework.Libraries.Patterns;

public class CompositeObject
{
    private ConcurrentDictionary<Type, object> objects = new();
    
    public T Get<T>()
    {
        return (T)objects[typeof(T)];
    }
    
    public object Get(Type t)
    {
        return objects[t];
    }

    public void Set<T>(T o)
    {
        objects[typeof(T)] = o;
    }
    
    public object Set(Type t, object o)
    {
        return objects[t];
    }
}