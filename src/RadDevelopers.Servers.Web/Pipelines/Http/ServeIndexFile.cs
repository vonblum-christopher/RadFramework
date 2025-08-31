using RadFramework.Libraries.Patterns.Pipeline;
using RadFramework.Libraries.Web;

namespace RadDevelopers.Servers.Web.Pipelines.Http;

public class ServeIndexFile: IHttpPipe
{
    public void Process(HttpConnection connection, ExtensionPipeContext pipeContext)
    {
    }
}