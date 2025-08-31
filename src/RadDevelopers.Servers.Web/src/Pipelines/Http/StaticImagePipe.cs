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
    
    public void Process(HttpConnection input, ExtensionPipeContext pipeContext)
    {
        string fileExtension = input.Request.UrlPath.Substring(input.Request.UrlPath.LastIndexOf('.') + 1);

        if (imgExtensions.Contains(fileExtension))
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