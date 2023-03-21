using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQV2_Push
{
    class Program
    {
        /// <summary>
        /// 推送消息
        /// </summary>
        /// <param name="args"></param>
        static async Task Main(string[] args)
        {
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
                      
                        String message = queueName+":" + DateTime.Now;
                        //消息内容
                        byte[] body = Encoding.UTF8.GetBytes(message);
                        //发送消息
                        channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
                        Console.WriteLine("成功发送消息:" + message);
                    }
                }
            }
        }
    }
}
