using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
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


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();

