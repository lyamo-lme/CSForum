using CSForum.Core.Models;
using CSForum.Data;
using CSForum.Data.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CSForum.WebUI.Data;

var builder = WebApplication.CreateBuilder(args);
var assembly = typeof(Program).Assembly.GetName().Name;
// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("MsSqlConnection");

builder.Services.AddDbContext<ForumDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDbForumContext(connectionString, assembly);

builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ForumDbContext>();

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