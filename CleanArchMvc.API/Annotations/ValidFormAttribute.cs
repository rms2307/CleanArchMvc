using CleanArchMvc.API.ApiModels.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace CleanArchMvc.API.Annotations
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class ValidFormAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            Sanitize(context);

            if (!context.ModelState.IsValid)
            {
                var errors = GetErrors(context.ModelState);
                context.Result = new BadRequestObjectResult(new ApiResponse<List<string>>(errors));
                return;
            }

            base.OnActionExecuting(context);
        }

        public List<string> GetErrors(ModelStateDictionary modelState)
        {
            List<string> errors = new();

            foreach (var item in modelState.Values)
                errors.AddRange(item.Errors.Select(x => x.ErrorMessage));

            return errors;
        }

        private void Sanitize(ActionExecutingContext context)
        {
            foreach (var actionArgument in context.ActionArguments)
            {
                var obj = actionArgument.Value;
                Type objectType = obj.GetType();
                PropertyInfo[] properties = objectType.GetProperties();

                foreach (PropertyInfo property in properties)
                {
                    if (property.PropertyType == typeof(string))
                    {
                        object value = property.GetValue(obj);

                        HttpUtility.HtmlEncode((string)value);
                        string sanitizedInput = HttpUtility.HtmlEncode((string)value);

                        property.SetValue(obj, sanitizedInput, null);
                    }
                }
            }
        }
    }
}
