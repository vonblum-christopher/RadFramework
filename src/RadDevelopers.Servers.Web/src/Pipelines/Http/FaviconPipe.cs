using RadFramework.Libraries.Pipelines;
using RadFramework.Libraries.Pipelines.Base;
using RadFramework.Libraries.Pipelines.Parameters;
using RadFramework.Libraries.Web;

namespace RadDevelopers.Servers.Web.Pipelines.Http;

public class FaviconPipe : ExtensionPipeBase<HttpConnection, HttpConnection>
{
    private string WWWRootPath = "wwwroot";

    public override void Process(HttpConnection input, ExtensionPipeContext<HttpConnection> pipeContext)
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