using System.Text;
using RadFramework.Libraries.Abstractions;
using RadFramework.Libraries.Utils;

namespace RadFramework.Libraries.DataTypes;

public class PrefixedLengthString : ISerializable
{
    public int Length { get; private set; }
    
    public byte[] Data { get; private set; }
    
    public PrefixedLengthString(string str)
    {
        Length = str.Length;
        Data = Encoding.Unicode.GetBytes(str);
    }

    public PrefixedLengthString(byte[] bytes)
    {
        int length = BitConverter.ToInt32(bytes, 0);
        Data = bytes.Skip(4).Take(length - 4).ToArray();
    }
    
    private PrefixedLengthString()
    {
    }

    public byte[] Serialize()
    {
        return BitConverter.GetBytes(Length).Concat(Data).ToArray();
    }

    public static PrefixedLengthString operator +(PrefixedLengthString strA, PrefixedLengthString strB)
    {
        int length = strA.Length + strB.Length;

        PrefixedLengthString str = new PrefixedLengthString();

        str.Length = length;
        str.Data = strA.Data.Concat(strB.Data).ToArray();

        return str;
    }
}