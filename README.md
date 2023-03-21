# RabbitMQTest
RabbitMQ使用测试、.net

使用步骤
1.docker安装镜像

docker run -it --rm --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3.11-management

2.默认管理界面 外网ip:15672
http://111.230.71.xxx:15672/#/

3.发布一条消息
如代码：RabbitMQV2_Push.Program

4.接收一条消息
如代码：RabbitMQV2.Program

