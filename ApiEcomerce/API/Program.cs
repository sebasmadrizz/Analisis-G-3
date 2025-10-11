using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Interfaces.Reglas;
using Abstracciones.Interfaces.Servicios;
using Abstracciones.Modelos;
using Autorizacion.Abstracciones;
using Autorizacion.Abstracciones.BW;
using Autorizacion.Abstracciones.DA;
using Autorizacion.BW;
using Autorizacion.DA;
using Autorizacion.Middleware;
using DA;
using DA.Repositorio;
using DA.Repositorios;
using Flujo;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Reglas;
using Servicios;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHostedService<LimpiezaCarritos>();
var tokenConfiguration = builder.Configuration.GetSection("Token").Get<TokenConfiguracion>();
var jwtIssuer = tokenConfiguration.Issuer;
var jwtAudience = tokenConfiguration.Audience;
var jwtKey = tokenConfiguration.Key;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
    options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    }



    );

builder.Services.AddSingleton<ICorreoServicio>(sp =>
    new CorreoServicio(
        smtpServer: "smtp.gmail.com",
        smtpPort: 587,
        usuario: "madrizsebas71@gmail.com",
        password: "nkkt rrel baqa hiyh",
        from: "madrizsebas71@gmail.com"
    )
);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpClient();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<Abstracciones.Interfaces.DA.IRepositorioDapper, RepositorioDapper>();
builder.Services.AddScoped<IProductosFlujo, ProductosFlujo>();
builder.Services.AddScoped<IProductosDA, ProductosDA>();
builder.Services.AddScoped<IProveedorFlujo, ProveedorFlujo>();
builder.Services.AddScoped<IProveedorDA, ProveedorDA>();
builder.Services.AddScoped<IUsuarioDA, UsuarioDA>();
builder.Services.AddScoped<IUsuarioFlujo, UsuarioFlujo>();
builder.Services.AddScoped<IAutenticacionFlujo, AutenticacionFlujo>();
builder.Services.AddScoped<IAutenticacionReglas, AutenticacionReglas>();
builder.Services.AddScoped<IDocumentoRegla, DocumentoRegla>();
builder.Services.AddScoped<IRepositorioSistemaArchivos, RepositorioSistemaArchivos>();
builder.Services.AddScoped<ICarritoProductoFlujo, CarritoProductoFlujo>();
builder.Services.AddScoped<ICarritoProductoDA, CarritoProductoDA>();
builder.Services.AddScoped<ICategoriasFlujo, CategoriasFlujo>();
builder.Services.AddScoped<ICategoriasReglas, CategoriasReglas>();
builder.Services.AddScoped<ICategoriasDA, CategoriasDA>();
builder.Services.AddScoped<ICarritoFlujo, CarritoFlujo>();
builder.Services.AddScoped<ICarritoDA, CarritoDA>();
builder.Services.AddScoped<ICarritoProductoReglas, CarritoProductoReglas>();
builder.Services.AddSingleton<LevenshteinService>();
builder.Services.AddScoped<IExportarArchivosReglas, ExportarArchivosReglas>();
builder.Services.AddScoped<IGenerarResetTokenRegla, GenerarResetTokenRegla>();
builder.Services.AddScoped<IResetPasswordFlujo, ResetPasswordFlujo>();
builder.Services.AddScoped<IRepositorioResetPassword, RepositorioResetPassword>();
builder.Services.AddScoped<IResetPasswordDA, ResetPasswordDA>();
builder.Services.AddScoped<IProductosRegla, ProductosRegla>();

builder.Services.AddTransient<IAutorizacionBW, Autorizacion.BW.AutorizacionBW>();
builder.Services.AddTransient<Autorizacion.Abstracciones.DA.ISeguridadDA, Autorizacion.DA.SeguridadDA>();
builder.Services.AddTransient<Autorizacion.Abstracciones.DA.IRepositorioDapper, Autorizacion.DA.Repositorios.RepositorioDapper>();

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("https://localhost:7117")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseClaimsPerfiles();
app.UseCors(MyAllowSpecificOrigins);
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseStaticFiles();


app.Run();
