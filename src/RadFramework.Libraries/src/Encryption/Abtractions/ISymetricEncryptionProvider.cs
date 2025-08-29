namespace RadFramework.Libraries.Encryption.Abtractions;

public interface ISymetricEncryptionProvider
{
    byte[] Encrypt(byte[] bytes, byte[] key);
    byte[] Decrypt(byte[] bytes, byte[] key);
}