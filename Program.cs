using App.Services;
using App.Extensions;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using App.Models;
using App.Data;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
var Configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
{
   options.UseSqlServer(builder.Configuration.GetConnectionString("AppMvcConnectionString"));
});
builder.Services.AddControllersWithViews();

builder.Services.AddOptions();
var mailsetting = Configuration.GetSection("MailSettings");
builder.Services.Configure<MailSettings>(mailsetting);
builder.Services.AddSingleton<IEmailSender, SendMailService>();

builder.Services.AddRazorPages();
builder.Services.AddTransient(typeof(ILogger<>), typeof(Logger<>));
builder.Services.Configure<RazorViewEngineOptions>(options =>
{
   options.ViewLocationFormats.Add("/Custom/View/{1}/{0}" + RazorViewEngine.ViewExtension);
});
builder.Services.AddSingleton(typeof(ProductService));
builder.Services.AddSingleton<PlanetService>();
builder.Services.AddAuthorization(options =>
{
   options.AddPolicy("ViewManageMenu", builder =>
   {
      builder.RequireAuthenticatedUser();
      builder.RequireRole(RoleName.Administrator);
      // builder.RequireClaim("permission", "manage.admin");
   });
});

// Dang ky Identity
builder.Services.AddIdentity<AppUser, IdentityRole>()
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();

// Truy cập IdentityOptions
builder.Services.Configure<IdentityOptions>(options =>
{
   // Thiết lập về Password
   options.Password.RequireDigit = false; // Không bắt phải có số
   options.Password.RequireLowercase = false; // Không bắt phải có chữ thường
   options.Password.RequireNonAlphanumeric = false; // Không bắt ký tự đặc biệt
   options.Password.RequireUppercase = false; // Không bắt buộc chữ in
   options.Password.RequiredLength = 3; // Số ký tự tối thiểu của password
   options.Password.RequiredUniqueChars = 1; // Số ký tự riêng biệt

   // Cấu hình Lockout - khóa user
   options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // Khóa 5 phút
   options.Lockout.MaxFailedAccessAttempts = 3; // Thất bại 3 lầ thì khóa
   options.Lockout.AllowedForNewUsers = true;

   // Cấu hình về User.
   options.User.AllowedUserNameCharacters = // các ký tự đặt tên user
       "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
   options.User.RequireUniqueEmail = true;  // Email là duy nhất


   // Cấu hình đăng nhập.
   options.SignIn.RequireConfirmedEmail = true;            // Cấu hình xác thực địa chỉ email (email phải tồn tại)
   options.SignIn.RequireConfirmedPhoneNumber = false;     // Xác thực số điện thoại
   options.SignIn.RequireConfirmedAccount = true;

});

builder.Services.ConfigureApplicationCookie(options =>
{
   options.LoginPath = "/login/";
   options.LogoutPath = "/logout/";
   options.AccessDeniedPath = "/khongduoctruycap.html";
});

builder.Services.AddAuthentication()
        .AddGoogle(options =>
        {
           var gconfig = Configuration.GetSection("Authentication:Google");
           options.ClientId = gconfig["ClientId"];
           options.ClientSecret = gconfig["ClientSecret"];
           // https://localhost:5001/signin-google
           options.CallbackPath = "/dang-nhap-tu-google";
        })
        .AddFacebook(options =>
        {
           var fconfig = Configuration.GetSection("Authentication:Facebook");
           options.AppId = fconfig["AppId"];
           options.AppSecret = fconfig["AppSecret"];
           options.CallbackPath = "/dang-nhap-tu-facebook";
        })
        // .AddTwitter()
        // .AddMicrosoftAccount()
        ;

builder.Services.AddSingleton<IdentityErrorDescriber, AppIdentityErrorDescriber>();

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
