using BanqueTardi.ContexteDb;
using BanqueTardi.Data;
using BanqueTardi.Interfaces;
using BanqueTardi.Services;
using BanqueTardi.TardiCompte;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<BanqueTardiContexte>(options =>
options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnexion")));

builder.Services.AddScoped<IBanque, CompteCanadien>();
builder.Services.AddHttpClient<IAssuranceClientServices, AssuranceClientServicesProxy>(client => 
client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("UrlAPI")));

builder.Services.AddHttpClient<IAssuranceInteretServices, AssuranceInteretServiceProxy>(client =>
client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("UrlAPI")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Comptes/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Clients}/{action=Index}");


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<BanqueTardiContexte>();
        var tardi = services.GetRequiredService<IBanque>();
        DbInitializer.Initialize(context, tardi);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred creating the DB."); 
    }
}

app.Run();
