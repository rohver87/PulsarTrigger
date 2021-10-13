using Microsoft.Azure.WebJobs.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using Extensions.Pulsar.Config;
using Microsoft.Azure.WebJobs;

[assembly: WebJobsStartup(typeof(PulsarStartup))]
namespace Extensions.Pulsar.Config
{

    public class PulsarStartup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            builder.AddPulsarExtension();
        }
    }

}
