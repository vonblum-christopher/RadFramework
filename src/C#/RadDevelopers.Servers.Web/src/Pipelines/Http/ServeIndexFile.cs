using System.Reflection;
using RadFramework.Libraries.Pipelines;
using RadFramework.Libraries.Pipelines.Base;
using RadFramework.Libraries.Pipelines.Parameters;
using RadFramework.Libraries.Web;

namespace RadDevelopers.Servers.Web.Pipelines.Http;

public class ServeIndexFile : ExtensionPipeBase<HttpConnection, HttpConnection>
{
    private string WWWRootPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/wwwroot";
    
    public override void Process(HttpConnection input, ExtensionPipeContext<HttpConnection> pipeContext)
    {
        if (input.Request.UrlPath == "/" || input.Request.UrlPath == "")
        {
            input.Response.TryServeStaticHtmlFile(WWWRootPath + "/index.html");
            pipeContext.Return();
            return;
        }
    }
}