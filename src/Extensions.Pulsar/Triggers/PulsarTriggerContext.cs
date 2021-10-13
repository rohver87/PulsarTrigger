using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.WebJobs.Host.Triggers;

namespace Extensions.Pulsar.Triggers
{
    public class PulsarTriggerContext
    {

        public PulsarTriggerAttribute TriggerAttribute;

        public PulsarCoreClient Client;

        public PulsarTriggerContext(PulsarTriggerAttribute attribute, PulsarCoreClient client)
        {
            this.TriggerAttribute = attribute;
            this.Client = client;
        }
    }
}
