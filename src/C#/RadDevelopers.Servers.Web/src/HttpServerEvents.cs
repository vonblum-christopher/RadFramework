using RadFramework.Libraries.Web.Interfaces;

namespace RadDevelopers.Servers.Web;

public class HttpServerEvents
{
    public OnRequestDelegate OnRequest { get; set; }
    public OnErrorDelegate OnError { get; set; }
    public OnFatalErrorDelegate OnFatalError { get; set; }
    public OnWebsocketConnectedDelegate OnWebsocketConnected { get; set; }
}