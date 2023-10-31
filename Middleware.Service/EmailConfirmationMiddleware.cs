using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using StarApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Middleware.Service
{
    public class EmailConfirmationMiddleware
    {
        private readonly RequestDelegate _next;

        public EmailConfirmationMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context, UserManager<ApplicationUser> userManager)
        {
            if(context.User.Identity.IsAuthenticated)
            {
                var user = await userManager.GetUserAsync(context.User);
                if(!user.EmailConfirmed)
                {
                    context.Response.Redirect("/Account/EmailNotConfirmed");
                    return;
                }
            }
            await _next(context);
        }
    }
}
