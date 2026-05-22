using System.Text.Json;
using GroveStart.Infra;
using GroveStart.Model;
using GroveStart.Repository;
using GroveStart.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// OpenAPI 3.x gerado em runtime em /openapi/v1.json (Microsoft.AspNetCore.OpenApi).
// Learn more: https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
builder.Services.AddHealthChecks();
DotNetEnv.Env.Load();

var connectionString = DotNetEnv.Env.GetString("ConnectionStrings__DefaultConnection");
if (string.IsNullOrWhiteSpace(connectionString))
    throw new InvalidOperationException(
        "Connection string 'DefaultConnection' não está configurada (appsettings / variáveis de ambiente).");

builder.Services.AddDbContext<ConnectionContext>(options =>
{
    options.UseNpgsql(connectionString);
    if (builder.Environment.IsDevelopment())
    {
        options.EnableDetailedErrors();
        options.EnableSensitiveDataLogging();
    }
});

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.All;
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;
});

// Repositórios específicos
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPasswordHasher<User>,PasswordHasher<User>>();
// Serviços
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IUserService, UserService>();  // Adicione esta linha

// Serviços
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    // Swagger UI apenas como cliente visual; o contrato vem do OpenAPI acima.
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", $"{app.Environment.ApplicationName} (OpenAPI v1)");
    });
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
else
{
    app.UseDeveloperExceptionPage();
}
app.UseAuthentication();
app.UseAuthorization();
app.Use(async(context,next) =>
{
    Console.WriteLine(context.Request.Path);
    await next.Invoke();
    Console.WriteLine(context.Response.StatusCode);
});

app.UseWhen(context => context.Request.Method != "GET", branch =>
{
    branch.Use(async (context, next) =>
    {
        Console.WriteLine("not get method");
        await next.Invoke();
    });
});

if (app.Configuration.GetValue<bool>("UseHttpsRedirection"))
{
    app.UseHttpsRedirection();
}

app.MapHealthChecks("/health");

app.MapControllers();
app.Run();

