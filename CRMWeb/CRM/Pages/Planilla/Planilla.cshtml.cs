using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos.Empleados;
using CRM.Pages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Text.Json;

namespace Web.Pages.Planilla
{
    public class PlanillaModel : PageModel
    {
        private IConfiguracion _configuracion;
        
        public IList<EmpleadoResponse> empleados { get; set; }
        [BindProperty]
        public Empleado empleadoRequest { get; set; } 


        public PlanillaModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }
        public async Task OnGet()
        {
            //aqui tengo pensado llamar 2 funciones aparte que hagan los obtener uno de planilla y otro de empleado y entonce en este onget llama a esas 2 funciones
            string endpoint = _configuracion.ObtenerMetodo("EndPointsEmpleados", "ObtenerEmpleados");
            var cliente = new HttpClient();
            var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c3VhcmlvIjoiSU5uVHF0RVV5RVNRaEVkVWhjcEdyd2xwdHZIZkhIemZ3bk1LcERZdU1TUkt1eUFaZW94bXZkZyIsInNlcnZpY2lvIjoiM2ZhODVmNjQtNTcxNy00NTYyLWIzZmMtMmM5NjNmNjZhZmE2IiwiaWRVc3VhcmlvIjoiMmZkMTU5OWEtYmJkNy00ZjhmLTkyNTgtOGQ0MTc4MWVmMmViIiwiY29ycmVvRWxlY3Ryb25pY28iOiJhZG1pbkBnbWFpbC5jb20iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiIxIiwiZXhwIjoxNzU3OTA5OTY4LCJpc3MiOiJsb2NhbGhvc3QiLCJhdWQiOiJsb2NhbGhvc3QifQ.036hcd6OzUWFrIZZQc5VAE_YBCt_hm30n7xc-IzLYAw";
            cliente.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var solicitud = new HttpRequestMessage(HttpMethod.Get,endpoint);

            var respuesta = await cliente.SendAsync(solicitud);
            respuesta.EnsureSuccessStatusCode();
            if (respuesta.StatusCode == HttpStatusCode.OK)
            {
                var resultado = await respuesta.Content.ReadAsStringAsync();
                var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                empleados = JsonSerializer.Deserialize<List<EmpleadoResponse>>(resultado, opciones);

               


            }
        }
        public async Task<IActionResult> OnPostAgregarEmpleado()
        {
            
            string endpoint = _configuracion.ObtenerMetodo("EndPointsEmpleados", "AgregarEmpleados");
            var cliente = new HttpClient();
            var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c3VhcmlvIjoiSU5uVHF0RVV5RVNRaEVkVWhjcEdyd2xwdHZIZkhIemZ3bk1LcERZdU1TUkt1eUFaZW94bXZkZyIsInNlcnZpY2lvIjoiM2ZhODVmNjQtNTcxNy00NTYyLWIzZmMtMmM5NjNmNjZhZmE2IiwiaWRVc3VhcmlvIjoiMmZkMTU5OWEtYmJkNy00ZjhmLTkyNTgtOGQ0MTc4MWVmMmViIiwiY29ycmVvRWxlY3Ryb25pY28iOiJhZG1pbkBnbWFpbC5jb20iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiIxIiwiZXhwIjoxNzU3OTA5OTY4LCJpc3MiOiJsb2NhbGhvc3QiLCJhdWQiOiJsb2NhbGhvc3QifQ.036hcd6OzUWFrIZZQc5VAE_YBCt_hm30n7xc-IzLYAw";
            cliente.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var solicitud = new HttpRequestMessage(HttpMethod.Post, endpoint);
            var respuesta = await cliente.PostAsJsonAsync(endpoint, empleadoRequest);

            if (!respuesta.IsSuccessStatusCode)
            {
                
                return RedirectToPage();
            }

           
            return RedirectToPage();
        }
        public async Task<IActionResult> OnGetFormularioEmpleadoEditar(Guid? idEmpleado)
        {
            if (idEmpleado == Guid.Empty)
                return NotFound();
            var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c3VhcmlvIjoiSU5uVHF0RVV5RVNRaEVkVWhjcEdyd2xwdHZIZkhIemZ3bk1LcERZdU1TUkt1eUFaZW94bXZkZyIsInNlcnZpY2lvIjoiM2ZhODVmNjQtNTcxNy00NTYyLWIzZmMtMmM5NjNmNjZhZmE2IiwiaWRVc3VhcmlvIjoiMmZkMTU5OWEtYmJkNy00ZjhmLTkyNTgtOGQ0MTc4MWVmMmViIiwiY29ycmVvRWxlY3Ryb25pY28iOiJhZG1pbkBnbWFpbC5jb20iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiIxIiwiZXhwIjoxNzU3OTA5OTY4LCJpc3MiOiJsb2NhbGhvc3QiLCJhdWQiOiJsb2NhbGhvc3QifQ.036hcd6OzUWFrIZZQc5VAE_YBCt_hm30n7xc-IzLYAw";
            string endpoint = _configuracion.ObtenerMetodo("EndPointsEmpleados", "ObtenerEmpleado");
            var cliente = new HttpClient();
            cliente.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var solicitud = new HttpRequestMessage(HttpMethod.Get, string.Format(endpoint, idEmpleado));

            var respuesta = await cliente.SendAsync(solicitud);
            respuesta.EnsureSuccessStatusCode();
            if (respuesta.StatusCode == HttpStatusCode.OK)
            {
                var resultado = await respuesta.Content.ReadAsStringAsync();
                var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                empleadoRequest = JsonSerializer.Deserialize<EmpleadoResponse>(resultado, opciones);
                empleadoRequest.IdEmpleado = idEmpleado;


                return Partial("_FormularioEmpleadoEditar", empleadoRequest);
            }
            else
            {
                return NotFound();
            }

        }
        public async Task<ActionResult> OnPostEditarEmpleado()
        {
            var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c3VhcmlvIjoiSU5uVHF0RVV5RVNRaEVkVWhjcEdyd2xwdHZIZkhIemZ3bk1LcERZdU1TUkt1eUFaZW94bXZkZyIsInNlcnZpY2lvIjoiM2ZhODVmNjQtNTcxNy00NTYyLWIzZmMtMmM5NjNmNjZhZmE2IiwiaWRVc3VhcmlvIjoiMmZkMTU5OWEtYmJkNy00ZjhmLTkyNTgtOGQ0MTc4MWVmMmViIiwiY29ycmVvRWxlY3Ryb25pY28iOiJhZG1pbkBnbWFpbC5jb20iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiIxIiwiZXhwIjoxNzU3OTA5OTY4LCJpc3MiOiJsb2NhbGhvc3QiLCJhdWQiOiJsb2NhbGhvc3QifQ.036hcd6OzUWFrIZZQc5VAE_YBCt_hm30n7xc-IzLYAw";

            string endpoint = _configuracion.ObtenerMetodo("EndPointsEmpleados", "EditarEmpleado");
            var cliente = new HttpClient();
            cliente.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var respuesta = await cliente.PutAsJsonAsync<Empleado>(string.Format(endpoint, empleadoRequest.IdEmpleado), empleadoRequest);
            respuesta.EnsureSuccessStatusCode();
            return new JsonResult(new { success = true });


        }
    }
}
