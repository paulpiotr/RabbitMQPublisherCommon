using RabbitMQ.Client;
using System;
using System.Reflection;
using System.Text;

namespace RabbitMQPublisherCommon
{
    public class RabbitMQPublisherCommon
    {
        #region private static readonly log4net.ILog log4net
#if !NET48
        /// <summary>
        /// Log4 Net Logger
        /// </summary>
        private static readonly log4net.ILog log4net = Log4netLogger.Log4netLogger.GetLog4netInstance(MethodBase.GetCurrentMethod().DeclaringType);
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
                log4net.Debug($"{ queueName }, { message }");
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
                        log4net.Debug($"Sent { queueName }, { message }");
#endif
                    }
                }
            }
#if !NET48
            catch (Exception e)
            {

                log4net.Error(string.Format("\n{0}\n{1}\n{2}\n{3}\n", e.GetType(), e.InnerException?.GetType(), e.Message, e.StackTrace), e);
            }
#else
            catch (Exception e)
            {

                Console.WriteLine(string.Format("\n{0}\n{1}\n{2}\n{3}\n", e.GetType(), e.InnerException?.GetType(), e.Message, e.StackTrace), e);
            }
#endif
        }
        #endregion
    }
}