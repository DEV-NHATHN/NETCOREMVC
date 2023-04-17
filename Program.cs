using App.Services;
using App.Extensions;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using App.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
{
   options.UseSqlServer(builder.Configuration.GetConnectionString("AppMvcConnectionString"));
});
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddTransient(typeof(ILogger<>), typeof(Logger<>));
builder.Services.Configure<RazorViewEngineOptions>(options =>
{
   options.ViewLocationFormats.Add("/Custom/View/{1}/{0}" + RazorViewEngine.ViewExtension);
});
builder.Services.AddSingleton(typeof(ProductService));
builder.Services.AddSingleton<PlanetService>();

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
app.AddStatusCodePage();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.UseEndpoints(endpoints =>
{
   endpoints.MapAreaControllerRoute(
       name: "product",
       areaName: "ProductManage",
       pattern: "/{controller}/{action=Index}/{id?}");
   endpoints.MapControllerRoute(
       name: "first",
              pattern: "{url:regex(^view.*product$)}/{id:range(1,5)}",
              defaults: new { controller = "First", action = "ViewProduct" });
   endpoints.MapControllerRoute(
              name: "default",
              pattern: "{controller=Home}/{action=Index}/{id?}");
   endpoints.MapRazorPages();
});
app.Run();
