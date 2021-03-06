using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Queue;
using System.Configuration;

namespace SCHackathon.Foundation.AzureExtension.Services
{
    public class AzureExtension
    {
        public async System.Threading.Tasks.Task azStorageQueueConAsync(string body, string AzureQueueName)
        {
            var azQueueAcName = ConfigurationManager.AppSettings["AzureStorageAccountName"];
            var azQueueAcKey = ConfigurationManager.AppSettings["AzureStorageAccountkey"];
            var saCred = new StorageCredentials(azQueueAcName, azQueueAcKey);
            var saConfig = new CloudStorageAccount(saCred, true);
            var azClient = saConfig.CreateCloudQueueClient();
            var azQueue = azClient.GetQueueReference(AzureQueueName);
            await azQueue.CreateIfNotExistsAsync();
            var msgToSend = new CloudQueueMessage(body);
            await azQueue.AddMessageAsync(msgToSend);
        }
    }
}