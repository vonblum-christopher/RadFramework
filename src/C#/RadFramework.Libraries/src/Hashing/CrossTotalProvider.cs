using RadFramework.Libraries.Abstractions;

namespace RadFramework.Libraries.Hashing;

public class CrossTotalProvider : IHashingProvider
{
    public byte[] Hash(byte[] data)
    {
        ulong crossTotal = 0;

        unchecked
        {
            foreach (var b in data)
            {
                crossTotal += b;
            }            
        }
        
        return BitConverter.GetBytes(crossTotal);
    }
}