using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RabbitMQ.Client;
using System.Text;
using RabbitMQ.Client.Events;

namespace RabbitMQTestV1.Controllers
{
    /// <summary>
    /// 测试使用RabbitMQ
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RabbitMQTestController : ControllerBase
    {
        private readonly ILogger<RabbitMQTestController> _logger;
        public RabbitMQTestController(ILogger<RabbitMQTestController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 创建Mq消息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> CreateMQ()
        {
            _logger.LogInformation("创建Mq消息");

            try
            {
                ////链接RabbitMQ
                //ConnectionFactory factory = new ConnectionFactory();
                //factory.Uri = new Uri("amqp://guest:guest@111.230.71.75:5672/");
                //IConnection conn = factory.CreateConnection();

                //var exchangeName = "fanoutchange1";
                //var routingKey = "*";
                //var qName = "lhtest1";
                //var exchangeType = "fanout";//topic、fanout



                //using (var connection = conn)
                //{
                //    using (var channel = connection.CreateModel())
                //    {
                //        //设置交换器的类型
                //        channel.ExchangeDeclare(exchangeName, exchangeType);
                //        //声明一个队列，设置队列是否持久化，排他性，与自动删除
                //        channel.QueueDeclare(qName, true, false, false, null);
                //        //绑定消息队列，交换器，routingkey
                //        channel.QueueBind(qName, exchangeName, routingKey);
                //        var properties = channel.CreateBasicProperties();
                //        //队列持久化
                //        properties.Persistent = true;
                //        var body = Encoding.UTF8.GetBytes("123456");
                //        //发送信息
                //        channel.BasicPublish(exchangeName, routingKey, properties, body);


                //    }
                //}



                IConnectionFactory conFactory = new ConnectionFactory//创建连接工厂对象
                {
                    HostName = "111.230.71.75",//IP地址
                    Port = 5672,//端口号
                    UserName = "guest",//用户账号
                    Password = "guest"//用户密码
                };
                using (IConnection con = conFactory.CreateConnection())//创建连接对象
                {
                    using (IModel channel = con.CreateModel())//创建连接会话对象
                    {
                        String queueName = String.Empty;
                               queueName = "queue1";

                        //声明一个队列
                        channel.QueueDeclare(
                          queue: queueName,//消息队列名称
                          durable: false,//是否缓存
                          exclusive: false,
                          autoDelete: false,
                          arguments: null
                           );
                        while (true)
                        {
                            await Task.Delay(5000);
                            Console.WriteLine("消息内容:");
                            String message = "asd123"+DateTime.Now;
                            //消息内容
                            byte[] body = Encoding.UTF8.GetBytes(message);
                            //发送消息
                            channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
                            Console.WriteLine("成功发送消息:" + message);
                        }
                    }
                }
            }
            catch(Exception e)
            {

            }



            return "11";
        }


        /// <summary>
        /// 获取Mq消息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> GetMQ()
        {
            _logger.LogInformation("创建Mq消息");

            try
            {
                ////链接RabbitMQ
                //ConnectionFactory factory = new ConnectionFactory();
                //factory.Uri = new Uri("amqp://guest:guest@111.230.71.75:5672/");
                //IConnection conn = factory.CreateConnection();

                //var exchangeName = "fanoutchange1";
                //var routingKey = "*";
                //var qName = "lhtest1";
                //var exchangeType = "fanout";//topic、fanout

                //var channel = conn.CreateModel();
                //channel.ExchangeDeclare(exchangeName, exchangeType);
                //channel.QueueDeclare(qName, true, false, false, null);
                //channel.QueueBind(qName, exchangeName, routingKey);

                //var consumer = new EventingBasicConsumer(channel);
                //consumer.Received += (ch, ea) =>
                //{
                //    var body = ea.Body.ToArray();
                //    // copy or deserialise the payload
                //    // and process the message
                //    // ...
                //    var message = Encoding.UTF8.GetString(body);

                //    channel.BasicAck(ea.DeliveryTag, false);
                //};


                //// this consumer tag identifies the subscription
                //// when it has to be cancelled
                //string consumerTag = channel.BasicConsume(queueName, false, consumer);

                //using (var connection = conn)
                //{
                //    using (var channel = connection.CreateModel())
                //    {
                //        channel.ExchangeDeclare(exchangeName, exchangeType);
                //        channel.QueueDeclare(qName, true, false, false, null);
                //        channel.QueueBind(qName, exchangeName, routingKey);


                //        var consumer = new EventingBasicConsumer(channel);
                //        consumer.Received += (model, ea) =>
                //        {
                //            var body = ea.Body.ToArray(); // 将内存区域的内容复制到一个新的数组中
                //            var message = Encoding.UTF8.GetString(body);
                //            Console.WriteLine(" [x] Received {0}", message);
                //        };

                //    }
                //}



                IConnectionFactory connFactory = new ConnectionFactory//创建连接工厂对象
                {
                    HostName = "111.230.71.75",//IP地址
                    Port = 5672,//端口号
                    UserName = "guest",//用户账号
                    Password = "guest"//用户密码
                };
                using (IConnection conn = connFactory.CreateConnection())
                {
                    using (IModel channel = conn.CreateModel())
                    {
                        String queueName = String.Empty;
                            queueName = "queue1";
                        //声明一个队列
                        channel.QueueDeclare(
                          queue: queueName,//消息队列名称
                          durable: false,//是否缓存
                          exclusive: false,
                          autoDelete: false,
                          arguments: null
                           );
                        //创建消费者对象
                        var consumer = new EventingBasicConsumer(channel);
                        consumer.Received += (model, ea) =>
                        {
                            byte[] message = ea.Body.ToArray();//接收到的消息
                            Console.WriteLine("接收到信息为:" + Encoding.UTF8.GetString(message));
                        };
                        //消费者开启监听
                        channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
                        Console.ReadKey();
                    }
                }


            }
            catch (Exception e)
            {

            }



            return "11";
        }


    }
}
