using RadFramework.Libraries.Pipelines;
using RadFramework.Libraries.Pipelines.Base;
using RadFramework.Libraries.Pipelines.Parameters;

namespace RadDevelopers.Servers.Web.Pipelines.HttpError;

public class Send500InternalServerError : ExtensionPipeBase<RadFramework.Libraries.Web.HttpError, RadFramework.Libraries.Web.HttpError>
{
    public override void Process(RadFramework.Libraries.Web.HttpError input, ExtensionPipeContext<RadFramework.Libraries.Web.HttpError> pipeContext)
    {
        input.Response.Send500();
    }
}