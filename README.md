# RabbitMQTest
RabbitMQ使用测试、.net

官网：https://www.rabbitmq.com/dotnet-api-guide.html#major-api-elements

# 功能：
1.队列接收

2.发布订阅

3.手动清空某个请求队列

4.程序层面可以控制删除消息



# 使用步骤
1.docker安装镜像

docker run -it --rm --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3.11-management

2.默认管理界面 外网ip:15672
http://xxx.xxx.xx.xx:15672/#/

3.发布一条消息
如代码：RabbitMQV2_Push.Program

4.接收一条消息
如代码：RabbitMQV2.Program

5.删除队列消息）
可以显式删除队列或交换：
channel.QueueDelete("queue-name", false, false);

可以清除队列（删除其所有消息）：
channel.QueuePurge("queue-name");



