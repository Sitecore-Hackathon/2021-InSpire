using Sitecore.Diagnostics;
using System;
using System.Web.Mvc;

namespace SCHackathon.Foundation.AzureExtension.Controllers
{
    public class AzureExtensionController : Controller
    {
        public ActionResult SendMessage()
        {
            ActionResult result = null;
            try
            {
                return result = View();
            }
            catch (Exception ex)
            {
                Log.Error(ex.StackTrace, ex, typeof(AzureExtensionController));
                return result;
            }
        }

    }
}