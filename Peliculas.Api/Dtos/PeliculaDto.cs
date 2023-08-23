using System.ComponentModel.DataAnnotations;

namespace Peliculas.Api.Dtos
{
    public class PeliculaDto: PeliculaBaseDto
    {
        public int Id { get; set; }

        public string Poster { get; set; }
    }

    public class PeliculaBaseDto
    {
        [MaxLength(100)]
        public string Titulo { get; set; }

        public bool EnCines { get; set; } = false;

        public DateTime FechaDeLanzamiento { get; set; }

        [MaxLength(1000)]
        public string Trailer { get; set; }

        [MaxLength(1000)]
        public string Resumen { get; set; }
    }

    public class PeliculaDtoIn//: PeliculaBaseDto
    {
        [MaxLength(100)]
        public string Titulo { get; set; }

        public bool EnCines { get; set; } = false;

        public DateTime FechaDeLanzamiento { get; set; }

        [StringLength(1000)]
        public string? Trailer { get; set; }

        [StringLength(1000)]
        public string? Resumen { get; set; }
        public IFormFile Poster { get; set; }
    }

}
