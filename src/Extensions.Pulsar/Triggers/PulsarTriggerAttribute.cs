using System;
using Microsoft.Azure.WebJobs.Description;
using Pulsar.Client.Common;

namespace Extensions.Pulsar.Triggers
{
    [AttributeUsage(AttributeTargets.Parameter)]
    [Binding]
    public sealed class PulsarTriggerAttribute: Attribute
    {
        public PulsarTriggerAttribute()
        {
                
        }

        [AutoResolve]
        public string ServiceUrl { get; set; }
        [AutoResolve]
        public string SubscriptionName { get; set; }
        [AutoResolve]
        public string TopicName { get; set; }
        [AutoResolve]
        public string Token { get; set; }

        public string ConsumerName { get; set; }

        public string Audience { get; set; }

        public string IssuerUrl { get; set; }

        //public SubscriptionMode SubscriptionMode { get; set; }

        //public SubscriptionInitialPosition SubscriptionInitialPosition { get; set; }

    }
}
