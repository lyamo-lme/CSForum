using CSForum.Core.Models;
using CSForum.Data.Context;
using CSForum.IdentityServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;



var builder = WebApplication.CreateBuilder(args);

var assembly = typeof(Program).Assembly.GetName().Name;
var defaultConnString = builder.Configuration.GetConnectionString("MsSqlConnection");

SeedData.EnsureSeedData(defaultConnString);

builder.Services.AddDbContext<ForumDbContext>(options =>
    options.UseSqlServer(defaultConnString,
        b => b.MigrationsAssembly(assembly)));

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ForumDbContext>();

builder.Services.AddIdentityServer()
    .AddAspNetIdentity<User>()
    .AddConfigurationStore(options =>
    {
        options.ConfigureDbContext = b =>
            b.UseSqlServer(defaultConnString, opt => opt.MigrationsAssembly(assembly));
    })
    .AddOperationalStore(options =>
    {
        options.ConfigureDbContext = b =>
            b.UseSqlServer(defaultConnString, opt => opt.MigrationsAssembly(assembly));
    })
    .AddDeveloperSigningCredential();



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