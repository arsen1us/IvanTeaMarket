using RabbitMQ.Client;


namespace CustomerChurmPrediction.RabbitMQ


{
    public interface IRabbitMQService
    {
        /// <summary>
        /// Отправить сообщение Consumer-ам
        /// </summary>
        /// <param name="obj">Объект отправки</param>
        public void PublishMessage(object obj);
    }
    public class RabbitMQService : IRabbitMQService
    {
        IConnection _connection;
        IChannel _channel;
        public RabbitMQService(IConfiguration _config)
        {
            var factory = new ConnectionFactory { HostName = _config["RabbitMQ:Host"] };
        }

        public void PublishMessage(object obj)
        {
            if (obj is null)
                throw new ArgumentNullException($"{nameof(obj)}");
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
