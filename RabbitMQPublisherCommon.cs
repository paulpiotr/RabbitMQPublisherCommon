using RabbitMQ.Client;
using System;
using System.Reflection;
using System.Text;

namespace RabbitMQPublisherCommon
{
    public class RabbitMQPublisherCommon
    {
        #region private static readonly log4net.ILog _log4net
#if !NET48
        /// <summary>
        /// Log4 Net Logger
        /// </summary>
        private static readonly log4net.ILog _log4net = Log4netLogger.Log4netLogger.GetLog4netInstance(MethodBase.GetCurrentMethod().DeclaringType);
#endif
        #endregion

        #region public static void Publish(string queueName, string message, string hostName = "localhost", string userName = "guest", string password = "guest", int port = 5672)
        /// <summary>
        /// Opublikuj wiadomość
        /// Post the message
        /// </summary>
        /// <param name="queueName">
        /// Nazwa kolejki
        /// Queue name
        /// </param>
        /// <param name="message">
        /// Wiadomość do wysłania
        /// Message to be sent
        /// </param>
        public static void Publish(string queueName, string message, string hostName = "localhost", string userName = "guest", string password = "guest", int port = 5672)
        {
            try
            {
#if !NET48
                _log4net.Info($"{ queueName }, { message }");
#endif
                IConnectionFactory rabbitConnectionFactory = new ConnectionFactory()
                {
                    HostName = hostName,
                    UserName = userName,
                    Password = password,
                    Port = port,
#if !NET48
                    RequestedConnectionTimeout = TimeSpan.FromSeconds(1),
#else
                    RequestedConnectionTimeout = TimeSpan.FromSeconds(1).Seconds,
#endif
                };
                using (IConnection connection = rabbitConnectionFactory.CreateConnection())
                {
                    using (IModel channel = connection.CreateModel())
                    {
                        channel.QueueDeclare(
                            queue: queueName,
                            durable: false,
                            exclusive: false,
                            autoDelete: false,
                            arguments: null);
                        byte[] body = Encoding.UTF8.GetBytes(message);
                        channel.BasicPublish(
                            exchange: string.Empty,
                            routingKey: queueName,
                            basicProperties: null,
                            body);
#if !NET48
                        _log4net.Info($"Sent { queueName }, { message }");
#endif
                    }
                }
            }
            catch (Exception e)
            {
#if !NET48
                _log4net.Error(string.Format("{0}, {1}.", e.Message, e.StackTrace), e);
#endif
            }
        }
        #endregion
    }
}