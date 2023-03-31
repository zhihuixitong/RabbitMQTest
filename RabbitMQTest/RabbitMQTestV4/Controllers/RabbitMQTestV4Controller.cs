using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Channels;

namespace RabbitMQTestV4.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RabbitMQTestV4Controller : ControllerBase
    {

        /// <summary>
        /// 订阅方式消费
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "Test")]
        public async Task<string> TestAsync()
        {

            _= Task.Run(async () =>
            {

                RabbitMQ.Client.IConnectionFactory connFactory = new ConnectionFactory//创建连接工厂对象
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
                        channel.BasicQos(0, 2, false);

                        //创建消费者对象
                        var consumer = new EventingBasicConsumer(channel);
                        consumer.Received +=  (model, ea) =>
                        {
                            byte[] message = ea.Body.ToArray();//接收到的消息
                            Console.WriteLine("接收到信息为:" + Encoding.UTF8.GetString(message));

                           
                        };
                        //消费者开启监听
                        channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
                        bool waitRes = true;
                        while (waitRes)
                        {
                            await Task.Delay(10000);
                        }

                    }
                }


            });

            return "运行成功";
        }


        /// <summary>
        /// 定时拉取数据消费
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "Test2")]
        public async Task<string> Test2Async()
        {

            _ = Task.Run(async () =>
            {
                RabbitMQ.Client.IConnectionFactory connFactory = new ConnectionFactory//创建连接工厂对象
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
                        channel.BasicQos(0, 2, false);

                        //创建消费者对象
                        var consumer = new EventingBasicConsumer(channel);

                        bool waitRes = true;
                        while (waitRes)
                        {
                            BasicGetResult msgResponse = channel.BasicGet(queueName, true);
                            BasicGetResult msgResponse2 = channel.BasicGet(queueName, true);
                            BasicGetResult msgResponse3 = channel.BasicGet(queueName, true);
                            if (msgResponse != null)
                            {
                                byte[] message = msgResponse.Body.ToArray();//接收到的消息
                                Console.WriteLine("接收到信息为:" + Encoding.UTF8.GetString(message));
                            }
                            if (msgResponse2 != null)
                            {
                                byte[] message = msgResponse2.Body.ToArray();//接收到的消息
                                Console.WriteLine("接收到信息为2:" + Encoding.UTF8.GetString(message));
                            }
                            if (msgResponse3 != null)
                            {
                                byte[] message = msgResponse3.Body.ToArray();//接收到的消息
                                Console.WriteLine("接收到信息为3:" + Encoding.UTF8.GetString(message));
                            }

                            await Task.Delay(1000);
                        }
                    }
                }

            });

            return "运行成功";
        }
    }
}
