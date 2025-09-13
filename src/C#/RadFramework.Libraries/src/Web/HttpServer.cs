using System.Net;
using System.Net.Sockets;
using RadFramework.Libraries.Serialization.Json;
using RadFramework.Libraries.Socket;
using RadFramework.Libraries.Threading.Interface;
using RadFramework.Libraries.Threading.ThreadPools;
using RadFramework.Libraries.Threading.ThreadPools.Internals;
using RadFramework.Libraries.Web.Interfaces;
using RadFramework.Libraries.Web.Models;

namespace RadFramework.Libraries.Web;

public class HttpServer : IDisposable
{
    private readonly OnHttpRequestDelegate processHttpRequestDelegate;
    private List<SocketConnectionListener> listeners;
    private QueuedThreadPool<(System.Net.Sockets.Socket socket, HttpConnection connection)> httpRequestProcessingPool;
    public HttpServer(
        IEnumerable<IPEndPoint> listenerEndpoints,
        HttpServerEvents events)
    {
        this.processHttpRequestDelegate = events.OnHttpRequestDelegate;

        httpRequestProcessingPool = 
            new QueuedThreadPool<(System.Net.Sockets.Socket socket, HttpConnection connection)>(
                2,
                ThreadPriority.Highest,
                ProcessHttpSocketConnection,
                (task, thread, exception) => 
                    events.OnHttpErrorDelegate(task.connection,
                        new HttpError()
                        {
                            Connection = task.connection,
                            Exception = exception
                        }),
                "RadFramework.Libraries.Web.HttpServer-RequestProcessingPool");
        
        listeners = listenerEndpoints.Select(endpoint => new SocketConnectionListener(
            endpoint,
            SocketType.Stream,
            ProtocolType.Tcp,
            OnSocketAccepted))
            .ToList();//start the next thread when the listener accepted a socket
    }

    private void OnSocketAccepted(System.Net.Sockets.Socket connectionSocket)
    {
        httpRequestProcessingPool.Enqueue((connectionSocket, new HttpConnection()));
    }

    private void ProcessHttpSocketConnection((System.Net.Sockets.Socket socket, HttpConnection connection) connectionObjects)
    {
        NetworkStream networkStream = new(connectionObjects.socket);
        
        StreamReader requestReader = new(networkStream);
        
        string firstRequestLine = requestReader.ReadLine();
        
        HttpRequest requestModel = new();
        
        requestModel.Method = HttpRequestParser.ExtractHttpMethod(firstRequestLine);
        requestModel.Url = HttpRequestParser.ExtractUrl(firstRequestLine);
        requestModel.UrlPath = HttpRequestParser.ExtractUrl(firstRequestLine);
        requestModel.QueryString = HttpRequestParser.ExtractQueryString(requestModel.Url);
        requestModel.HttpVersion = HttpRequestParser.ExtractHttpVersion(firstRequestLine);

        string currentHeaderLine = null;
        
        while ((currentHeaderLine = requestReader.ReadLine()) != "")
        {
            var header = HttpRequestParser.ReadHeader(currentHeaderLine);
            requestModel.Headers.Add(header.header, header.value);
        }
            
        HttpConnection connection =
            new()
            {
                Request = requestModel,
                RequestReader = requestReader,
                UnderlyingStream = networkStream,
                UnderlyingSocket = connectionObjects.connection.UnderlyingSocket
            };

        try
        {
            processHttpRequestDelegate(connection);
        }
        catch (Exception e)
        {
            connection.ServerContext.Logger.LogError("[fatal] even the error handler crashed.");
        }
        
        networkStream.Flush();
        
        connection.Response.Dispose();
        
        requestReader.Dispose();
        networkStream.Dispose();
            
        connectionObjects.connection.DisposeReaderAndStream();
    }

    public void Dispose()
    {
        listeners.ForEach(l => l.Dispose());
        httpRequestProcessingPool.Dispose();
    }
}