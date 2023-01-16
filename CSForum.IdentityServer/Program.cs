using CSForum.Data;
using CSForum.Data.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


var migrationsAssembly = typeof(Program).Assembly.GetName().Name;
var connectionString = builder.Configuration.GetConnectionString("MsSqlConnection");

builder.Services.AddDbForumContext(connectionString, migrationsAssembly);

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ForumContext>();

builder.Services.AddIdentityServer()
    .AddAspNetIdentity<IdentityUser>()
    .AddConfigurationStore(options =>
    {
        options.ConfigureDbContext = db
            => db.UseSqlServer(connectionString, opt => opt.MigrationsAssembly(migrationsAssembly));
    })
    .AddOperationalStore(options =>
    {
        options.ConfigureDbContext = db
            => db.UseSqlServer(connectionString, opt => opt.MigrationsAssembly(migrationsAssembly));
    })
    .AddDeveloperSigningCredential();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");


app.UseIdentityServer();

app.Run();