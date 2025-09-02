using RadFramework.Libraries.Ioc;
using RadFramework.Libraries.Pipelines.Builder;
using IocContainer = RadFramework.Libraries.Ioc.IocContainer;

namespace RadFramework.Libraries.Web;

public class HttpServerWithPipeline : IDisposable
{
    private readonly ExtensionPipeline<HttpConnection> httpPipeline;
    private HttpServer server;
    private HttpServerContext ServerContext;
    private readonly ExtensionPipeline<HttpError> httpErrorPipeline;

    public HttpServerWithPipeline(
        int port,
        PipelineBuilder httpPipelineBuilder,
        PipelineBuilder httpErrorPipelineBuilder,
        IocContainer iocContainer,
        Action<HttpRequest, System.Net.Sockets.Socket> webSocketConnected = null)
    {
        iocContainer.RegisterSingleton<HttpServerContext>();
        ServerContext = iocContainer.Resolve<HttpServerContext>();

        server = new HttpServer(port, ProcessRequestUsingPipeline, null);
    }

    private void ProcessRequestUsingPipeline(HttpConnection connection)
    {
        connection.ServerContext = ServerContext;

        try
        {
            if (!httpPipeline.Process(connection))
            {
                httpErrorPipeline.Process(new HttpError { Connection = connection, Exception = null });
            }
        }
        catch (Exception e)
        {
            try
            {
                httpErrorPipeline.Process(new HttpError { Connection = connection, Exception = null });
            }
            catch (Exception ee)
            {
                ServerContext.Logger.LogError("HttpPipeline crashed while processing request. When the error should have been dealt with an exception occured too. Review your pipeline implementations.");
            }
        }
    }
    
    public void Dispose()
    {
        server.Dispose();
    }
}