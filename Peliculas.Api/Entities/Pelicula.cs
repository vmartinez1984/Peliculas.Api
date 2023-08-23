using System.ComponentModel.DataAnnotations;

namespace Peliculas.Api.Entities
{
    public class Pelicula
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Titulo { get; set; }

        public bool EnCines { get; set; } = false;

        //public bool  { get; set; }
    }
}
