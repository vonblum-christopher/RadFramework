using RadFramework.Libraries.Pipelines;
using RadFramework.Libraries.Pipelines.Base;
using RadFramework.Libraries.Pipelines.Parameters;

namespace RadDevelopers.Servers.Web.Pipelines.HttpError;

public class Send500InternalServerError : ExtensionPipeBase<RadFramework.Libraries.Web.Models.HttpError, RadFramework.Libraries.Web.Models.HttpError>
{
    public override void Process(RadFramework.Libraries.Web.Models.HttpError input, ExtensionPipeContext<RadFramework.Libraries.Web.Models.HttpError> pipeContext)
    {
        input.Response.Send500();
    }
}