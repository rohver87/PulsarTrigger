using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Newtonsoft.Json;
using Pulsar.Client.Common;

namespace Extensions.Pulsar.Triggers
{
    public class PulsarValueProvider: IValueProvider
    {
        private object _value;
        public PulsarValueProvider(object value,Type parameterType)
        {
            _value = value;
            Type = parameterType;
        }

        public async Task<object> GetValueAsync()
        {
            await Task.CompletedTask;
            return _value;
        }

        public string ToInvokeString()
        {
            return JsonConvert.SerializeObject(_value);
        }

        public Type Type { get; }
    }
}
