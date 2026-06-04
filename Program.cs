using ChicaizaSuite.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddScoped<ChicaizaSuite.Api.Services.GroqChatbotService>();
builder.Services.AddScoped<ChicaizaSuite.Api.Services.ZebraPrintService>();
builder.Services.AddScoped<ChicaizaSuite.Api.Services.SriXmlSignatureService>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ChicaizaSuite.Api.Services.ITenantService, ChicaizaSuite.Api.Services.TenantService>();

// CORS - Permitir conexiones desde el Frontend (Vercel o Local)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Configure PostgreSQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? "Host=localhost;Database=chicaizasuite;Username=postgres;Password=solocali";

// Helper para limpiar la cadena de conexión
connectionString = connectionString.Trim();
if (connectionString.StartsWith("postgres://") || connectionString.StartsWith("postgresql://"))
{
    var uri = new Uri(connectionString);
    var userInfo = uri.UserInfo.Split(':', 2);
    connectionString = $"Host={uri.Host};Port={(uri.Port > 0 ? uri.Port : 5432)};Database={uri.AbsolutePath.TrimStart('/')};Username={userInfo[0]};Password={(userInfo.Length > 1 ? userInfo[1] : "")};SSL Mode=Require;Trust Server Certificate=true;";
}

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// Configure JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "ChicaizaSuite",
            ValidAudience = "ChicaizaSuite",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SuperSecretKeyForJWT1234567890!!"))
        };
    });

var app = builder.Build();

// Habilitado temporalmente en producción para ver el error exacto
app.UseDeveloperExceptionPage();
app.UseSwagger();
app.UseSwaggerUI();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        await DbSeeder.SeedAsync(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Un error ocurrió durante el seeding de la base de datos.");
    }
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
