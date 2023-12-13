
//Title - Digital Restaurant Ordering System
//Author - Daniel Samson M
//Created by - 01-Feb-2023
//Last modified date - 06-03-2023
//Reviewed by - Anitha manogaran
// Reviewed date - 21-feb-2023 and 24-feb-2023
using Microsoft.SqlServer;
using Microsoft.EntityFrameworkCore;
using Digitalrestaurantorderplatform.Models;
using Digitalrestaurantorderplatform.Repository;
using Digitalrestaurantorderplatform.IRepository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<IAdminModel, AdminModel>();


builder.Services.AddDistributedMemoryCache();
//entity framework
builder.Services.AddDbContext<AppDbContext>(options=> options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSession(options =>
{
    
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

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

//session call
app.UseSession();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

