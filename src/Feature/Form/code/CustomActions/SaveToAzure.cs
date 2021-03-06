using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sitecore;
using Sitecore.Diagnostics;
using Sitecore.ExperienceForms.Models;
using Sitecore.ExperienceForms.Mvc.Models.Fields;
using Sitecore.ExperienceForms.Processing;
using Sitecore.ExperienceForms.Processing.Actions;
using Sitecore.Globalization;
using Sitecore.Mvc.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Helpers;

namespace SCHackathon.Feature.Form.CustomActions
{
    public class SaveToAzure : SubmitActionBase<FormDataModel>
    {
        public SaveToAzure(ISubmitActionData submitActionData) : base(submitActionData)
        {
        }
        /// <param name="data"></param>
        /// <param name="formSubmitContext"></param>
        /// <returns></returns>
        protected override bool Execute(FormDataModel data, FormSubmitContext formSubmitContext)
        {
            Assert.ArgumentNotNull(formSubmitContext, nameof(formSubmitContext));
            
            try
            {
                var value = string.Empty;
                var formFieldName = string.Empty;
                var dict = new Dictionary<string, string>();
                var fields = formSubmitContext.Fields.ToList();
                foreach (var item in fields)
                {
                    formFieldName = item.Name;
                    value = GetFieldValue(formSubmitContext, formFieldName);
                    dict.Add(formFieldName, value);
                }
                var formDetails = JsonConvert.SerializeObject(dict);
                var queueClient = Translate.Text("clouddictionary");
                var a=queueClient.Split(new string[] { "||" }, StringSplitOptions.None);
                var cloudQueueClient = new Azure.Storage.Queues.QueueClient(a[0],a[1]);
                cloudQueueClient.CreateIfNotExists();
                cloudQueueClient.SendMessage(formDetails);
                return true;
            }
            catch (Exception e)
            {
                Log.Error($"Error sending data to azure", e, this);
                return false;
            }
        }

        /// <summary>
        /// To get Field values
        /// </summary>
        /// <param name="formSubmitContext"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        private string GetFieldValue(FormSubmitContext formSubmitContext, string fieldName)
        {
            var fieldValue = string.Empty;
            try
            {
                var valueField = formSubmitContext.Fields.FirstOrDefault(f => f.Name.Equals(fieldName));
                var property = valueField?.GetType().GetProperty("Value");
                if (property != null)
                {
                    var postedId = property.GetValue(valueField);

                    if (valueField.GetType().Name == "DropDownListViewModel")
                    {
                        var result = ((IEnumerable)postedId).Cast<object>().ToList();
                        fieldValue = result.FirstOrDefault()?.ToString();
                    }
                    else if (valueField.GetType().Name == "ListViewModel")
                    {
                        var listField = (ListViewModel)postedId;
                        var array = listField?.Value?.ToArray();
                        if (array == null)
                        {
                            return string.Empty;
                        }
                        return String.Join(",", array);
                    }
                    else if (valueField.GetType().Name == "DateViewModel")
                    {
                        var dateField = (DateViewModel)postedId;
                        return dateField.Value.HasValue ? dateField.Value.Value.ToShortDateString() : string.Empty;
                    }
                    else if (valueField.GetType().Name == "NumberViewModel")
                    {
                        var numberField = (NumberViewModel)postedId;
                        return numberField.Value.HasValue ? numberField.Value.ToString() : string.Empty;
                    }
                    else if (valueField.GetType().Name == "CheckBoxListViewModel")
                    {
                        var checkbox = ((IEnumerable)postedId).Cast<object>().ToList();
                        var array = checkbox.ToArray();
                        return String.Join(",", array);
                    }
                    else if (valueField.GetType().Name == "TextViewModel")
                    {
                        var textField = (TextViewModel)postedId;
                        return (string)(object)textField.Text;
                    }
                    else
                    {
                        fieldValue = postedId.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error Getting FieldValue for {fieldName}", ex, this);
            }
            return fieldValue;
        }
    }
}