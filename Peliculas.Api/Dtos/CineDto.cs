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
        [StringLength(75)]
        public string Nombre { get; set; }

        [Required]
        [Range(-180,180)]
        public double Longitud { get; set; }

        [Required]
        [Range(-180,180)]
        public double Latitud { get; set; }
    }
}
