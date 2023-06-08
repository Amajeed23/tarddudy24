using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FirstAttempt.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<FirstAttemptContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("FirstAttemptContext") ?? throw new InvalidOperationException("Connection string 'FirstAttemptContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession(options => { options.IdleTimeout = TimeSpan.FromMinutes(60); });
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=login}/{id?}");
app.UseSession();

app.Run();
