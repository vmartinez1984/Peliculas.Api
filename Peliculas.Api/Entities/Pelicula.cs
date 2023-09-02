using System.ComponentModel.DataAnnotations;

namespace Peliculas.Api.Entities
{
    public class Pelicula
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(300)]
        public string Titulo { get; set; }

        public string? Resumen { get; set; }

        public string? Trailer { get; set; }

        public bool EnCines { get; set; }

        public DateTime? FechaDeLanzamiento { get; set; }

        public string? Poster { get; set; }

        public List<PeliculasActores> PeliculasActores { get; set; }

        public List<PeliculasGeneros> PeliculasGeneros { get; set; }

        public List<PeliculasCines> PeliculasCines { get; set; }
    }
}
