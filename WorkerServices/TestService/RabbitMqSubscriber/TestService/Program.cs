using RabbitMqSubscriberWorker;
using TestService.Services;

IHost host = Host.CreateDefaultBuilder(args)
    .UseWindowsService()
    .ConfigureServices(services =>
    {
        //services.AddHostedService<TimerWorkerService>();
        services.AddHostedService<RabbitMqSubscriber>();
    })
    .Build();

await host.RunAsync();