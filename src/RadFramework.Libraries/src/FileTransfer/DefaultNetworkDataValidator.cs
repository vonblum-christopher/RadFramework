namespace RadFramework.Libraries.FileTransfer;

public class DefaultNetworkDataValidator : INetworkDataValidator
{     
    public bool IsValid(byte[] downloadedData, ulong hashFromHeader)
    {
        return Hash(downloadedData) == hashFromHeader;
    }
     
    private static ulong Hash(byte[] bytes)
    {
        ulong hash = 0;
 
        unchecked
        {
            for (int i = 0; i < bytes.Length; i++)
            {
                hash += bytes[i];
            }
        }
         
        return hash;
    }


}