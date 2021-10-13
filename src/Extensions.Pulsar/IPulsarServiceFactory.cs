using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Extensions.Pulsar
{
    public interface IPulsarServiceFactory
    {
        Task<PulsarCoreClient> CreatePulsarCoreClient(string serviceUrl, string token);

        Task<PulsarCoreClient> CreatePulsarCoreClient(string serviceUrl, string issuerUrl, string audience);
    }
}
