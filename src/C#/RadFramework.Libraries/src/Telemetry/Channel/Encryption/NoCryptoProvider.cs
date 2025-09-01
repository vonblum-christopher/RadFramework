namespace RadFramework.Libraries.Telemetry.Channel.Encryption
{
    public class NoCryptoProvider : ITelemetryCryptoProvider
    {
        public byte[] Encrypt(byte[] package)
        {
            return package;
        }

        public byte[] Decrypt(byte[] encryptedPackage)
        {
            return encryptedPackage;
        }
    }
}