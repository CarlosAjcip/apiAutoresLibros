using apiAutoresLibros.DTOs;
using apiAutoresLibros.Entidades;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.Xml;

namespace apiAutoresLibros.Controllers.v1
{
    [Route("api/v1/autor")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
    public class AutorControllers : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IAuthorizationService authorizationService;

        public AutorControllers(ApplicationDbContext context, IMapper mapper, IAuthorizationService authorizationService)
        {
            this.context = context;
            this.mapper = mapper;
            this.authorizationService = authorizationService;
        }

        [HttpGet(Name = "ObtenerAutores")]
        //s[AllowAnonymous]
        public async Task<ColeccionRecursos<AutorDTO>> Get()
        {
            var autores = await context.Autores.ToListAsync();

            var dtos = mapper.Map<List<AutorDTO>>(autores);

            var esAdmin = await authorizationService.AuthorizeAsync(User, "esAdmin");

            dtos.ForEach(dto => GenerarEnlace(dto, esAdmin.Succeeded));

            var resultado = new ColeccionRecursos<AutorDTO> { Valores = dtos };
            resultado.Enlaces.Add(new DatoHATEOAS(enlace: Url.Link("ObtenerAutores", new { }),
                descripcion: "self", metodo: "GET"));

            if (esAdmin.Succeeded)
            {
                resultado.Enlaces.Add(new DatoHATEOAS(enlace: Url.Link("CrearAutor", new { }),
                descripcion: "crear-autor", metodo: "POST"));
            }



            return resultado;
        }

        [HttpGet("{id:int}", Name = "ObtenerAutor")]
       // [AllowAnonymous]
        public async Task<ActionResult<AutorDTOConLibros>> Get(int id)
        {
            var autor = await context.Autores
                .Include(autorDB => autorDB.AutoresLibros)
                .ThenInclude(autoreLibroDb => autoreLibroDb.Libro)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (autor is null)
            {
                return NotFound();
            }

            var dto = mapper.Map<AutorDTOConLibros>(autor);
            var esAdmin = await authorizationService.AuthorizeAsync(User, "esAdmin");
            GenerarEnlace(dto, esAdmin.Succeeded);
            return dto;
        }

        private void GenerarEnlace(AutorDTO autorDTO, bool esAdmin)
        {
            autorDTO.Enlaces.Add(new DatoHATEOAS(enlace: Url.Link("ObtenerAutor", new { id = autorDTO.Id }),
                descripcion: "self",
                metodo: "GET"));

            if (esAdmin)
            {
                autorDTO.Enlaces.Add(new DatoHATEOAS(enlace: Url.Link("ActualizarAutor", new { id = autorDTO.Id }),
               descripcion: "autor-actualizar",
               metodo: "PUT"));

                autorDTO.Enlaces.Add(new DatoHATEOAS(enlace: Url.Link("EliminarAutor", new { id = autorDTO.Id }),
                   descripcion: "self",
                   metodo: "DELETE"));
            }



        }

        [HttpGet("{nombre}", Name = "ObtenerAutorPorNombre")]
        public async Task<ActionResult<List<AutorDTO>>> Get([FromRoute] string nombre)
        {
            var autor = await context.Autores.Where(autorDB => autorDB.Nombre.Contains(nombre)).ToListAsync();
            return mapper.Map<List<AutorDTO>>(autor);
        }

        [HttpPost(Name = "CrearAutor")]
        public async Task<ActionResult> Post([FromBody] AutorCreacionDTO autorCreacionDTO)
        {
            var existeAutor = await context.Autores.AnyAsync(AutorDB => AutorDB.Nombre == autorCreacionDTO.Nombre);
            if (existeAutor)
            {
                return BadRequest($"Ya existe un autor con el nombre {autorCreacionDTO.Nombre}");
            }
            var autor = mapper.Map<Autor>(autorCreacionDTO);

            context.Add(autor);
            await context.SaveChangesAsync();
            var autorDTO = mapper.Map<AutorDTO>(autor);
            //return Ok();
            return CreatedAtRoute("ObtenerAutor", new { id = autor.Id }, autorDTO);
        }

        [HttpPut("{id:int}", Name = "ActualizarAutor")]
        public async Task<ActionResult> Put(AutorCreacionDTO autorCreacionDTO, int id)
        {
            var exite = await context.Autores.AnyAsync(x => x.Id == id);

            if (!exite)
            {
                return NotFound();
            }
            var autor = mapper.Map<Autor>(autorCreacionDTO);
            autor.Id = id;

            context.Update(autor);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}", Name = "EliminarAutor")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Autores.AnyAsync(x => x.Id == id);
            if (!existe)
            {
                return NotFound();
            }

            context.Remove(new Autor()
            {
                Id = id
            });

            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
