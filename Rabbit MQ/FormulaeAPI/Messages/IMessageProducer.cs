namespace FormulaeAPI.Messages
{
    public interface IMessageProducer
    {
        public void SendingMessages<T>(T message);
    }
}
