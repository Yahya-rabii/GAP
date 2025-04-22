using GAP.ActionFilters;
using GAP.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Security.Claims;
using Xceed.Document.NET;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    // Add the NotificationActionFilter as a global filter
    options.Filters.Add<NotificationActionFilter>();
}).AddMvcOptions(options =>
{
    options.EnableEndpointRouting = false; // Disable endpoint routing for attribute routing
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Users/Login";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization(options =>
{
    // ... existing policy configurations ...
});

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "ToDo API",
        Description = "An ASP.NET Core Web API for managing ToDo items",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Example Contact",
            Url = new Uri("https://example.com/contact")
        },
        License = new OpenApiLicense
        {
            Name = "Example License",
            Url = new Uri("https://example.com/license")
        }
    });
});
builder.Services.AddControllers();
builder.Services.AddSession();

// Add the DbContext to the container.
builder.Services.AddDbContext<GAPContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ??
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


app.UseStatusCodePagesWithReExecute("/Home/AccessDenied", "?statusCode={0}");

app.Use(async (context, next) =>
{
    if (!context.User.Identity.IsAuthenticated)
    {
        if (context.Request.Path.Value == "/Users/Login" || context.Request.Path.Value == "/Suppliers/Register")
        {
            await next();
        }
        else if (context.Request.Path.Value == "/OutIndex" || context.Request.Path.Value == "/PurchaseRequestsOut" || context.Request.Path.Value == "/ServicesRequestsOut")
        {
            await next();
        }
        else
        {
            context.Response.Redirect("/OutIndex");
        }
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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(options =>
    {
        options.SerializeAsV2 = true;
    });

    app.UseSwaggerUI();
}

app.UseSession();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();
