// See https://aka.ms/new-console-template for more information

using RadFramework.Libraries.DataTypes;
using RadFramework.Libraries.Reflection.Caching;

namespace RadDevelopers.Servers.Database;

public class TreeDatabase<TRecordId>
{
    private Dictionary<PrefixedLengthString, PrefixedLengthString> index = new Dictionary<PrefixedLengthString, PrefixedLengthString>();

    public TreeDatabase(string indexFilePath, string schemeFilePath, string[] databaseFiles)
    {
        byte[] indexFile = File.ReadAllBytes(indexFilePath);

        PrefixedLengthString;
            
        for (var i = 0; i < indexFile.Length; i++)
        {
            string part = string.Empty;
            var c = indexFile[i];

            if (c == '\n')
            {
                part = null;
                continue;
            }
            
            part += c;
        }
    }
    
    public TRecord Create<TRecord>(string path) where TRecord : TRecordId
    {
        CachedType tRecord = typeof(TRecord);
        
        
        // append record to the end of the file
        // load records by seeking backwards
        // optimize deletes records from the beginning when the id duplicates
        // delete deletes all occurences of the path
    }

    public void Read<TRecord>(string path) where TRecord : TRecordId
    {
        throw new NotImplementedException();
    }

    public void Update<TRecord>(string path) where TRecord : TRecordId
    {
        throw new NotImplementedException();
    }

    public void Delete<TRecord>(string path) where TRecord : TRecordId
    {
        throw new NotImplementedException();
    }

    private void Optimize()
    {
        
    }
}