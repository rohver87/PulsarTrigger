using System;
using System.IO;
using Extensions.Pulsar.Config;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pulsar.Client.Api;
using PulsarStartup = SampleFunction.PulsarStartup;

[assembly: FunctionsStartup(typeof(PulsarStartup))]
namespace SampleFunction
{
    public class PulsarStartup : IWebJobsStartup, IWebJobsConfigurationStartup
    {
        public IConfiguration Configuration { get; private set; }

        public void Configure(IWebJobsBuilder builder)
        {
            builder.AddPulsarExtension();
            ConfigureServices(builder.Services);
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton((src) =>
            {
                var token = Configuration.GetSection("Pulsar:Token").Value;

                var serviceUrl = Configuration.GetSection("Pulsar:ServiceUrl").Value;

                return new PulsarClientBuilder()
                    .ServiceUrl(serviceUrl)
                    .Authentication(AuthenticationFactory.token(token))
                    .BuildAsync().GetAwaiter().GetResult();
            });
        }

        public void Configure(WebJobsBuilderContext context, IWebJobsConfigurationBuilder builder)
        {
            var env = Environment.GetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT", EnvironmentVariableTarget.Process);

            var config = builder.ConfigurationBuilder
                .AddJsonFile(Path.Combine(context.ApplicationRootPath, "appsettings.json"), optional: true, reloadOnChange: false)
                .AddJsonFile(Path.Combine(context.ApplicationRootPath, $"appsettings.{env}.json"),
                    optional: true, reloadOnChange: false)
                .AddEnvironmentVariables()
                .Build();

            Configuration = config;
        }
    }
}
