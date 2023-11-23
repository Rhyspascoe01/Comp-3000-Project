using Microsoft.EntityFrameworkCore;
using B.A.S.S.Models;

var builder = WebApplication.CreateBuilder(args);
IConfiguration configuration = builder.Configuration;
var server = configuration["DbServer"] ?? "localhost";
var user = configuration["DbUser"] ?? "SA";
var pwd = configuration["DbPwd"] ?? "C0mp2001!";
var database = configuration["DB"] ?? "Comp_3000_Database";

builder.Services.AddDbContext<RouteContexts>(options =>
options.UseSqlServer($"Server={server};Initial Catalog={database};User ID={user};Password={pwd}; TrustServerCertificate = True;"));

builder.Services.AddDbContext<StopContext>(options =>
options.UseSqlServer($"Server={server};Initial Catalog={database};User ID={user};Password={pwd}; TrustServerCertificate = True;"));

builder.Services.AddDbContext<BusContext>(options =>
options.UseSqlServer($"Server={server};Initial Catalog={database};User ID={user};Password={pwd}; TrustServerCertificate = True;"));

// Add services to the container.
builder.Services.AddControllersWithViews();

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
