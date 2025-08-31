using RadFramework.Libraries.Patterns.Pipeline;
using RadFramework.Libraries.Web;

namespace RadDevelopers.Servers.Web.Pipelines.HttpError;

public class Send500InternalServerError : IHttpErrorPipe
{
    public void Process(RadFramework.Libraries.Web.HttpError input, ExtensionPipeContext pipeContext)
    {
        input.Response.Send500();
    }
}