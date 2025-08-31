using System.Reflection;
using RadFramework.Libraries.Patterns.Pipeline;
using RadFramework.Libraries.Web;

namespace RadDevelopers.Servers.Web.Pipelines.Http;

public class StaticHtmlPipe : IHttpPipe
{
    private string WWWRootPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/wwwroot";
    
    private string[] htmlExtensions = new[]
    {
        "htm",
        "html"
    };
    
    public void Process(HttpConnection input, ExtensionPipeContext pipeContext)
    {
        string urlPath = input.Request.UrlPath;
        
        if (input.Request.UrlPath == "/" || input.Request.UrlPath == "")
        {
            input.Response.TryServeStaticHtmlFile(WWWRootPath + "/index.html");
            pipeContext.Return();
            return;
        }
        
        if (urlPath.EndsWith('/'))
        {
            urlPath = urlPath.TrimEnd('/');
        }

        string[] segments = urlPath.Split('/');

        string fileName = segments.Last();

        if (fileName.Contains('.'))
        {
            string fileExtension = fileName.Substring(fileName.LastIndexOf('.') + 1);

            if (htmlExtensions.Contains(fileExtension))
            {
                input.Response.TryServeStaticHtmlFile(WWWRootPath + input.Request.UrlPath);
                pipeContext.Return();
                return;             
            }
        }
    }
}