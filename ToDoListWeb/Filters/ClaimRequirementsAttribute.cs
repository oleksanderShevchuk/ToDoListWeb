using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace ToDoListWeb.Filters
{
    public class ClaimRequirementsAttribute : Attribute, IAsyncActionFilter
    {
        public string Claim { get; set; }
        public ClaimRequirementsAttribute(string claim) 
        { 
            Claim = claim;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var user = context.HttpContext.User;
            if (user == null || !user.HasClaim(c => c.Type == Claim && c.Value == bool.TrueString))
            {
                context.Result = new BadRequestResult();
                return;
            }
            await next();
        }
    }
}
