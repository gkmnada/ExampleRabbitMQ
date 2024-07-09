

using Producer;
using RabbitMQ.Client;
using System.Text;

// Create a connection factory

var factory = new ConnectionFactory();
factory.Uri = new Uri("Cloud-Address");

// Create a connection and a channel

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

// Declare a exchange

// exchange: "" - the default exchange
// type: "" - the type of the exchange
// durable: true - the exchange will survive a broker restart
// autoDelete: false - the exchange will not be deleted when all queues have finished using it

channel.ExchangeDeclare("example-direct-exchange", ExchangeType.Direct, true, false);

Enum.GetNames(typeof(MessageType)).ToList().ForEach(x =>
{
    // Declare a queue

    // queue: queueName - the name of the queue
    // durable: true - the queue will survive a broker restart
    // exclusive: false - the queue can be accessed in other channels
    // autoDelete: false - the queue will not be deleted when all consumers have finished using it

    var queueName = "example-queue-" + x;

    channel.QueueDeclare(queueName, true, false, false);

    // Bind the queue to the exchange

    // queue: queueName - the name of the queue
    // exchange: "example-direct-exchange" - the name of the exchange
    // routingKey: queueName - the name of the queue

    channel.QueueBind(queueName, "example-direct-exchange", queueName);
});

// Create a message

Enumerable.Range(1, 50).ToList().ForEach(x =>
{
    MessageType messageType = (MessageType)new Random().Next(1, 4);

    var body = Encoding.UTF8.GetBytes("Message Type-" + messageType);

    var routingKey = "example-queue-" + messageType;

    // Publish the message

    // exchange: "example-direct-exchange" - the default exchange
    // routingKey: routingKey - the name of the queue
    // basicProperties: null - no additional properties
    // body: body - the message

    channel.BasicPublish("example-direct-exchange", routingKey, null, body);

    Console.WriteLine("Message Sent");
});

Console.ReadLine();
