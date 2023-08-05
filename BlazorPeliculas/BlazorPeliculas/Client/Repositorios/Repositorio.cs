using BlazorPeliculas.Shared.Entidades;
using System.Text;
using System.Text.Json;

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
                    Lanzamiento = new DateTime(2022, 11, 11),
                    Poster = "https://th.bing.com/th/id/OIP.d-e92xz1a36-EFTBszzzKQAAAA?pid=ImgDet&rs=1"
                },
                new Pelicula
                {
                    Titulo = "Moana",
                    Lanzamiento = new DateTime(2016, 11, 23),
                    Poster = "https://th.bing.com/th/id/OIP.X1emm8zndOf3_N7deHf7OwHaLH?pid=ImgDet&rs=1"
                },
                new Pelicula
                {
                    Titulo = "Inception",
                    Lanzamiento = new DateTime(2023, 07, 31),
                    Poster = "https://th.bing.com/th/id/R.dd9827fbb0199cd20694739d54c346d8?rik=5JsQjhudqNqO9w&pid=ImgRaw&r=0"
                }
            };
        }

        private readonly HttpClient _httpClient;

        public Repositorio(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponseWrapper<object>> Post<T>(string url, T enviar)
        {
            var enviarJSON = JsonSerializer.Serialize(enviar);
            var enviarContent = new StringContent(enviarJSON, Encoding.UTF8, "application/json");
            var responseHttp = await _httpClient.PostAsync(url, enviarContent);
            return new HttpResponseWrapper<object>(null, !responseHttp.IsSuccessStatusCode, responseHttp);
        }
    }
}
