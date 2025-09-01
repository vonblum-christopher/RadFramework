namespace RadFramework.Libraries.FileTransfer;

public interface INetworkDataValidator
{
    bool IsValid(byte[] downloadedData, ulong hashFromHeader);

}