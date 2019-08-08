using Data;
using MediatR;
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
        const string USERNAME_TYPE_CLAIM = "preferred_username";
        const string NAME_CLAIM = "name";
        private readonly IMediator _mediator;

        public AuthenticationActionFilter(IMediator mediator)
        {
            this._mediator = mediator;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // execute any code before the action executes
            var user = context.HttpContext.User;

            if(context.HttpContext.User.HasClaim(x => x.Type == USERNAME_TYPE_CLAIM))
            {
                var userModel = new User();

                var usernameClaim = context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == USERNAME_TYPE_CLAIM);
                userModel.Email = usernameClaim?.Value;

                if(context.HttpContext.User.HasClaim(x => x.Type == NAME_CLAIM))
                {
                    var nameClaim = context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == NAME_CLAIM);
                    userModel.Name = nameClaim?.Value;
                }

                var updatedUserModel = await _mediator.Send(userModel);

                if (updatedUserModel.Id > 0)
                {
                    //action execution (only allow in this case if they have an email claim associated with the account)
                    var result = await next();
                    // execute any code after the action executes
                }
            }


        }
    }
}
