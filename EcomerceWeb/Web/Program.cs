using Abstracciones.Interfaces.Reglas;
using Autorizacion.Abstracciones.BW;
using Autorizacion.Abstracciones.DA;
using Autorizacion.BW;
using Autorizacion.DA;
using Autorizacion.DA.Repositorios;
using Autorizacion.Middleware;
using Microsoft.AspNetCore.Authentication.Cookies;
using Reglas;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Cuenta/Login";
        options.LogoutPath = "/Cuenta/CerrarSession";
        options.AccessDeniedPath = "/Cuenta/AccesoDenegado";
    });
//Configuración Autorización
builder.Services.AddTransient<IRepositorioDapper, RepositorioDapper>();
builder.Services.AddTransient<ISeguridadDA, SeguridadDA>();
builder.Services.AddTransient<IAutorizacionBW, AutorizacionBW>();
builder.Services.AddTransient<IObtenerCorreoDesdeTokenRegla, ObtenerCorreoDesdeTokenRegla>();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddScoped<IConfiguracion, Configuracion>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.MapGet("/", context =>
{
    context.Response.Redirect("/Productos");
    return Task.CompletedTask;
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseClaimsPerfiles();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
