using System.Net;

namespace BlazorPeliculas.Client.Repositorios
{
    public class HttpResponseWrapper<T>
    {
        public HttpResponseWrapper(T? response, bool error, HttpResponseMessage responseMessage)
        {
            this.error = error;
            this.response = response;
            this.responseMessage = responseMessage;
        }

        public bool error { get; set; }
        public T? response { get; set; }
        public HttpResponseMessage responseMessage { get; set; }

        public async Task<string?> ObtenerMensajeError()
        {
            if(!error)
            {
                return null;
            }

            var codigoEstatus = responseMessage.StatusCode;

            if(codigoEstatus == HttpStatusCode.NotFound)
            {
                return "Recurso no encontrado";
            }else if (codigoEstatus == HttpStatusCode.BadRequest)
            {
                return await responseMessage.Content.ReadAsStringAsync();
            }else if (codigoEstatus == HttpStatusCode.Unauthorized)
            {
                return "Tienes que loguearte para hacer esto";
            }else if (codigoEstatus == HttpStatusCode.Forbidden)
            {
                return "No tienes permisos para hacer esto";
            }
            else
            {
                return "Ha ocurrido un error inesperado";
            }
        }
    }
}
