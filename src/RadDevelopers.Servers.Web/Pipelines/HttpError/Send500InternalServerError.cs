using RadFramework.Libraries.Extensibility.Pipeline.Extension;
using RadFramework.Libraries.Web;

namespace RadDevelopers.Servers.Web.Pipelines.HttpError;

public class Send500InternalServerError : IHttpErrorPipe
{
    public void Process(RadFramework.Libraries.Web.HttpError context, ExtensionPipeContext pipeContext)
    {
        context.Response.Send500();
    }
}