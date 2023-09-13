using System.ComponentModel.DataAnnotations;

namespace Peliculas.Api.Dtos
{
    public class RatingDto
    {
        public int PeliculaId { get; set; }

        [Range(1,5)]
        public int Puntuacion { get; set; }
    }
}
