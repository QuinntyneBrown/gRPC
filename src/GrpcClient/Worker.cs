using Grpc.Net.Client;
using GrpcServer;

namespace GrpcClient;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly string _serverAddress;

    public Worker(ILogger<Worker> logger, IConfiguration configuration)
    {
        _logger = logger;
        _serverAddress = configuration["GrpcServer:Address"] ?? "http://localhost:5000";
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Wait a bit for the server to start if running locally
        await Task.Delay(2000, stoppingToken);

        using var channel = GrpcChannel.ForAddress(_serverAddress);
        var client = new Greeter.GreeterClient(channel);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var reply = await client.SayHelloAsync(
                    new HelloRequest { Name = "gRPC Client Worker" },
                    cancellationToken: stoppingToken);

                _logger.LogInformation("Greeting response: {Message}", reply.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling gRPC service");
            }

            await Task.Delay(5000, stoppingToken);
        }
    }
}
