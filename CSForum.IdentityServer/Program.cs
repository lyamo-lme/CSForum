using CSForum.Core.Models;
using CSForum.Core.Service;
using CSForum.Data.Context;
using CSForum.IdentityServer;
using CSForum.IdentityServer.Identity;
using CSForum.Services.EmailService;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var assembly = typeof(Program).Assembly.GetName().Name;
var defaultConnString = builder.Configuration.GetConnectionString("MsSqlConnection");

builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("Secrets.json", optional: true);
// SeedData.EnsureSeedData(defaultConnString);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ForumDbContext>(options =>
    options.UseSqlServer(defaultConnString,
        b => b.MigrationsAssembly(assembly)));

builder.Services.AddIdentity<User, IdentityRole<int>>(options =>
    {
        options.User.AllowedUserNameCharacters = string.Empty;
    })
    .AddEntityFrameworkStores<ForumDbContext>()
    .AddDefaultTokenProviders()
    .AddUserValidator<CustomUserValidator<User>>();

builder.Services.ConfigureApplicationCookie(config =>
{
    config.Cookie.Name = "IdentityServer.Cookie";
    config.LoginPath = "/Auth/Login";
    config.LogoutPath = "/Auth/Logout";
});

builder.Services.AddIdentityServer()
    .AddInMemoryClients(Config.Clients)
    .AddInMemoryApiResources(Config.ApiResources)
    .AddInMemoryApiScopes(Config.ApiScopes)
    .AddInMemoryIdentityResources(Config.IdentityResources)
    .AddAspNetIdentity<User>()
    // .AddConfigurationStore(options =>
    // {
    //     options.ConfigureDbContext = b =>
    //         b.UseSqlServer(defaultConnString, opt => opt.MigrationsAssembly(assembly));
    // })
    // .AddOperationalStore(options =>
    // {
    //     options.ConfigureDbContext = b =>
    //         b.UseSqlServer(defaultConnString, opt => opt.MigrationsAssembly(assembly));
    // })
    .AddDeveloperSigningCredential();

builder.Services.AddAuthentication()
    .AddGoogle( options =>
    {
        options.ClientId = builder.Configuration["googleClientId"];
        options.ClientSecret = builder.Configuration["googlePassword"];
    });

var password = builder.Configuration["emailPassword"];
var email = builder.Configuration["email"];
builder.Services.AddSingleton<IEmailService>(new EmailService(password, email));

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

app.UseIdentityServer();
app.UseAuthorization();



app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
});

app.Run();