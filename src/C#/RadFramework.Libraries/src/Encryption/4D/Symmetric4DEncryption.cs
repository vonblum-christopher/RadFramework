using System.Security.Cryptography;
using RadFramework.Libraries.Abstractions;

namespace RadFramework.Libraries.Encryption._4D;

public class Symmetric4DEncryption : ISymetricEncryptionProvider
{
    public byte[] Encrypt(byte[] bytes, byte[] key)
    {
        // rotate left to right by factor
        // rotate right to left by factor
        // split data into blocks and mix them
        // count bytes of the data up
        throw new NotImplementedException();
    }

    public byte[] Decrypt(byte[] bytes, byte[] key)
    {
        throw new NotImplementedException();
    }
}