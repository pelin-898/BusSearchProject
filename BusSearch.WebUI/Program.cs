using BusSearch.Application.Interfaces;
using BusSearch.Application.Services;
using System.Net.Http.Headers;
using BusSearch.Infrastructure.ServiceRegistration;
using BusSearch.Infrastructure.Configurations;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();



// Infrastructure içindeki tüm servisler DI'a eklendi
builder.Services.AddInfrastructure(builder.Configuration);


var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();


//builder.Logging.ClearProviders();
builder.Logging.AddConsole();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();