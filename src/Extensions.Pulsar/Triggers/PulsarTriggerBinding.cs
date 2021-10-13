using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Listeners;
using Microsoft.Azure.WebJobs.Host.Protocols;
using Microsoft.Azure.WebJobs.Host.Triggers;
using Pulsar.Client.Common;

namespace Extensions.Pulsar.Triggers
{
    public class PulsarTriggerBinding<TValue>: ITriggerBinding
    {
        private readonly PulsarTriggerContext _context;

        public PulsarTriggerBinding(PulsarTriggerContext context)
        {
            _context = context;
        }

        public Task<ITriggerData> BindAsync(object value, ValueBindingContext context)
        {
            var bindingData = new Dictionary<string, object>();
            var triggerData = new TriggerData(new PulsarValueProvider(value, TriggerValueType), bindingData);

            return Task.FromResult<ITriggerData>(triggerData);
        }

        public Task<IListener> CreateListenerAsync(ListenerFactoryContext context)
        {
            var executor = context.Executor;

            var listener = new PulsarListener<TValue>(executor, _context);

            return Task.FromResult<IListener>(listener);
        }

        public ParameterDescriptor ToParameterDescriptor()
        {
            return new TriggerParameterDescriptor
            {
                Name = "Pulsar trigger",
                DisplayHints = new ParameterDisplayHints
                {
                    Prompt = "Pulsar",
                    Description = "Pulsar message trigger"
                }
            };
        }

        public Type TriggerValueType => typeof(TValue);

        public IReadOnlyDictionary<string, Type> BindingDataContract => new Dictionary<string, Type>();
    }
}
