
using CSForum.Data;
var builder = WebApplication.CreateBuilder(args);

var assembly = typeof(Program).Assembly.GetName().Name;
// Add services to the container.

builder.Services.AddControllers();


builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("Secrets.json");

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddDbForumContext(
    builder.Configuration.GetConnectionString("MsSqlConnection"), assembly);

builder.Services.AddRepositories();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
