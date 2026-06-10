// 1. Namespaces hamesha top par honi chahiye
using Microsoft.EntityFrameworkCore;
using HealthcareCRM.Data;

var builder = WebApplication.CreateBuilder(args);

// 2. Add services to the container.
builder.Services.AddControllersWithViews();

// 3. Database context configuration (SQLite)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();