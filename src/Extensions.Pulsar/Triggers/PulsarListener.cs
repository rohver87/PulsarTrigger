using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host.Executors;
using Microsoft.Azure.WebJobs.Host.Listeners;
using Pulsar.Client.Api;

namespace Extensions.Pulsar.Triggers
{
    internal sealed class PulsarListener<TValue>: IListener
    {
        private readonly ITriggeredFunctionExecutor _executor;
        private readonly PulsarTriggerContext _context;
        private IConsumer<TValue> _consumer;
        private bool _disposed;
        private bool _started;

        public PulsarListener(ITriggeredFunctionExecutor executor, PulsarTriggerContext context)
        {
            _executor = executor;
            _context = context;
        }
        public void Dispose()
        {

        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            if (_started)
            {
                throw new InvalidOperationException("The listener has already been started.");
            }
            
            _consumer = await _context.Client.SubscribeAsync<TValue>(_context.TriggerAttribute.TopicName,
                _context.TriggerAttribute.SubscriptionName,_context.TriggerAttribute.ConsumerName);

            var thread = new Thread(ProcessSubscription)
            {
                IsBackground = true
            };

            thread.Start(cancellationToken);

            _started = true;
        }

        private async void ProcessSubscription(object parameter)
        {
            var cancellationToken = (CancellationToken)parameter;

            while (!cancellationToken.IsCancellationRequested)
            {
                var message = await _consumer.ReceiveAsync(cancellationToken);

                var result = await _executor.TryExecuteAsync(new TriggeredFunctionData() { TriggerValue = message.GetValue() }, cancellationToken);


                if (result.Succeeded)
                {
                    await  _consumer.AcknowledgeAsync(message.MessageId);
                }
                else
                {
                    await _consumer.NegativeAcknowledge(message.MessageId);
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            if (!_started)
            {
                throw new InvalidOperationException("The listener has not yet been started or has already been stopped");
            }

            _consumer.DisposeAsync();
            _context.Client.DisposeClient();

            _started = false;
            _disposed = true;
            return Task.CompletedTask;
        }

        public void Cancel()
        {
            StopAsync(CancellationToken.None).Wait();
        }

        private void ThrowIfDisposed()
        {
            //if (_disposed)
            //{
            //    throw new ObjectDisposedException(null);
            //}
        }
    }
}
