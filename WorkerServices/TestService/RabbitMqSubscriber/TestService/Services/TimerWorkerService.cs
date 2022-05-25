using System.Reflection;

namespace TestService.Services
{
    internal class TimerWorkerService : IHostedService, IDisposable
    {
        private readonly ILogger<TimerWorkerService> _logger;

        private Timer? _timer { get; set; }

        public TimerWorkerService(ILogger<TimerWorkerService> logger)
        {
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{MethodBase.GetCurrentMethod()?.Name} method called");

            _timer = new Timer(OnTimer, cancellationToken, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{MethodBase.GetCurrentMethod()?.Name} method called");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        private void OnTimer(object? state)
        {        
            _logger.LogInformation($"{MethodBase.GetCurrentMethod()?.Name} event called");
        }
    }
}