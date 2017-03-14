using System;

namespace NetworkLayer
{
    public delegate void MessageEventHandler(IMessage message);

    public enum DeliveryMethod
    {
        //No guarantees, except for preventing duplicates.
        Unreliable,

        //Late messages will be dropped if newer ones were already received
        UnreliableSequenced,

        //All packages will arrive, but not necessarily in the same order.
        ReliableUnordered,

        //All packages will arrive, but late ones will be dropped.
        //This means that we will always receive the latest message eventually, but may miss older ones.
        ReliableSequenced,

        //All packages will arrive, and they will do so in the same order.
        //Unlike all the other methods, here the library will hold back messages until all previous ones are received, before handing them to us.
        ReliableOrdered
    }

    public interface IServer
    {
        event MessageEventHandler OnConnectionApproved;
        event MessageEventHandler OnConnectionDenied;
        event MessageEventHandler OnConnected;
        event MessageEventHandler OnDisconnected;
        event MessageEventHandler OnDisconnecting;
        event MessageEventHandler OnReceivedData;

        void RegisterDataCallback<T>(Action<IConnection, T> callback);

        void Start();

        int Port { get; }

        void HandleMessages();

        void SendAll(byte[] message, IConnection except, DeliveryMethod method, int channel);
        void Send(IConnection connection, byte[] message, DeliveryMethod method, int channel);
    }
}
