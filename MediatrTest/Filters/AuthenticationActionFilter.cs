using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediatrTest.Filters
{
    public class AuthenticationActionFilter : IAsyncActionFilter
    {
        string USERNAME_TYPE_CLAIM = "preferred_username";
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // execute any code before the action executes
            var user = context.HttpContext.User;

            if(context.HttpContext.User.HasClaim(x => x.Type == USERNAME_TYPE_CLAIM))
            {
                var username = context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == USERNAME_TYPE_CLAIM);
                //TODO: CHECK DATABASE to see if it exists, if it doesn't add it so that when they do perform something it will be there to link to
            }

            //action execution
            var result = await next();
            // execute any code after the action executes
        }
    }
}
