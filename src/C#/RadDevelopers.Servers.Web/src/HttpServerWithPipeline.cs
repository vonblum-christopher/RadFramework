using System.Net;
using RadDevelopers.Servers.Web.Pipelines.Definitions;
using RadFramework.Libraries.Ioc;
using RadFramework.Libraries.Ioc.Builder;
using RadFramework.Libraries.Pipelines.Builder;
using RadFramework.Libraries.Web;
using RadFramework.Libraries.Web.Models;

namespace RadDevelopers.Servers.Web;

public class HttpServerWithPipeline : IDisposable
{
    private readonly HttpServerEvents events;
    private HttpServer server;
    private HttpGlobalServerContext serverContext;
    
    public HttpServerWithPipeline(IEnumerable<IPEndPoint> listenerEndpoints, HttpServerEvents events)
    {
        this.events = events;
        server = new HttpServer(listenerEndpoints, events);
    }
        
    private void ProcessRequest(HttpConnection connection)
    {
        connection.ServerContext = serverContext;

        try
        {
            events.OnHttpRequest(connection);
        }
        catch (Exception initialError)
        {
            try
            {
                events.OnHttpError(new HttpError
                {
                    Connection = connection,
                    Exception = initialError,
                });
            }
            catch (Exception errorWhileErrorHandling)
            {
                events.OnHttpErrorHandlingFailedToo(new HttpError
                {
                    Connection = connection,
                    Exception = errorWhileErrorHandling,
                });
            }
        }
    }

    public void Dispose()
    {
        server.Dispose();
    }
}