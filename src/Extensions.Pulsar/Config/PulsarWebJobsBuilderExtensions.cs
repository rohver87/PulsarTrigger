using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.DependencyInjection;

namespace Extensions.Pulsar.Config
{
    public static class PulsarWebJobsBuilderExtensions
    {
        public static IWebJobsBuilder AddPulsarExtension(this IWebJobsBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }


            builder.AddExtension<PulsarExtensionConfigProvider>();

            builder.Services.AddSingleton<IPulsarServiceFactory, PulsarServiceFactory>();

            return builder;
        }
    }
}
