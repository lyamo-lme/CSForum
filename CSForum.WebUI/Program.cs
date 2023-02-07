using CSForum.Core.IHttpClients;
using CSForum.Core.Models;
using CSForum.Core.Service;
using CSForum.Data;
using CSForum.Data.Context;
using CSForum.Services.EmailService;
using CSForum.Services.HttpClients;
using CSForum.Shared.Models;
using CSForum.WebUI;
using CSForum.WebUI.Resources;
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


builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


builder.Services.AddHttpClient<IForumClient, ApiHttpClientBase>();

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
if (app.Environment.IsDevelopment()){}
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();