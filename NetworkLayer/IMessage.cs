
namespace NetworkLayer
{
    public interface IMessage
    {
        string Description();
        IConnection Connection { get; }
        byte[] Data { get; }
    }
}
