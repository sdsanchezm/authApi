using AuthApi.Domain.Interfaces;
using AuthApi.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AuthApi.Helpers
{
    public class ValidateApplicationAttribute : ActionFilterAttribute
    {


        public async override void OnActionExecuting(ActionExecutingContext context)
        {

            var headers = context.HttpContext.Request.Headers;
            if (headers != null)
            {
                var header = headers.FirstOrDefault(m => m.Key == "appId").Value;

                Guid guid;
                if (!Guid.TryParse(header[0], out guid))
                {
                    context.ModelState.AddModelError("Error", "Error in appId, please verify");

                    context.Result = new ObjectResult(context.ModelState)
                    {
                        Value = context.ModelState,
                        StatusCode = StatusCodes.Status400BadRequest
                    };
                }
            }

            base.OnActionExecuting(context);
        }
    }
}
