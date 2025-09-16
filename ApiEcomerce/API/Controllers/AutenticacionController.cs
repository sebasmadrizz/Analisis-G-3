﻿using Abstracciones.Interfaces.API;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AutenticacionController : ControllerBase, IAutenticacionController
    {
        private IAutenticacionFlujo _autenticacionFlujo;

        public AutenticacionController(IAutenticacionFlujo autenticacionFlujo)
        {
            _autenticacionFlujo = autenticacionFlujo;
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> PostAsync([FromBody] Login login)
        {
            return Ok(await _autenticacionFlujo.LoginAsync(login));
        }

    }
}
