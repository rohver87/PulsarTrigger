using System.Threading.Tasks;
using Extensions.Pulsar.Triggers;
using Microsoft.Azure.WebJobs;

namespace SampleFunction
{
    public  class Function1
    {
        public Function1()
        {
                
        }

        [FunctionName("Function1")]
        public async Task Run([PulsarTrigger(ServiceUrl = "",
            Token = "",ConsumerName = "",
            SubscriptionName = "",
            TopicName = "")] byte[] message)
        {
           
        }
    }

    public class Function2
    {
        public Function2()
        {

        }

        [FunctionName("Function2")]
        public async Task Run([PulsarTrigger(ServiceUrl = "",
            Token = "",ConsumerName = "",
            SubscriptionName = "",
            TopicName = "")] byte[] message)
        {

        }
    }
}
