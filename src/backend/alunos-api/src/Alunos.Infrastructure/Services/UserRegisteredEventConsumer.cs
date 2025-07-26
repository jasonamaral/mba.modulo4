using Alunos.Application.EventHandlers;
using Alunos.Domain.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Alunos.Infrastructure.Services;

public class UserRegisteredEventConsumer : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<UserRegisteredEventConsumer> _logger;
    private readonly IConfiguration _configuration;
    private IConnection? _connection;
    private IModel? _channel;

    public UserRegisteredEventConsumer(
        IServiceProvider serviceProvider,
        ILogger<UserRegisteredEventConsumer> logger,
        IConfiguration configuration)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _configuration = configuration;
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQ:Host"] ?? "localhost",
                Port = int.Parse(_configuration["RabbitMQ:Port"] ?? "5672"),
                UserName = _configuration["RabbitMQ:Username"] ?? "admin",
                Password = _configuration["RabbitMQ:Password"] ?? "admin123",
                VirtualHost = _configuration["RabbitMQ:VirtualHost"] ?? "/"
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            // Declarar exchange
            var exchangeName = _configuration["RabbitMQ:ExchangeName"] ?? "plataforma.events";
            _channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Topic, durable: true);

            // Declarar fila
            var queueName = "usuario-registrado";
            _channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false);

            // Bind da fila ao exchange
            _channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: "usuario.registrado");

            _logger.LogInformation("Consumidor de eventos UserRegistered iniciado");

            await base.StartAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao inicializar consumidor de eventos");
            // Não lançar exceção para evitar que o serviço pare completamente
            // O serviço tentará reconectar na próxima execução
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            if (_channel == null)
            {
                _logger.LogError("Canal RabbitMQ não foi inicializado");
                return;
            }

            var consumer = new EventingBasicConsumer(_channel);
            
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                try
                {
                    var userRegisteredEvent = JsonConvert.DeserializeObject<UserRegisteredEvent>(message);
                    
                    if (userRegisteredEvent != null && !userRegisteredEvent.EhAdministrador)
                    {
                        using var scope = _serviceProvider.CreateScope();
                        var handler = scope.ServiceProvider.GetRequiredService<UserRegisteredEventHandler>();
                        
                        await handler.HandleAsync(userRegisteredEvent);
                    }
                    else
                    {
                        _logger.LogInformation("Evento ignorado - usuário é administrador ou evento inválido");
                    }

                    // Acknowledg do processamento
                    _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao processar evento: {Message}", message);
                    
                    // Rejeitar mensagem e não reenviar para evitar loop infinito
                    _channel.BasicNack(deliveryTag: ea.DeliveryTag, multiple: false, requeue: false);
                }
            };

            _channel.BasicConsume(queue: "usuario-registrado", autoAck: false, consumer: consumer);

            // Aguardar até que o serviço seja cancelado
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Consumidor de eventos cancelado");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado no consumidor de eventos");
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        
        _channel?.Close();
        _connection?.Close();
        
        await base.StopAsync(cancellationToken);
    }

    public override void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
        base.Dispose();
    }
} 