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
using Sitecore.Data.Items;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Sitecore.Data;
using Sitecore.Publishing;

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
                    var queueClient = new QueueClient(azureProperties.AzureAccountName, azureProperties.AzureQueueName);
                    queueClient.CreateIfNotExists();
                    if (queueClient.Exists())
                    {
                        result = true;
                        SetSitecoreCloudDictionary(azureProperties.AzureAccountName, azureProperties.AzureQueueName);
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

        private bool SetSitecoreCloudDictionary(string accountName, string queuename)
        {
            bool isSuccess = false;
            Item newItem = null;
            var masterDB = Sitecore.Configuration.Factory.GetDatabase("master");

            var parentItem = masterDB.GetItem("/sitecore/system/Dictionary");

            var template = masterDB.GetTemplate(new Sitecore.Data.ID("{6D1CD897-1936-4A3A-A511-289A94C2A7B1}"));

            using (new Sitecore.SecurityModel.SecurityDisabler())
            {
                try
                {

                    var getexistingItem = masterDB.GetItem("/sitecore/system/Dictionary/clouddictionary");
                    newItem = getexistingItem == null ? parentItem.Add("clouddictionary", template) : getexistingItem;
                    if (newItem != null)
                    {
                        newItem.Editing.BeginEdit();
                        newItem["Key"] = newItem.Name;
                        newItem["Phrase"] = accountName + "||" + queuename;
                        newItem.Editing.EndEdit();
                        isSuccess = true;
                        PublishItem(Database.GetDatabase("master"), Database.GetDatabase("web"), parentItem);
                        return isSuccess;
                    }
                    return isSuccess;
                }
                catch (Exception ex)
                {
                    isSuccess = false;
                    newItem.Editing.CancelEdit();
                    return isSuccess;
                }
            }
        }


        public void PublishItem(Database dbMaster, Database dbWeb, Item iParent)
        {
            try
            {
                PublishOptions po = new PublishOptions(dbMaster, dbWeb, PublishMode.SingleItem, Sitecore.Context.Language, DateTime.Now);
                po.RootItem = iParent;
                po.Deep = true; // Publishing subitems

                (new Publisher(po)).Publish();
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Exception publishing items from custom pipeline! : " + ex, this);
            }
        }

    }
}