namespace RadFramework.Libraries.Abstractions;

public interface IAsymetricEncryptionProvider
{
    byte[] Encrypt(byte[] publicKey, byte[] data);
    byte[] Decrypt(byte[] privateKey, byte[] encryptedData);
}