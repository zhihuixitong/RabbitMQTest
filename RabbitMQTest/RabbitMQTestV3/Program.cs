// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

Console.WriteLine("Hello, World!");

//测试多个事件同时消费Mq消息

await Task.Run(() =>
{
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
            consumer.Received += (model, ea) =>
            {
                byte[] message = ea.Body.ToArray();//接收到的消息
                Console.WriteLine("接收到信息为2:" + Encoding.UTF8.GetString(message));
            };
            //消费者开启监听
            channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
            Console.ReadKey();
        }
    }
});

