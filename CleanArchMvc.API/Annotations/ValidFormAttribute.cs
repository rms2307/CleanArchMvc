using CleanArchMvc.API.ApiModels.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CleanArchMvc.API.Annotations
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class ValidFormAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
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
    }
}
