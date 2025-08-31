using RadFramework.Libraries.Patterns.Pipeline;
using RadFramework.Libraries.Web;

namespace RadDevelopers.Servers.Web.Pipelines.Http;

public class FaviconPipe : IHttpPipe
{
    private string WWWRootPath = "wwwroot";
    
    public void Process(HttpConnection input, ExtensionPipeContext pipeContext)
    {
        string fileExtension = input.Request.UrlPath.Substring(input.Request.UrlPath.LastIndexOf('.') + 1);

        if (fileExtension == "ico")
        {
            input.Response.SendFile(
                input
                    .ServerContext
                    .Cache
                    .GetOrSet(
                        input.Request.UrlPath, 
                        () => File.ReadAllBytes(WWWRootPath + input.Request.UrlPath)));
            
            pipeContext.Return();
            return;
        }
    }
}