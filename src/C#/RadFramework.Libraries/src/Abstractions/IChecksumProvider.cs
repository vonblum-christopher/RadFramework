namespace RadFramework.Libraries.Abstractions;

public interface IChecksumProvider
{
    ulong CalculateChecksum(byte[] data);
}