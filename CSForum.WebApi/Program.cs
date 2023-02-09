using CSForum.Core.IRepositories.Services;
using CSForum.Data;
using CSForum.WebApi.RepositoryServices;
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
            ValidateAudience = false
        };
    });

builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("Secrets.json", optional: true);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IPostService, PostService>();

builder.Services.AddDbForumContext(
    builder.Configuration.GetConnectionString("MsSqlConnection"), assembly);

builder.Services.AddForumDbContext();

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