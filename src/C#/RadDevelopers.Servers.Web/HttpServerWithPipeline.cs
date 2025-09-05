using RadDevelopers.Servers.Web.Pipelines.Definitions;
using RadFramework.Libraries.Ioc;
using RadFramework.Libraries.Ioc.Builder;
using RadFramework.Libraries.Pipelines.Builder;
using RadFramework.Libraries.Web;

namespace RadDevelopers.Servers.Web;

public class HttpServerWithPipeline : IDisposable
{
    private readonly HttpServerEvents events;
    private HttpServer server;
    private HttpGlobalServerContext serverContext;
    
    public HttpServerWithPipeline(int port, HttpServerEvents events)
    {
        this.events = events;
        server = new HttpServer(port,
            ProcessRequest,
            (socket,
                thread,
                exception) => events.OnError(new HttpError()
                    {
                        
                    }));
    }
        
    private void ProcessRequest(HttpConnection connection)
    {
        connection.ServerContext = serverContext;

        try
        {
            events.OnRequest(connection);
        }
        catch (Exception initialError)
        {
            try
            {
                events.OnError(new HttpError
                {
                    Connection = connection,
                    Exception = initialError,
                });
            }
            catch (Exception errorWhileErrorHandling)
            {
                events.OnFatalError(new HttpError
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