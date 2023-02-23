
using CSForum.Core.IRepositories;
using CSForum.Core.Models;
using CSForum.Core.Service;
using CSForum.Data;
using CSForum.Infrastructure.Repository;
using CSForum.Services.ChatServ;
using CSForum.WebUI;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var assembly = typeof(Program).Assembly.GetName().Name;

// Add services to the container.
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
    );

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer",config =>
    {
        config.Authority = "https://localhost:5444/";
        config.Audience = "api";

        config.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("Secrets.json", optional: true);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.AddDbForumContext(
    builder.Configuration.GetConnectionString("MsSqlConnection"), assembly);

builder.Services.AddCoreIdentity<User>(_ => { });

builder.Services.AddTransient<IRepositoryFactory, RepositoryFactory>();

builder.Services.AddForumDbContext();
builder.Services.AddTransient<IChatService, ChatService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("apiPolicy",
        policyBuilder =>
            policyBuilder
                .AllowAnyOrigin()
                .AllowAnyMethod());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("apiPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();