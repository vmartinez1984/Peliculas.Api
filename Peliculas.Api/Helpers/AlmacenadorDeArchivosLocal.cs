namespace Peliculas.Api.Helpers
{
    public class AlmacenadorDeArchivosLocal : IAlmacenadorDeArchivos
    {
        private readonly IWebHostEnvironment _environment;
        private readonly HttpContextAccessor _httpContextAccessor;

        public AlmacenadorDeArchivosLocal(
            IWebHostEnvironment environment, 
            HttpContextAccessor httpContextAccessor
        )
        {
            this._environment = environment;
            this._httpContextAccessor = httpContextAccessor;
        }

        public Task Borrar(string ruta, string contenedor)
        {
            var nombreDelArchivo = Path.GetFileName(ruta);
            var directoriDelArchivo = Path.Combine(_environment.WebRootPath, contenedor, nombreDelArchivo);
            if(File.Exists(directoriDelArchivo))
            {
                File.Delete(directoriDelArchivo);
            }
             return Task.CompletedTask;
        }

        public async Task<string> EditarArchivo(string ruta, string contenedor, IFormFile archivo)
        {
            await Borrar(ruta, contenedor);
            return await Guardar(contenedor, archivo);
        }

        public async Task<string> Guardar(string contenedor, IFormFile archivo)
        {
            var extension = Path.GetExtension(archivo.FileName);
            var nombreDeArchivo = $"{Guid.NewGuid()}{extension}";
            string folder = Path.Combine(_environment.WebRootPath, contenedor);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            string ruta = Path.Combine(folder, nombreDeArchivo);
            using (var memoryStream = new MemoryStream())
            {
                await archivo.CopyToAsync(memoryStream);
                var contenido = memoryStream.ToArray();
                await File.WriteAllBytesAsync(ruta, contenido);
            }
            var urlActual = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
            var rutaPAraDb = Path.Combine(urlActual,contenedor, nombreDeArchivo).Replace("\\", "/");

            return rutaPAraDb;
        }
    }
}
