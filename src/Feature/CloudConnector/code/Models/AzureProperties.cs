using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCHackathon.Feature.CloudConnector.Models
{
    public class AzureProperties
    {
        public string AzureAccountName { get; set; }
        public string SecretKey { get; set; }
        public string AzureUrl { get; set; }
        public string AzureQueueName { get; set; }
    }
}