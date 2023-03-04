using CSForum.Core;
using CSForum.Core.IRepositories;
using CSForum.Core.Models;
using CSForum.Core.Service;
using CSForum.Data;
using CSForum.Data.Context;
using CSForum.IdentityServer;
using CSForum.IdentityServer.Areas.Identity.Validator;
using CSForum.Infrastructure.Repository;
using CSForum.Services.EmailService;
using CSForum.Services.Http;
using CSForum.Services.TokenService;
using CSForum.Shared;
using CSForum.Shared.Models;
using CSForum.WebUI;
using CSForum.WebUI.Services.HttpClients;
using CSForum.WebUI.Services.Interfaces;
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

builder.Services.Configure<ApiSettingConfig>(
    builder.Configuration.GetSection("DevelopmentApiSettings"));
builder.Services.Configure<IdentityServerSettings>(
    builder.Configuration.GetSection("IdentityServerSettings"));
builder.Services.AddHttpClient();
builder.Services.AddTransient<ApiHttpClientBase>();
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddTransient<IHttpAuthorization, HttpAuthorization>();


builder.Services.AddCoreIdentity<User>(_ => { });

builder.Services.AddDbContext<ForumDbContext>(options =>
    options.UseSqlServer(defaultConnString,
        b => b.MigrationsAssembly(assembly)));

builder.Services.AddRepository();

builder.Services.AddIdentity<User, IdentityRole<int>>(options =>
    {
        options.User.AllowedUserNameCharacters = string.Empty;
        options.SignIn.RequireConfirmedEmail = true;
    })
    .AddEntityFrameworkStores<ForumDbContext>()
    .AddDefaultTokenProviders()
    .AddUserValidator<CustomUserValidator<User>>();

builder.Services.ConfigureApplicationCookie(config =>
{
    config.Cookie.Name = "IdentityServer.Cookie";
    config.LoginPath = "/AuthCS/Login";
    config.LogoutPath = "/AuthCS/Logout";
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
builder.Services.AddScoped<IRepositoryFactory, RepositoryFactory>();

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
app.MapRazorPages();


app.Run();