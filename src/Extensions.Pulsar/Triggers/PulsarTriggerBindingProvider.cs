using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Extensions.Pulsar.Config;
using Microsoft.Azure.WebJobs.Host.Triggers;
using Pulsar.Client.Common;

namespace Extensions.Pulsar.Triggers
{
    internal class PulsarTriggerBindingProvider : ITriggerBindingProvider
    {
        private readonly PulsarExtensionConfigProvider _pulsarExtensionConfigProvider;

        public PulsarTriggerBindingProvider(PulsarExtensionConfigProvider pulsarExtensionConfigProvider)
        {
            _pulsarExtensionConfigProvider = pulsarExtensionConfigProvider;
        }

        public async Task<ITriggerBinding> TryCreateAsync(TriggerBindingProviderContext context)
        {
            var parameter = context.Parameter;
            var attribute = parameter.GetCustomAttribute<PulsarTriggerAttribute>(false);

            if (attribute == null)
            {
                return null;
            }
            //if (parameter.ParameterType != typeof(Message<>)) throw new InvalidOperationException("Invalid parameter type");

            var pulsarContext = await _pulsarExtensionConfigProvider.CreateContext(attribute);
            var valueType = parameter.ParameterType;
            var genericCreateBindingStrategy = this.GetType().GetMethod(nameof(CreatePulsarTriggerBinding), BindingFlags.Instance | BindingFlags.NonPublic).MakeGenericMethod(valueType);
            return (ITriggerBinding)genericCreateBindingStrategy.Invoke(this, new object[] { pulsarContext });
        }

        private ITriggerBinding CreatePulsarTriggerBinding<TValue>(PulsarTriggerContext pulsarContext)
        {
            return new PulsarTriggerBinding<TValue>(pulsarContext);
        }
    }
}
