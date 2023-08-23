using System.ComponentModel.DataAnnotations;

namespace Peliculas.Api.Dtos
{
    public class CineDto: CineDtoIn
    {
        public int Id { get; set; }
    }

    public class CineDtoIn
    {
        [Required]
        public string Nombre { get; set; }

        [Required]
        public float Longitud { get; set; }

        [Required]
        public float Latitud { get; set; }
    }
}
