namespace RadFramework.Libraries.Abstractions;

public interface IHashingProvider
{
    byte[] Hash(byte[] data);
}