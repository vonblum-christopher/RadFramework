using RadFramework.Libraries.Web.Interfaces;

namespace RadDevelopers.Servers.Web;

public class HttpServerEvents
{
    public OnHttpRequestDelegate OnHttpRequest { get; set; }
    public OnHttpErrorDelegate OnHttpError { get; set; }
    public OnHttpErrorHandlingFailedTooDelegate OnHttpErrorHandlingFailedToo { get; set; }
    public OnHttpWebsocketConnectedDelegate OnHttpWebsocketConnected { get; set; }
}