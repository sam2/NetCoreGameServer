using System;

namespace NetworkLayer
{
    public delegate void MessageEventHandler(IMessage message);

    public interface IServer
    {
        event MessageEventHandler OnConnectionApproved;
        event MessageEventHandler OnConnectionDenied;
        event MessageEventHandler OnConnected;
        event MessageEventHandler OnDisconnected;
        event MessageEventHandler OnReceivedData;

        void RegisterDataCallback<T>(Action<T> callback);

        void Start();

        int Port { get; }

        void HandleMessages();
    }
}
