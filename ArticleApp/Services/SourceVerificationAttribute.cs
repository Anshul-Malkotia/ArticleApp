using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace ArticleApp.Services
{
    public class SourceVerificationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var requestSource = context.HttpContext.Request.Headers["X-Request-Source"].ToString();

            if (string.IsNullOrEmpty(requestSource))
            {
                context.Result = new BadRequestObjectResult("Request source header is missing.");
                return;
            }

            var allowedSources = new[] { "trusted-app", "trusted-website" };

            if (!allowedSources.Contains(requestSource))
            {
                context.Result = new ForbidResult();
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}
