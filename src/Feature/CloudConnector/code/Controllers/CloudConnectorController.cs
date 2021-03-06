using Azure.Identity;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using SCHackathon.Feature.CloudConnector.Models;
using Sitecore.Diagnostics;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sitecore.Web;

namespace SCHackathon.Feature.CloudConnector.Controllers
{
    public class CloudConnectorController : Controller
    {
        [HttpGet]
        public JsonResult ConnectToAzure(AzureProperties azureProperties)
        {
            bool result = false;
            try
            {
                try
                {
                    var azQueueAcName = azureProperties.AzureAccountName;
                    var azQueueAcKey = azureProperties.SecretKey;
                    var saCred = new StorageCredentials(azQueueAcName, azQueueAcKey);
                    var saConfig = new CloudStorageAccount(saCred, true);
                    var azClient = saConfig.CreateCloudQueueClient();
                    if (azClient.BaseUri != null)
                    {
                        this.Session.Add("AzClientData", azClient);
                        result = true;
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (AuthenticationFailedException e)
                {
                    Console.WriteLine($"Authentication Failed. {e.Message}");
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex.StackTrace, ex, typeof(CloudConnectorController));
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

    }
}