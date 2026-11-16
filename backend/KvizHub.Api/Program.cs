using KvizHub.Api.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("KvizHub")
    ?? throw new InvalidOperationException("Connection string 'KvizHub' is not configured.");

builder.Services.AddDbContext<KvizHubDbContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddHealthChecks().AddDbContextCheck<KvizHubDbContext>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseWebAssemblyDebugging();
}

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.MapControllers();
app.MapGet("/dbtest", async (KvizHubDbContext db) =>
{
    try
    {
        await db.Database.OpenConnectionAsync();
        return "OK";
    }
    catch (Exception ex)
    {
        return ex.ToString();
    }
});
app.MapHealthChecks("/health");
app.MapFallbackToFile("index.html");

app.Run();
