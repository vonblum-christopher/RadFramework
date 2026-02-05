using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using RadFramework.Libraries.Abstractions;

namespace RadFramework.Libraries.Hashing;

public class Sha512Provider : IHashingProvider
{
    public byte[] Hash(byte[] data)
    {
        return SHA512.HashData(data);
    }
}