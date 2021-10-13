using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Extensions.Pulsar
{
    public class PulsarServiceFactory:IPulsarServiceFactory
    {
        public async Task<PulsarCoreClient> CreatePulsarCoreClient(string serviceUrl, string token)
        {
            var client = new PulsarCoreClient();
            await client.ConnectAsync(serviceUrl,token);
            return client;
        }

        public async Task<PulsarCoreClient> CreatePulsarCoreClient(string serviceUrl, string issuerUrl,string audience)
        {
            var client = new PulsarCoreClient();
            await client.ConnectAsync(serviceUrl, issuerUrl, audience);
            return client;
        }
    }
}
