using RadFramework.Libraries.Pipelines;
using RadFramework.Libraries.Pipelines.Base;
using RadFramework.Libraries.Web;

namespace RadDevelopers.Servers.Web.Pipelines.Http;

public class StaticImagePipe : ExtensionPipeBase<HttpConnection, HttpConnection>
{
    private string WWWRootPath = "wwwroot";
    
    private string[] imgExtensions = new[]
    {
        "png",
        "bmp",
        "jpeg",
        "jpg"
    };
    
    public override void Process(HttpConnection input, ExtensionPipeContext<HttpConnection> pipeContext)
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