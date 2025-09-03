using RadFramework.Libraries.Checksum;

namespace RadFramework.Libraries.VCS;

public class RadVcs
{
    private readonly string repositoryPath;
    private ChecksumProvider checksumProvider = new ();

    public RadVcs(string repositoryPath)
    {
        if (!Directory.Exists(repositoryPath))
        {
            Directory.CreateDirectory(repositoryPath);
        }
        
        this.repositoryPath = repositoryPath;
    }
    
    public void StoreVersions(params string[] files)
    {
        
    }
    
    public void RestoreVersions(params string[] files)
    {
        
    }
    
    public void StoreVersion(string file)
    {
        
    }
    
    public void RestoreVersion(string file)
    {
        
    }

    public void StoreAll()
    {
        
    }
}