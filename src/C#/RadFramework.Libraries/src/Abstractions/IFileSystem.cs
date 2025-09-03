namespace RadFramework.Libraries.Abstractions;

public interface IFileSystem
{
    bool ExistsDirectory(string path);
    void CreateDirectory(string path);
    void CopyDirectory(string path, string destination);
    bool ExistsFile(string path);
    void CreateFile(string path);
    void CopyFile(string path, string destination);
    void WriteFile(string path, IEnumerable<byte[]> content);
    IEnumerable<byte[]> ReadFile(string path);
}