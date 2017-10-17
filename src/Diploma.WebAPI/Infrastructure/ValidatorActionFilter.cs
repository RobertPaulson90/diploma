using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace Diploma.WebAPI.Infrastructure
{
    public class ValidatorActionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.ModelState.IsValid)
            {
                return;
            }

            var result = new ContentResult();
            var errors = filterContext.ModelState.ToDictionary(
                pair => pair.Key,
                pair => pair.Value.Errors.Select(x => x.ErrorMessage)
                    .ToArray());

            var content = JsonConvert.SerializeObject(
                new
                {
                    errors
                });
            result.Content = content;
            result.ContentType = "application/json";

            filterContext.HttpContext.Response.StatusCode = 422; // unprocessable entity;
            filterContext.Result = result;
        }
    }
}
