using BusSearch.Application.ServiceRegistration;
using BusSearch.Infrastructure.ServiceRegistration;
using BusSearch.WebUI.Mappings;




var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews(); 
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();



builder.Services.AddApplication();// Application service
builder.Services.AddInfrastructure(builder.Configuration); // Infrastructure service

builder.Services.AddScoped<IJourneyMappingService, JourneyMappingService>();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error"); 
}

app.UseStaticFiles();
app.UseRouting();
app.UseSession();



app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "customJourney",
    pattern: "seferler/{originId:int}-{destinationId:int}/{departureDate}",
    defaults: new { controller = "Journey", action = "Index" }
);

app.Run();