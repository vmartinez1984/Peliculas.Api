namespace Peliculas.Api.Dtos
{
    public class GeneroDto: GeneroDtoIn
    {
        public int Id { get; set; }
    }

    public class GeneroDtoIn
    {
        public string Nombre { get; set; }
    }
}
