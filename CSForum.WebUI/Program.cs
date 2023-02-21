using System.Text.Json.Serialization;
using CSForum.Core;
using CSForum.Core.Models;
using CSForum.Core.Service;
using CSForum.Data;
using CSForum.Data.Context;
using CSForum.Services.ChatServ;
using CSForum.Services.EmailService;
using CSForum.Services.Http;
using CSForum.Services.TokenService;
using CSForum.Shared;
using CSForum.Shared.Models;
using CSForum.WebUI;
using CSForum.WebUI.Resources;
using CSForum.WebUI.Services.HttpClients;
using CSForum.WebUI.Services.Interfaces;
using CSForum.WebUI.SignalR;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
var assembly = typeof(Program).Assembly.GetName().Name;

builder.Services.AddLocalizationApp();
// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("MsSqlConnection");

//add users secrets json
builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("Secrets.json", optional: true);

//add dbcontext for forum db
builder.Services.AddDbForumContext(connectionString, assembly);
builder.Services.AddForumDbContext();

builder.Services.AddAppIdentity<User>(_ => { });

//identity 
// builder.Services.AddDefaultIdentity<User>(options =>
//     {
//         options.SignIn.RequireConfirmedAccount = true;
//     })
//  .AddEntityFrameworkStores<ForumDbContext>();


//Ioptions for api settings
builder.Services.Configure<ApiSettingConfig>(
    builder.Configuration.GetSection("DevelopmentApiSettings"));
builder.Services.Configure<IdentityServerSettings>(
    builder.Configuration.GetSection("IdentityServerSettings"));


builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddSignalR();

builder.Services.AddHttpClient();

builder.Services.AddScoped<ApiHttpClientBase>();

builder.Services.AddTransient<IChatService, ChatService>();
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddScoped<IHttpAuthorization, HttpAuthorization>();

//email service di
var password = builder.Configuration["emailPassword"];
var email = builder.Configuration["email"];
builder.Services.AddSingleton<IEmailService>(new EmailService(password, email));


builder.Services
    .AddControllersWithViews()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapHub<ChatHub>("chat");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();