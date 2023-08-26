namespace Peliculas.Api
{
    public interface IAlmacenadorDeArchivos
    {       
        Task Borrar(string ruta, string contenedor);

        Task<string> EditarArchivo(string ruta, string contenedor, IFormFile archivo);

        Task<string> Guardar(string contenedor, IFormFile archivo);
    }
}
