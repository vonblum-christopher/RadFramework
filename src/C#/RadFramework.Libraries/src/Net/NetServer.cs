using System.Net.Sockets;
using RadFramework.Libraries.Socket;

namespace RadFramework.Libraries.Net;

public class NetServer
{
    private SocketConnectionListener Listener =
        new SocketConnectionListener(SocketType.Stream, ProtocolType.Udp, OnSocketConnected,1234);

    private static void OnSocketConnected(System.Net.Sockets.Socket obj)
    {
        
    }
}