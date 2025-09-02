using RadFramework.Libraries.Abstractions;

namespace RadFramework.Libraries.Checksum;

public class ChecksumProvider : IChecksumProvider
{
    public ulong Hash(byte[] data)
    {
        unchecked
        {
            ulong checksum = 0;
    
            foreach (var b in data)
            {
                checksum += b;
            }
            
            return checksum;
        }
    }
}