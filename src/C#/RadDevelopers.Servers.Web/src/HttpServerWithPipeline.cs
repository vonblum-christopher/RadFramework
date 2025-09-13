using System.Net;
using RadDevelopers.Servers.Web.Pipelines.Definitions;
using RadFramework.Libraries.Ioc;
using RadFramework.Libraries.Ioc.Builder;
using RadFramework.Libraries.Pipelines;
using RadFramework.Libraries.Pipelines.Builder;
using RadFramework.Libraries.Web;
using RadFramework.Libraries.Web.Models;

namespace RadDevelopers.Servers.Web;

public class HttpServerWithPipeline : IDisposable
{
    private HttpServer server;
    private HttpGlobalServerContext serverContext;
    private readonly HttpServerEvents events;

    public HttpServerWithPipeline(IEnumerable<IPEndPoint> listenerEndpoints,
        ExtensionPipeline<HttpConnection, HttpConnection> httpPipeline,
        ExtensionPipeline<HttpError, HttpError> httpErrorPipeline)
    {
        this.events =
            new HttpServerEvents()
            {
                OnHttpRequestDelegate = connection => httpPipeline.Process(connection),
                OnHttpErrorDelegate = (HttpConnection connection, HttpError error) => httpErrorPipeline.Process(new HttpError()
                {
                    Connection = connection,
                    Exception = error.Exception
                }),
                OnHttpErrorHandlingFailedTooDelegate = error => error.Connection.Response.Send500()
            };
        
        server = new HttpServer(listenerEndpoints, events);
    }
        
    private void ProcessRequest(HttpConnection connection)
    {
        connection.ServerContext = serverContext;

        try
        {
            events.OnHttpRequestDelegate(connection);
        }
        catch (Exception initialError)
        {
            try
            {
                events.OnHttpErrorDelegate(connection, new HttpError
                {
                    Connection = connection,
                    Exception = initialError,
                });
            }
            catch (Exception errorWhileErrorHandling)
            {
                events.OnHttpErrorHandlingFailedTooDelegate(new HttpError
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