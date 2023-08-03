using BlazorPeliculas.Shared.Entidades;

namespace BlazorPeliculas.Client.Repositorios
{
    public class Repositorio : Irepositorio
    {
        public List<Pelicula> ObtenerPeliculas()
        {
                return new List<Pelicula>()
                {
                new Pelicula
                {
                    Titulo = "Wakanda Forever",
                    FechaLanzamiento = new DateTime(2022, 11, 11)
                },
                new Pelicula
                {
                    Titulo = "Mohana",
                    FechaLanzamiento = new DateTime(2016, 11, 23)
                },
                new Pelicula
                {
                    Titulo = "Inception",
                    FechaLanzamiento = new DateTime(2023, 07, 31)
                }
            };
        }
    }
}
