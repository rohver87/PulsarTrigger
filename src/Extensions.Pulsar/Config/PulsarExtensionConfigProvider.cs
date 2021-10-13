using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Extensions.Pulsar.Triggers;
using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Azure.WebJobs.Logging;
using Microsoft.Extensions.Logging;

namespace Extensions.Pulsar.Config
{
    public class PulsarExtensionConfigProvider: IExtensionConfigProvider
    {
        private readonly IPulsarServiceFactory _serviceFactory;
        private readonly ILogger _logger;
        private ConcurrentDictionary<string, PulsarCoreClient> ClientCache { get; } = new ConcurrentDictionary<string, PulsarCoreClient>();

        public PulsarExtensionConfigProvider(IPulsarServiceFactory serviceFactory, ILoggerFactory loggerFactory)
        {
            _serviceFactory = serviceFactory;
            _logger = loggerFactory?.CreateLogger(LogCategories.CreateTriggerCategory("Pulsar"));
        }

        public void Initialize(ExtensionConfigContext context)
        {
            // Add trigger first
            var triggerRule = context.AddBindingRule<PulsarTriggerAttribute>();
            triggerRule.BindToTrigger(new PulsarTriggerBindingProvider(this));
        }

        public async Task<PulsarTriggerContext> CreateContext(PulsarTriggerAttribute attribute)
        {
            var client = await _serviceFactory.CreatePulsarCoreClient(attribute.ServiceUrl, attribute.Token);
            return new PulsarTriggerContext(attribute, client);
        }
    }
}
