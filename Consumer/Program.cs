

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

// Create a connection factory

var factory = new ConnectionFactory();
factory.Uri = new Uri("Cloud");

// Create a connection and a channel

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

// Bind the queue to the exchange

// queue: "" - the name of the queue
// exchange: "example-fanout-exchange" - the name of the exchange
// routingKey: "" - the routing key
// arguments: null - no additional arguments

// random queue name
var randomQueueName = channel.QueueDeclare().QueueName;

channel.QueueBind(randomQueueName, "example-fanout-exchange", "");

// Consume the message

// queue: "example-queue" - the name of the queue
// autoAck: true - the message will be automatically acknowledged
// consumer: null - no additional consumer
// consumerTag: "" - the consumer tag
// noLocal: false - the message will be delivered to the same connection
// exclusive: false - the queue can be accessed by other connections
// arguments: null - no additional arguments

var consumer = new EventingBasicConsumer(channel);

channel.BasicConsume(randomQueueName, true, consumer);

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
