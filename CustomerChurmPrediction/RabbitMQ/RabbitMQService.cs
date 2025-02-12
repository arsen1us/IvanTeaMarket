namespace CustomerChurmPrediction.RabbitMQ
{
    public interface IRabbitMQService
    {
        /// <summary>
        /// Отправить объект
        /// </summary>
        /// <param name="obj">Объект</param>
        void SendMessage(object obj);

        /// <summary>
        /// Отправить текстовое сообщение
        /// </summary>
        /// <param name="message">Сообщение</param>
        void SendMessage(string message);
    }
    public class RabbitMQService : IRabbitMQService
    {
        public void SendMessage(object obj)
        {
            if (obj is null)
            {
                throw new ArgumentNullException($"{nameof(obj)}");
            }
            try
            {

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void SendMessage(string message)
        {
            if(string.IsNullOrEmpty(message))
            {
                throw new ArgumentNullException($"{nameof(message)}");
            }
            try
            {

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
