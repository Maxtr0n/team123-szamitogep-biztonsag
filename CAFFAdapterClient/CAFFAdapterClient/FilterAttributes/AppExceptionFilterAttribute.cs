using CAFFAdapterClient.Framework;
using CAFFAdapterClient.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace CAFFAdapterClient.FilterAttributes
{
    public class AppExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override async Task OnExceptionAsync(ExceptionContext context)
        {
            var exception = context.Exception;

            if (exception is DataNotFoundException)
            {
                context.ExceptionHandled = true;
                context.Result = new NoContentResult();
            }
            else if (exception is BusinessLogicException)
            {
                context.ExceptionHandled = true;
                context.Result = new BadRequestObjectResult(new
                {
                    Error = GlobalConfigurations.IsDevelopment ? exception.Message : "An unknown error occurred."
                });
            }
            else
            {
                context.ExceptionHandled = true;
                context.Result = new ObjectResult(new
                {
                    Error = GlobalConfigurations.IsDevelopment ? exception.Message : "An unknown error occurred."
                })
                {
                    StatusCode = 500
                };
            }
        }
    }
}