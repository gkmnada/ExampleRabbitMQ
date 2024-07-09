

using RabbitMQ.Client;
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

// Create a message

var body = Encoding.UTF8.GetBytes("Hello World");

// Publish the message

// exchange: "" - the default exchange
// routingKey: "example-queue" - the name of the queue
// basicProperties: null - no additional properties
// body: body - the message

channel.BasicPublish("", "example-queue", null, body);

Console.WriteLine("Message Sent");

// Close the connection

// connection.Close();

Console.ReadLine();
