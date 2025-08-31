using RadFramework.Libraries.Extensibility.Pipeline.Extension;
using RadFramework.Libraries.Web;

namespace RadDevelopers.Servers.Web.Pipelines.Http;

public class ServeIndexFile: IHttpPipe
{
    public void Process(HttpConnection context, ExtensionPipeContext pipeContext)
    {
    }
}