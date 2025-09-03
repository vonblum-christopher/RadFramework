using RadFramework.Libraries.Abstractions;

namespace RadFramework.Libraries.FileSystem;

public class FileSystem : IFileSystem
{
    public bool ExistsDirectory(string path)
    {
        return Directory.Exists(path);
    }

    public void CreateDirectory(string path)
    {
        Directory.CreateDirectory(path);
    }

    public void CopyDirectory(string path, string destination)
    {
        foreach (string dirPath in Directory.GetDirectories(path, "*", SearchOption.AllDirectories))
        {
            Directory.CreateDirectory(dirPath.Replace(path, destination));
        }

        //Copy all the files & Replaces any files with the same name
        foreach (string newPath in Directory.GetFiles(path, "*.*",SearchOption.AllDirectories))
        {
            File.Copy(newPath, newPath.Replace(path, destination), true);
        }
    }

    public bool ExistsFile(string path)
    {
        return File.Exists(path);
    }

    public void CreateFile(string path)
    {
        File.Create(path);
    }

    public void CopyFile(string path, string destination)
    {
        File.Copy(path, destination);
    }

    public void WriteFile(string path, IEnumerable<byte[]> content)
    {
        CreateFile(path);

        FileInfo file = new FileInfo(path);

        FileStream writer = file.OpenWrite();

        foreach (byte[] part in content)
        {
            writer.Write(part);
        }
        
        writer.Flush();
        writer.Close();
    }

    public IEnumerable<byte[]> ReadFile(string path)
    {
        FileInfo file = new FileInfo(path);

        FileStream reader = file.OpenRead();

        byte[] buffer = new byte[1024];

        int readBytes = 0;

        do
        {
            readBytes = reader.Read(buffer);
            yield return buffer;
            buffer = new byte[1024];
        } while (readBytes != 0);
        
        reader.Flush();
        reader.Close();
    }
}