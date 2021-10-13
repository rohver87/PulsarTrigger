using System;
using System.IO;
using System.Threading.Tasks;
using Pulsar.Client.Api;
using Pulsar.Client.Common;

namespace Extensions.Pulsar
{

    public class PulsarCoreClient
    {
        private PulsarClient _client = null;

        public async Task ConnectAsync(string serviceUrl, string token)
        {
            _client = await new PulsarClientBuilder()
                .ServiceUrl(serviceUrl)
                .Authentication(AuthenticationFactory.token(token))
                .BuildAsync();
        }

        public async Task ConnectAsync(string serviceUrl, string issuerUrl, string audience)
        {
            var file = Path.Combine(Directory.GetCurrentDirectory(), "private_key.json");

            var fileUri = new Uri(file);

            _client = await new PulsarClientBuilder()
                .ServiceUrl(serviceUrl)
                .BuildAsync();
        }

        public async Task<IConsumer<T>> SubscribeAsync<T>(string topicName, string subscriptionName,string consumerName)
        {
            var ret = await _client.NewConsumer()
                .Topic(topicName)
                .SubscriptionType(SubscriptionType.Shared)
                .SubscriptionInitialPosition(SubscriptionInitialPosition.Earliest)
                .SubscriptionName(subscriptionName)
                .ConsumerName(consumerName)
                .DeadLetterPolicy(new DeadLetterPolicy(3, "dlq"))
                .AckTimeout(TimeSpan.FromMinutes(5))
                .SubscribeAsync();

            return ret as IConsumer<T>;
        }

        public void DisposeClient()
        {
            _client.CloseAsync();
        }

    }
}
