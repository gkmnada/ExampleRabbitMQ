

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

// Create a connection factory

var factory = new ConnectionFactory();
factory.Uri = new Uri("Cloud-Address");

// Create a connection and a channel

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

// Consume the message

// queue: "example-queue" - the name of the queue
// autoAck: true - the message will be automatically acknowledged
// consumer: null - no additional consumer

var consumer = new EventingBasicConsumer(channel);

var queueName = "example-queue-Info";

channel.BasicConsume(queueName, false, consumer);

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
