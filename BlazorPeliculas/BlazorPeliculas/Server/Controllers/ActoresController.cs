using BlazorPeliculas.Server.Helpers;
using BlazorPeliculas.Shared.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorPeliculas.Server.Controllers
{
    [ApiController]
    [Route("api/actores")]
    public class ActoresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IAlmacenadorArchivos _almacenadorArchivos;
        private readonly string _contenedor = "personas";

        public ActoresController(ApplicationDbContext context, IAlmacenadorArchivos almacenadorArchivos)
        {
            _context = context;
            _almacenadorArchivos = almacenadorArchivos;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Actor>>> Get()
        {
            return await _context.Actores.ToListAsync();
        }

        [HttpGet("buscar/{textoBusqueda}")]
        public async Task<ActionResult<List<Actor>>> Get(string textoBusqueda)
        {
            if (string.IsNullOrWhiteSpace(textoBusqueda))
            {
                return new List<Actor>();
            }

            textoBusqueda = textoBusqueda.ToLower();

            return await _context.Actores.Where(x => x.Nombre.ToLower().Contains(textoBusqueda)).Take(5).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(Actor actor)
        {
            if (!string.IsNullOrWhiteSpace(actor.Foto))
            {
                var fotoActor = Convert.FromBase64String(actor.Foto);
                actor.Foto = await _almacenadorArchivos.GuardarArchivo(fotoActor, ".jpg", _contenedor);
            }

            _context.Add(actor);
            await _context.SaveChangesAsync();
            return actor.Id;
        }
    }
}
