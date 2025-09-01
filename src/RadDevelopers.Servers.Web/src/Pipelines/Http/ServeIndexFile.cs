using RadFramework.Libraries.Pipelines;
using RadFramework.Libraries.Pipelines.Base;
using RadFramework.Libraries.Web;

namespace RadDevelopers.Servers.Web.Pipelines.Http;

public class ServeIndexFile : ExtensionPipeBase<HttpConnection, HttpConnection>
{
    public override void Process(HttpConnection input, ExtensionPipeContext<HttpConnection> pipeContext)
    {
    }
}