using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using GAP.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Users/Login";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("isadmin", policy => policy.RequireClaim(ClaimTypes.Role, "Admin"));
    options.AddPolicy("Fournisseur", policy => policy.RequireClaim(ClaimTypes.Role, "Fournisseur"));
    options.AddPolicy("RespServiceAchatPolicy", policy => policy.RequireRole("RespServiceAchat"));
    options.AddPolicy("ReceptServiceAchatPolicy", policy => policy.RequireRole("ReceptServiceAchat"));
    options.AddPolicy("RespServiceFinancePolicy", policy => policy.RequireRole("RespServiceFinance"));
    options.AddPolicy("RespServiceQualitePolicy", policy => policy.RequireRole("RespServiceQualite"));
});

builder.Services.AddSession();

// Add the DbContext to the container.
builder.Services.AddDbContext<GAPContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ??
        throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")));

var app = builder.Build();

// Create a scope to resolve the DbContext service and call InitializeData.
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<GAPContext>();
    dbContext.Database.Migrate(); // Apply any pending migrations
    dbContext.InitializeData();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication(); // UseAuthentication should be called before UseAuthorization.
app.UseAuthorization();

app.Use(async (context, next) =>
{
    if (!context.User.Identity.IsAuthenticated && context.Request.Path.Value != "/Users/Login" && context.Request.Path.Value != "/Fournisseurs/Register")
    {
        context.Response.Redirect("/Users/Login");
    }
    else if (context.User.Identity.IsAuthenticated && context.User.IsInRole("Admin") && context.Request.Path.Value == "/Users/Login")
    {
        context.Response.Redirect("/");
    }
    else
    {
        await next();
    }
});

app.UseSession();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();
