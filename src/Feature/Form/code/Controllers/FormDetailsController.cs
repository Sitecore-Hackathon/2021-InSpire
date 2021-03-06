using Newtonsoft.Json;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCHackathon.Feature.Form.Controllers
{
    public class FormDetailsController : Controller
    {
        public JsonResult SendToAzure()
        {
            try
            {
                var queryString = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                string jsonData = QueryStringToJSONData(queryString);

                return Json(new { data = jsonData, result = "true" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.StackTrace, ex, typeof(FormDetailsController));
            }

            return Json(new { data = "test", result = "false" }, JsonRequestBehavior.AllowGet);
        }
        public string QueryStringToJSONData(NameValueCollection queryString)
        {
            return JsonConvert.SerializeObject(queryString.Cast<string>().ToDictionary(k => k, v => queryString[v]));
        }

    }
}