using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace AzFunction
{
    public static class Function1
    {
        [FunctionName("Function1")]
        [return: TwilioSms(AccountSidSetting = "TwilioAccountSid", AuthTokenSetting = "TwilioAuthToken", From = "+15134022952")]
        public static CreateMessageOptions Run([QueueTrigger("testqueue", Connection = "AzureWebJobsStorage")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
            var queueItem = myQueueItem.Split(',');
            string telNo = "";
            foreach (var item in queueItem)
            {
                if(item.Contains("Telephone"))
                {
                    telNo = item.Split(':')[1].Trim('"');
                }
            }
            var message = new CreateMessageOptions(new PhoneNumber(telNo))
            {
                Body = $"Hello , thanks for your partcipation!"
            };
            return message;
        }
    }
}
