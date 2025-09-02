namespace RadFramework.Libraries.Abstractions;

public interface IChecksumProvider
{
    ulong Hash(byte[] data);
}