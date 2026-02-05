using RadFramework.Libraries.Abstractions;
using RadFramework.Libraries.DataTypes;
using RadFramework.Libraries.Reflection.Caching;

namespace RadFramework.Libraries.Serialization.Binary;

public class BinarySerializer : IContractSerializer
{
    public byte[] Serialize(CachedType type, object model)
    {
        throw new NotImplementedException();
    }

    public object Deserialize(CachedType type, byte[] data)
    {
        throw new NotImplementedException();
    }

    public object Clone(CachedType type, object model)
    {
        throw new NotImplementedException();
    }
}

public class SchemeHeader
{
    private Scheme Schemes { get; set; }
}

public class SchemeProperty
{
    public PrefixedLengthString Name { get; set; }
    public PrefixedLengthString AssemblyQualifiedTypeName { get; set; }
}

public class Scheme
{
    public Guid SchemeId { get; set; }

    public PrefixedLengthString AssemblyQualifiedTypeName { get; set; }
    
    public IEnumerable<SchemeProperty> Properties { get; set; }
}