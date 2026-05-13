using GroveStart.Infra;
using GroveStart.Model;
using GroveStart.Repository;
using GroveStart.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
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
builder.Services.AddControllers();

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.All;
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;
});

var app = builder.Build();

static bool IsSwaggerOrOpenApiPath(PathString path) =>
    path.StartsWithSegments("/swagger", StringComparison.OrdinalIgnoreCase)
    || path.StartsWithSegments("/openapi", StringComparison.OrdinalIgnoreCase);

// Rotas públicas para load balancer / monitoramento (sem API key nem ?secure=)
static bool IsPublicProbePath(PathString path) =>
    IsSwaggerOrOpenApiPath(path)
    || path.StartsWithSegments("/health", StringComparison.OrdinalIgnoreCase);

// Development: sempre. Demais ambientes: appsettings + variável de ambiente (ex.: EnableSwagger=true).
var enableSwagger = app.Environment.IsDevelopment()
    || app.Configuration.GetValue<bool>("EnableSwagger");

// Configure the HTTP request pipeline.
if (enableSwagger)
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
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

app.Use(async (context, next) =>
{
    if (IsPublicProbePath(context.Request.Path))
    {
        await next();
        return;
    }

    var IsAuthorized = context.Request.Headers["API_KEY_TESTE"] == "MY_API_KEY";
    if (!IsAuthorized)
    {
        context.Response.StatusCode = 403;
        await context.Response.WriteAsync("Access Denied");
        return;
    }
    context.Response.Cookies.Append("SecureCookie", "SecureData",new CookieOptions
    {
        HttpOnly = true,
        Secure = true
    });
    await next();
});

app.Use(async (context, next) =>
{
    if (IsPublicProbePath(context.Request.Path))
    {
        await next();
        return;
    }

    // Check for a query parameter to simulate HTTPS enforcement (e.g., "?secure=true")
    if (context.Request.Query["secure"] != "true")
    {
        context.Response.StatusCode = 400;
        await context.Response.WriteAsync("Simulated HTTPS Required");
        return;
    }

    await next();
});

if (app.Configuration.GetValue<bool>("UseHttpsRedirection"))
{
    app.UseHttpsRedirection();
}

app.MapHealthChecks("/health");

app.MapControllers();
app.Run();

