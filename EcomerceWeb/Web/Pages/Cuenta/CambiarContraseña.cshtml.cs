using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos.Seguridad;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reglas;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;

namespace Web.Pages.Cuenta
{
    [Authorize(Roles = "2")]
    public class CambiarContrasenaModel : PageModel
    {
        [BindProperty]
        public LoginRequest loginInfo { get; set; } = default!;
        [BindProperty]
        public CambiarContrasenaInfo cambiarContrasena { get; set; } = default!;
        [BindProperty]
        public Token token { get; set; } = default!;
        private IConfiguracion _configuracion;

        public CambiarContrasenaModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }
        public async Task<IActionResult> OnPost()
        {
            
                var Hash = Autenticacion.GenerarHash(loginInfo.Password);
                loginInfo.PasswordHash = Autenticacion.ObtenerHash(Hash);
               
                loginInfo.CorreoElectronico= HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Email).FirstOrDefault().Value;
                loginInfo.NombreUsuario = loginInfo.CorreoElectronico.Split("@")[0];

            string endpoint = _configuracion.ObtenerMetodo("ApiEndPointsSeguridad", "Login");
                var client = new HttpClient();
                var respuesta = await client.PostAsJsonAsync<LoginBase>(endpoint, new LoginBase { NombreUsuario = loginInfo.NombreUsuario, CorreoElectronico = loginInfo.CorreoElectronico, PasswordHash = loginInfo.PasswordHash });
                respuesta.EnsureSuccessStatusCode();
                var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                token = JsonSerializer.Deserialize<Token>(respuesta.Content.ReadAsStringAsync().Result, opciones);
                if (token.ValidacionExitosa)
                {
                    JwtSecurityToken? jwtToken = Autenticacion.leerToken(token.AccessToken);
                    var claims = Autenticacion.GenerarClaims(jwtToken, token.AccessToken);
                    await establecerAutenticacion(claims);
                    var hashNueva = Autenticacion.GenerarHash(cambiarContrasena.NuevaContraseña);
                    cambiarContrasena.NuevaContraseñaHash= Autenticacion.ObtenerHash(hashNueva);

                string endpointCambiarContrasena = _configuracion.ObtenerMetodo("ApiEndPointsSeguridad", "CambiarContrasena");
                var clientCambiarContrasena = new HttpClient();
                clientCambiarContrasena.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.User.Claims.Where(c => c.Type == "Token").FirstOrDefault().Value);
                var respuestaCambiarContrasena = await clientCambiarContrasena.PostAsJsonAsync<CambiarContrasenaRequest>(endpointCambiarContrasena, new CambiarContrasenaRequest { ContraseñaActual= loginInfo.PasswordHash , NuevaContraseña= cambiarContrasena.NuevaContraseñaHash, Correo= loginInfo.CorreoElectronico });
                respuestaCambiarContrasena.EnsureSuccessStatusCode();

            }
                else
                {
                    return Page();
                }
            
            
            return RedirectToPage("../Cuenta/verCuenta");
        }

        private async Task establecerAutenticacion(List<Claim> claims)
        {
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(principal);
        }
    }
}
