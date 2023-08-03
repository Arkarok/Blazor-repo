using BlazorPeliculas.Shared.Entidades;

namespace BlazorPeliculas.Client.Repositorios
{
    public interface Irepositorio
    {
        List<Pelicula> ObtenerPeliculas();
    }
}
