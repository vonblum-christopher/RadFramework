using System.Security.Cryptography;
using RadFramework.Libraries.Abstractions;

namespace RadFramework.Libraries.Hashing;

public class HashingProvider<THashAlgorithm> : IHashingProvider
    where THashAlgorithm : HashAlgorithm, new()
{
    public byte[] Hash(byte[] data)
    {
        return new THashAlgorithm().ComputeHash(data);
    }
}