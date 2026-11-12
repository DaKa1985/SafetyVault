using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SafetyVault.Api.Data;
using SafetyVault.Api.Entities;
using SafetyVault.Api.Services;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("SafetyVaultDb")
 ?? throw new InvalidOperationException("Connection string 'SafetyVaultDb' is not configured.");

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddScoped<IChecklistService, ChecklistService>();
builder.Services.AddScoped<IItemService, ItemService>();
builder.Services.AddScoped<IStockBatchService, StockBatchService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddSingleton<ITokenService, TokenService>();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString)
    .UseSeeding((context, _) =>
    {
        var checklists = context.Set<Checklist>();
        if (!checklists.Any())
        {
            checklists.AddRange(SeedData.CreateChecklists());
            context.SaveChanges();
        }
    })
    .UseAsyncSeeding(async (context, _, cancellationToken) =>
    {
        var checklists = context.Set<Checklist>();
        if (!await checklists.AnyAsync(cancellationToken))
        {
            checklists.AddRange(SeedData.CreateChecklists());
            await context.SaveChangesAsync(cancellationToken);
        }
    }
    )
    );

var jwtSettings = builder.Configuration.GetSection("Jwt");

var jwtIssuer = jwtSettings["Issuer"]
    ?? throw new InvalidOperationException("Konfiguration 'Jwt:Issuer' fehlt.");
var jwtAudience = jwtSettings["Audience"]
    ?? throw new InvalidOperationException("Konfiguration 'Jwt:Audience' fehlt.");
var jwtSigningKey = jwtSettings["SigningKey"]
    ?? throw new InvalidOperationException("Konfiguration 'Jwt:SigningKey' fehlt.");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.MapInboundClaims = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtIssuer,
            ValidateAudience = true,
            ValidAudience = jwtAudience,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(jwtSigningKey))
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

