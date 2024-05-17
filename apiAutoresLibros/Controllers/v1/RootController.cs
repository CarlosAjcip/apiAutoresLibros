using apiAutoresLibros.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace apiAutoresLibros.Controllers.v1
{
    [Route("api/v1")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
    public class RootController : ControllerBase
    {
        private readonly IAuthorizationService authorizationService;

        public RootController(IAuthorizationService authorizationService)
        {
            this.authorizationService = authorizationService;
        }

        [HttpGet(Name = "ObtenerRoot")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<DatoHATEOAS>>> GeT()
        {
            var datosHateos = new List<DatoHATEOAS>();
            var esAdmin = await authorizationService.AuthorizeAsync(User, "esAdmin");

            datosHateos.Add(new DatoHATEOAS(enlace: Url.Link("ObtenerRoot", new { }), descripcion: "self", metodo: "GET"));

            datosHateos.Add(new DatoHATEOAS(enlace: Url.Link("ObtenerAutores", new { }), descripcion: "autores", metodo: "GET"));

            if (esAdmin.Succeeded)
            {
                datosHateos.Add(new DatoHATEOAS(enlace: Url.Link("CrearAutor", new { }), descripcion: "autores-crear", metodo: "POST"));

                datosHateos.Add(new DatoHATEOAS(enlace: Url.Link("CrearLibro", new { }), descripcion: "libro-crear", metodo: "POST"));

            }

            return datosHateos;
        }
    }
}
