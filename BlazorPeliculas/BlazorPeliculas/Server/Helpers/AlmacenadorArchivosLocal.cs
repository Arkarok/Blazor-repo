namespace BlazorPeliculas.Server.Helpers
{
    public class AlmacenadorArchivosLocal : IAlmacenadorArchivos
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IHttpContextAccessor _contextAccessor;

        public AlmacenadorArchivosLocal(IWebHostEnvironment environment, IHttpContextAccessor contextAccessor)
        {
            _environment = environment;
            _contextAccessor = contextAccessor;
        }

        public Task EliminarArchivo(string ruta, string nombreContenedor)
        {
            var nombreArchivo = Path.GetFileName(ruta);
            var directorioArchivo = Path.Combine(_environment.WebRootPath, nombreContenedor, nombreArchivo);

            if (File.Exists(directorioArchivo))
            {
                File.Delete(directorioArchivo);
            }

            return Task.CompletedTask;
        }

        public async Task<string> GuardarArchivo(byte[] contenido, string extencion, string nombreContenedor)
        {
            var nombreArchivo = $"{Guid.NewGuid()}{extencion}";
            var folder = Path.Combine(_environment.WebRootPath, nombreContenedor);

            if(!Directory.Exists(folder)) 
            {
                Directory.CreateDirectory(folder);
            }

            string rutaGuardato = Path.Combine(folder, nombreArchivo);
            await File.WriteAllBytesAsync(rutaGuardato, contenido);

            var urlActual = $"{_contextAccessor!.HttpContext!.Request.Scheme}://{_contextAccessor.HttpContext.Request.Host}";
            var rutaParaDB = Path.Combine(urlActual, nombreContenedor, nombreArchivo).Replace("\\", "/");

            return rutaParaDB;
        }
    }
}
