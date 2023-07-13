using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using GAP.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<GAPContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("GAPContext") ?? throw new InvalidOperationException("Connection string 'GAPContext' not found.")));

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
    options.AddPolicy("isrespachat", policy => policy.RequireClaim(ClaimTypes.Role, "respachat"));
    options.AddPolicy("isrecepachat", policy => policy.RequireClaim(ClaimTypes.Role, "recepachat"));
    options.AddPolicy("isrespfin", policy => policy.RequireClaim(ClaimTypes.Role, "respfin"));
    options.AddPolicy("isrespqual", policy => policy.RequireClaim(ClaimTypes.Role, "respqual"));
});


builder.Services.AddSession();


var app = builder.Build();


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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
