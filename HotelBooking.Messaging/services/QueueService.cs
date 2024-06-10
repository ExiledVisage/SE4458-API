using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using HotelBooking.Core.Models;
using HotelBooking.Core.Interfaces;

public class QueueService : IQueueService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private const string QueueName = "hotelReservations";

    public QueueService()
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(queue: QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
    }

    public void SendReservationMessage(ReservationMessage message)
    {
        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
        _channel.BasicPublish(exchange: "", routingKey: QueueName, basicProperties: null, body: body);
    }

    public void ReceiveReservationMessages(Action<ReservationMessage> processMessage)
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = JsonSerializer.Deserialize<ReservationMessage>(Encoding.UTF8.GetString(body));
            processMessage(message);
        };
        _channel.BasicConsume(queue: QueueName, autoAck: true, consumer: consumer);
    }
}