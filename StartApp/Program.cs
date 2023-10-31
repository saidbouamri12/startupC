using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StarApp.Core.Models;
using StartApp.EF.DBContext;
using StartApp.Mid;
using StartApp.Utilities;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
    ));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
builder.Services.AddScoped<IdbInitializer,DbInitializer>();
// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
//DataSeeding();
app.UseRouting();
app.UseStatusCodePagesWithReExecute("/Account/AccessDenied?code={0}");

app.UseAuthentication();
app.UseAuthorization();
//app.UseMiddleware<SendEmailConfirm>();
app.UseMiddleware<EmailConfirmationMiddleware>();
//app.UseEndpoints(Endpoint =>
//{
//    //Endpoint.MapControllerRoute(
//    //     name:"adminArea",
//    //     pattern: "{area:exists}/{controller=Student}/{action=Index}/{id?}"
//    //    );

//});
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

void DataSeeding()
{
    using (var scope = app.Services.CreateScope())
    {
        var DbInitializer = scope.ServiceProvider.GetRequiredService<IdbInitializer>();
    }
}
