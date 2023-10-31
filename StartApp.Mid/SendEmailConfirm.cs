using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using StarApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StartApp.Mid
{
    public class SendEmailConfirm
    {
        private readonly RequestDelegate _next;
        private bool _middlewareExecuted = false;
        public SendEmailConfirm(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext Context, UserManager<ApplicationUser> userManager)
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                var user = await userManager.GetUserAsync(Context.User);

                if (!user.EmailConfirmed)
                {
                    // Check if the user is not already on the SendEmailConfirm page
                    if (!Context.Request.Path.StartsWithSegments("/account/SendEmailConfirm"))
                    {
                        Context.Response.Redirect("/account/SendEmailConfirm");
                        return;
                    }
                }
            }

            await _next(Context);
        }
    }
}
