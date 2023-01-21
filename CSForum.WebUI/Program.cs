using System.Reflection;
using CSForum.Core.Models;
using CSForum.Core.Service;
using CSForum.Data;
using CSForum.Data.Context;
using CSForum.Services.EmailService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CSForum.WebUI.Data;
using CSForum.Shared.Models;

var builder = WebApplication.CreateBuilder(args);
var assembly = typeof(Program).Assembly.GetName().Name;

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("MsSqlConnection");

//add users secrets json
builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("Secrets.json", optional: true);

//add dbcontext for forum db
builder.Services.AddDbForumContext(connectionString, assembly);

//identity server 
builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ForumDbContext>();


builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//Ioptions for api
builder.Services.Configure<ApiSettingConfig>(options 
    => builder.Configuration.GetSection("DevelopmentApiSettings").Bind(options));

//email service di
var password = builder.Configuration["emailPassword"];
var email = builder.Configuration["email"];
builder.Services.AddSingleton<IEmailService>(new EmailService(password, email));


builder.Services.AddControllersWithViews();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();