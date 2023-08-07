using BlazorPeliculas.Server.Helpers;
using BlazorPeliculas.Shared.DTOs;
using BlazorPeliculas.Shared.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorPeliculas.Server.Controllers
{
    [ApiController]
    [Route("api/peliculas")]
    public class PeliculasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IAlmacenadorArchivos _almacenadorArchivos;
        private readonly string _contenedor = "peliculas";

        public PeliculasController(ApplicationDbContext context, IAlmacenadorArchivos almacenadorArchivos)
        {
            _context = context;
            _almacenadorArchivos = almacenadorArchivos;
        }

        [HttpGet]
        public async Task<ActionResult<HomePageDTO>> Get()
        {
            var limite = 6;

            var peliculasEnCartelera = await _context.Peliculas.Where(x => x.EnCartelera).Take(limite)
                .OrderByDescending(x => x.Lanzamiento).ToListAsync();

            var fechaActual = DateTime.Today;

            var proximosEstrenos = await _context.Peliculas.Where(x => x.Lanzamiento > fechaActual).Take(limite)
                .OrderBy(x => x.Lanzamiento).ToListAsync();

            var resultado = new HomePageDTO
            {
                PeliculasEnCartelera = peliculasEnCartelera,
                ProximosEstrenos = proximosEstrenos
            };

            return resultado;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<PeliculaVisualizarDTO>> Get(int id)
        {
            var pelicula = await _context.Peliculas.Where(x => x.Id == id).Include(x => x.GenerosPelicula)
                .ThenInclude(g => g.Genero).Include(x => x.PeliculasActor.OrderBy(a => a.Orden))
                .ThenInclude(a => a.Actor).FirstOrDefaultAsync();

            if(pelicula is null)
            {
                return NotFound();
            }

            var promedioVoto = 4;
            var votoUsuario = 5;

            var modelo = new PeliculaVisualizarDTO();
            modelo.Pelicula = pelicula;
            modelo.Generos = pelicula.GenerosPelicula.Select(g => g.Genero).ToList()!;
            modelo.Actores = pelicula.PeliculasActor.Select(a => new Actor
            {
                Nombre = a.Actor!.Nombre,
                Foto = a.Actor.Foto,
                Personaje = a.Personaje,
                Id = a.ActorId
            }).ToList();

            modelo.PromedioVotos = promedioVoto;
            modelo.VotoUsuario = votoUsuario;

            return modelo;
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(Pelicula pelicula)
        {
            if (!string.IsNullOrWhiteSpace(pelicula.Poster))
            {
                var poster = Convert.FromBase64String(pelicula.Poster);
                pelicula.Poster = await _almacenadorArchivos.GuardarArchivo(poster, ".jpg", _contenedor);
            }

            if(pelicula.PeliculasActor is not null)
            {
                for (int i = 0; i < pelicula.PeliculasActor.Count; i++)
                {
                    pelicula.PeliculasActor[i].Orden = i + 1;
                }
            }

            _context.Add(pelicula);
            await _context.SaveChangesAsync();
            return pelicula.Id;
        }
    }
}
