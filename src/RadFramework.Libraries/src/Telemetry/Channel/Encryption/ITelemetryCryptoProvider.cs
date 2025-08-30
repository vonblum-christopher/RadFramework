namespace RadFramework.Libraries.Telemetry.Channel.Encryption
{
    public interface ITelemetryCryptoProvider
    {
        byte[] Encrypt(byte[] package);
        byte[] Decrypt(byte[] encryptedPackage);
    }
}