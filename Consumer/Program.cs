

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

// Create a connection factory

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://wwuqdkjr:I0JTDZq0njnYxKGoWaiPLCWi594OpmQT@sparrow.rmq.cloudamqp.com/wwuqdkjr");

// Create a connection and a channel

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

// Declare a queue

// durable: false - the queue will not survive a broker restart
// exclusive: false - the queue can be accessed by other connections
// autoDelete: false - the queue will not be deleted once the connection is closed
// arguments: null - no additional arguments

channel.QueueDeclare("example-queue", false, false, false, null);

// Consume the message

// queue: "example-queue" - the name of the queue
// autoAck: true - the message will be automatically acknowledged
// consumer: null - no additional consumer
// consumerTag: "" - the consumer tag
// noLocal: false - the message will be delivered to the same connection
// exclusive: false - the queue can be accessed by other connections
// arguments: null - no additional arguments

var consumer = new EventingBasicConsumer(channel);

channel.BasicConsume("example-queue", true, consumer);

consumer.Received += (object? sender, BasicDeliverEventArgs e) =>
{
    var body = e.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);

    Console.WriteLine("Message Received:" + message);

    // Acknowledge the message

    // deliveryTag: e.DeliveryTag - the delivery tag
    // multiple: false - acknowledge a single message
    // requeue: false - the message will not be requeued

    channel.BasicAck(e.DeliveryTag, false);
};

Console.ReadLine();
