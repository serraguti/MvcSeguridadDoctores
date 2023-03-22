using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MvcSeguridadDoctores.Data;
using MvcSeguridadDoctores.Policies;
using MvcSeguridadDoctores.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

//INCLUIMOS LAS POLITICAS DE ACCESO
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("PERMISOSELEVADOS",
        policy =>
        policy.RequireRole("PSIQUIATRIA", "CARDIOLOGIA"));
    options.AddPolicy("AdminOnly",
        policy => policy.RequireClaim("Administrador"));
    options.AddPolicy("SoloRicos",
        policy => policy.Requirements.Add(new OverSalarioRequirement()));
});

// Add services to the container.
string connectionString =
    builder.Configuration.GetConnectionString("SqlHospital");
builder.Services.AddTransient<RepositoryHospital>();
builder.Services.AddDbContext<HospitalContext>
    (options => options.UseSqlServer(connectionString));

//SEGURIDAD
builder.Services.AddAuthentication(options =>
{
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(
    CookieAuthenticationDefaults.AuthenticationScheme, 
    config =>
    {
        config.AccessDeniedPath = "/Managed/ErrorAcceso";
    });


builder.Services.AddControllersWithViews(options => 
options.EnableEndpointRouting = false)
    .AddSessionStateTempDataProvider();

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
app.UseAuthentication();
app.UseAuthorization();

app.UseSession();
app.UseMvc(route =>
{
    route.MapRoute(
        name: "default",
        template: "{controller=Home}/{action=Index}/{id?}");
});


app.Run();
