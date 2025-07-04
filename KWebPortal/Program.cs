using BAL.Interfaces;
using BAL.Repo;
using DAL;
using DAL.Enities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DataConnectionString");
builder.Services.AddDbContext<KWebContext > (x => x.UseSqlServer(connectionString));
builder.Services.AddScoped<IUser, UserRepo>();
builder.Services.AddScoped<ISubCategory, SubCategoryRepo>();
builder.Services.AddScoped<IRole, RoleRepo>();
builder.Services.AddScoped<IStoreType, StoreTypeRepo>();
builder.Services.AddScoped<IProduct, ProductRepo>();
builder.Services.AddScoped<IStore, StoreRepo>();
builder.Services.AddScoped<IPasswordHash, HashPaswordRepo>();
builder.Services.AddScoped<ICategory, CategoryRepo>();
// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(option =>
    {
        option.ExpireTimeSpan = TimeSpan.FromMinutes(60 * 1);
        option.LoginPath = "/Account/LogIn";
        option.AccessDeniedPath = "/Account/AccessDenied";
    });

builder.Services.AddSession(option =>
{
    option.IdleTimeout = TimeSpan.FromMinutes(60);
    option.Cookie.HttpOnly = true;
    option.Cookie.IsEssential = true;
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
app.UseRouting();

app.UseSession();

app.UseAuthentication();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}")
    .WithStaticAssets();


app.Run();
