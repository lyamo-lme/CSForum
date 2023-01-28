using CSForum.Data;

var builder = WebApplication.CreateBuilder(args);

var assembly = typeof(Program).Assembly.GetName().Name;

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddAuthentication("Bearer")
    .AddIdentityServerAuthentication("Bearer", options =>
    {
        options.Authority = "https://localhost:6000";
        options.ApiName = "api";
    });

builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("Secrets.json", optional: true);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


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