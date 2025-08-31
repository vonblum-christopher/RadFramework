using RadFramework.Libraries.Patterns.Pipeline;
using RadFramework.Libraries.Web;

namespace RadDevelopers.Servers.Web.Pipelines.Http;

public class FaviconPipe : IHttpPipe
{
    private string WWWRootPath = "wwwroot";
    
    public void Process(HttpConnection connection, ExtensionPipeContext pipeContext)
    {
        string fileExtension = connection.Request.UrlPath.Substring(connection.Request.UrlPath.LastIndexOf('.') + 1);

        if (fileExtension == "ico")
        {
            connection.Response.SendFile(
                connection
                    .ServerContext
                    .Cache
                    .GetOrSet(
                        connection.Request.UrlPath, 
                        () => File.ReadAllBytes(WWWRootPath + connection.Request.UrlPath)));
            
            pipeContext.Return();
            return;
        }
    }
}