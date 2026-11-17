using KvizHub.Api.Data;
using KvizHub.Api.Data.Entities;
using KvizHub.Api.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("KvizHub")
    ?? throw new InvalidOperationException("Connection string 'KvizHub' is not configured.");

builder.Services.AddDbContext<KvizHubDbContext>(options => options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention());
builder.Services.AddHealthChecks().AddDbContextCheck<KvizHubDbContext>();

builder.Services.AddIdentity<AppUser, IdentityRole<int>>(options =>
    {
        options.User.RequireUniqueEmail = true;
        options.Password.RequiredLength = 8;
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredUniqueChars = 1;
    })
    .AddEntityFrameworkStores<KvizHubDbContext>()
    .AddErrorDescriber<SlovenianIdentityErrorDescriber>()
    .AddPasswordValidator<LetterPasswordValidator>();

var app = builder.Build();
await AdminSeeder.SeedAsync(app.Services, app.Configuration, app.Logger);

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "KvizHub API"));
    app.UseWebAssemblyDebugging();
}

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");
app.MapFallbackToFile("index.html");

app.Run();
