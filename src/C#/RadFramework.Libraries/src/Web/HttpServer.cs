using System.Net.Sockets;
using RadDevelopers.Servers.Web;
using RadFramework.Libraries.Serialization.Json;
using RadFramework.Libraries.Socket;
using RadFramework.Libraries.Threading.Internals;
using RadFramework.Libraries.Threading.ThreadPools.Queued;

namespace RadFramework.Libraries.Web;

public class HttpServer : IDisposable
{
    private readonly OnRequestDelegate processRequest;
    private readonly OnProcessingError onProcessingError;
    private SocketConnectionListener listener;
    private QueuedThreadPool<System.Net.Sockets.Socket> httpRequestProcessingPool;
    public HttpServer(
        int port,
        OnRequestDelegate processRequest, 
        OnProcessingError onProcessingError)
    {
        this.processRequest = processRequest;
        this.onProcessingError = onProcessingError;

        httpRequestProcessingPool = 
            new QueuedThreadPool<System.Net.Sockets.Socket>(
                2,
                ThreadPriority.Highest,
                ProcessHttpSocketConnection,
                onProcessingError,
                "RadFramework.Libraries.Web.HttpServer-RequestProcessingPool");
        
        listener = new SocketConnectionListener(
            SocketType.Stream,
            ProtocolType.Tcp,
            port,
            OnSocketAccepted);//start the next thread when the listener accepted a socket
    }

    private void OnSocketAccepted(System.Net.Sockets.Socket connectionSocket)
    {
        httpRequestProcessingPool.Enqueue(connectionSocket);
    }

    private void ProcessHttpSocketConnection(System.Net.Sockets.Socket socketConnection)
    {
        NetworkStream networkStream = new(socketConnection);
        
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
                UnderlyingSocket = socketConnection
            };

        try
        {
            processRequest(connection);
        }
        catch (Exception e)
        {
            connection.ServerContext.Logger.LogError("[fatal] even the error handler crashed.");
        }
        
        networkStream.Flush();
        
        connection.Response.Dispose();
        
        requestReader.Dispose();
        networkStream.Dispose();
            
        socketConnection.Close();
        socketConnection.Dispose();
    }

    public void Dispose()
    {
        listener.Dispose();
        httpRequestProcessingPool.Dispose();
    }
}