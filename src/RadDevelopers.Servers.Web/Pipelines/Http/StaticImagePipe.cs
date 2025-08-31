using RadFramework.Libraries.Patterns.Pipeline;
using RadFramework.Libraries.Web;

namespace RadDevelopers.Servers.Web.Pipelines.Http;

public class StaticImagePipe : IHttpPipe
{
    private string WWWRootPath = "wwwroot";
    
    private string[] imgExtensions = new[]
    {
        "png",
        "bmp",
        "jpeg",
        "jpg"
    };
    
    public void Process(HttpConnection connection, ExtensionPipeContext pipeContext)
    {
        string fileExtension = connection.Request.UrlPath.Substring(connection.Request.UrlPath.LastIndexOf('.') + 1);

        if (imgExtensions.Contains(fileExtension))
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