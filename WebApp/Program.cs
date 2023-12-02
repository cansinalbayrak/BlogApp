using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Data.Entities;
using WebApp.Managers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("YasarDB");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddTransient<IEmailSender, MailManager>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    return new MailManager(
        configuration["EmailSender:Host"],
        configuration.GetValue<int>("EmailSender:Port"),
        configuration.GetValue<bool>("EmailSender:EnableSSL"),
        configuration["EmailSender:UserName"],
        configuration["EmailSender:Password"]
    );
});

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();


builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = new PathString("/User/Login");
    options.LogoutPath = new PathString("/User/Logout");
    options.AccessDeniedPath = new PathString("/Home/AccessDenined");


    options.Cookie = new CookieBuilder()
    {
        Name = "kukki",
        HttpOnly = true,
        SameSite = SameSiteMode.Lax,
        SecurePolicy = CookieSecurePolicy.None
    };

    options.SlidingExpiration = true;
    options.ExpireTimeSpan = TimeSpan.FromDays(1);

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
