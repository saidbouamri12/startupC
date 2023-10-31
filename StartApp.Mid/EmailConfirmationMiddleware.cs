using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using StarApp.Core.Models;

namespace StartApp.Mid
{

        public class EmailConfirmationMiddleware
        {
            private readonly RequestDelegate _next;
            private bool _middlewareExecuted = false;
        public EmailConfirmationMiddleware(RequestDelegate next)
            {
                _next = next;
            }
            public async Task Invoke(HttpContext Context, UserManager<ApplicationUser> userManager)
            {
           
                
                if (Context.User.Identity.IsAuthenticated == true)
                {
                    
                    var user1 = await userManager.GetUserAsync(Context.User);
                    if (user1.EmailConfirmed == false)
                    {
                    //if (!Context.Request.Path.StartsWithSegments("/account/SendEmailConfirm"))
                    //{
                    //    Context.Response.Redirect("/account/SendEmailConfirm");
                    //    return;
                    //}
                    if (!Context.Request.Path.StartsWithSegments("/Account/EmailNotConfirmed"))
                    {
                        Context.Response.Redirect("/Account/EmailNotConfirmed");
                        return;
                    }
                }
                    
                }
                
            
            
            await _next(Context);
        }
            
        }


    
}
