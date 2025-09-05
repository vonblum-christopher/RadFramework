namespace RadFramework.Libraries.Abstractions;

public interface ISocket
{
    void Send(byte[] data);
    byte[] Receive();
}