namespace RadFramework.Libraries.Encryption.Abtractions;

public interface IEncryptionProvider
{
    byte[] Encrypt(byte[] bytes, byte[] key);
    byte[] Decrypt(byte[] bytes, byte[] key);
}