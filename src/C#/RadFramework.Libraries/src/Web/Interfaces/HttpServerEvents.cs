using RadFramework.Libraries.Threading.Interface;
using RadFramework.Libraries.Web.Interfaces;

namespace RadFramework.Libraries.Web;

public class HttpServerEvents
{
    public OnHttpRequestDelegate OnHttpRequestDelegate;
    public OnHttpWebsocketConnectedDelegate OnHttpWebsocketConnectedDelegate;
    public OnHttpErrorDelegate OnHttpErrorDelegate;
    public OnHttpErrorHandlingFailedTooDelegate OnHttpErrorHandlingFailedTooDelegate;
}