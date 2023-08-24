using System.ComponentModel.DataAnnotations;

namespace Peliculas.Api.Dtos
{
    public class GeneroDto: GeneroDtoIn
    {
        public int Id { get; set; }
    }

    public class GeneroDtoIn
    {
        [StringLength(50)]
        public string Nombre { get; set; }
    }
}
