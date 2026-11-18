using KvizHub.Api.Data;
using KvizHub.Api.Data.Entities;
using KvizHub.Api.Identity;
using KvizHub.Shared.Enums;
using Microsoft.AspNetCore.Authorization;
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
    .AddPasswordValidator<LetterPasswordValidator>()
    .AddClaimsPrincipalFactory<AppUserClaimsPrincipalFactory>(); ;

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

    // The API returns status codes instead of redirecting to a login page
    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return Task.CompletedTask;
    };
    options.Events.OnRedirectToAccessDenied = context =>
    {
        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        return Task.CompletedTask;
    };
});

builder.Services.AddAuthorizationBuilder()
    // Every endpoint requires a signed-in user unless it is marked with [AllowAnonymous]
    .SetFallbackPolicy(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build())
    .AddPolicy(Policies.Quizmaster, policy => policy.RequireRole(nameof(UserRole.Quizmaster)))
    .AddPolicy(Policies.TeamLeader, policy => policy.RequireRole(nameof(UserRole.TeamLeader)))
    .AddPolicy(Policies.Admin, policy => policy.RequireRole(nameof(UserRole.Admin)));

var app = builder.Build();
await AdminSeeder.SeedAsync(app.Services, app.Configuration, app.Logger);

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi().AllowAnonymous();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "KvizHub API"));
    app.UseWebAssemblyDebugging();
}

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health").AllowAnonymous();
app.MapFallbackToFile("index.html").AllowAnonymous();

app.Run();
