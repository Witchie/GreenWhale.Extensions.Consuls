using Microsoft.Extensions.Hosting;
using System.Threading;

namespace GreenWhale.Extensions.Consuls.UnitTest
{
    public class TestApplicationLifetime : IHostApplicationLifetime
    {
        private readonly CancellationTokenSource _startedSource = new CancellationTokenSource();
        private readonly CancellationTokenSource _stoppingSource = new CancellationTokenSource();
        private readonly CancellationTokenSource _stoppedSource = new CancellationTokenSource();

        public CancellationToken ApplicationStarted => _startedSource.Token;

        public CancellationToken ApplicationStopping => _stoppingSource.Token;

        public CancellationToken ApplicationStopped => _stoppedSource.Token;

        public void StopApplication()
        {
            _stoppingSource.Cancel(throwOnFirstException: false);
        }

        public void Start()
        {
            _startedSource.Cancel(throwOnFirstException: false);
        }
    }
}