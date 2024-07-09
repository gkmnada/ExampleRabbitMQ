

using RabbitMQ.Client;
using System.Text;

// Create a connection factory

var factory = new ConnectionFactory();
factory.Uri = new Uri("Cloud");

// Create a connection and a channel

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

// Declare a exchange

// exchange: "" - the default exchange
// type: "" - the type of the exchange
// durable: true - the exchange will survive a broker restart
// autoDelete: false - the exchange will not be deleted when all queues have finished using it

channel.ExchangeDeclare("example-fanout-exchange", ExchangeType.Fanout, true, false);

// Create a message

var body = Encoding.UTF8.GetBytes("Hello World");

// Publish the message

// exchange: "" - the default exchange
// routingKey: "example-queue" - the name of the queue
// basicProperties: null - no additional properties
// body: body - the message

channel.BasicPublish("example-fanout-exchange", "", null, body);

Console.WriteLine("Message Sent");

// Close the connection

// connection.Close();

Console.ReadLine();
