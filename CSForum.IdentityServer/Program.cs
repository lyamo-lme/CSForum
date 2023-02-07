using CSForum.Core.Models;
using CSForum.Data.Context;
using CSForum.IdentityServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var assembly = typeof(Program).Assembly.GetName().Name;
var defaultConnString = builder.Configuration.GetConnectionString("MsSqlConnection");


// SeedData.EnsureSeedData(defaultConnString);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ForumDbContext>(options =>
    options.UseSqlServer(defaultConnString,
        b => b.MigrationsAssembly(assembly)));

builder.Services.AddIdentity<User, IdentityRole<int>>()
    .AddEntityFrameworkStores<ForumDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(config =>
{
    config.Cookie.Name = "IdentityServer.Cookie";
    config.LoginPath = "/Auth/Login";
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

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

app.UseIdentityServer();

app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
});

app.Run();