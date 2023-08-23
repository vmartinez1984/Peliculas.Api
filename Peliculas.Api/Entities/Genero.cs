using System.ComponentModel.DataAnnotations;

namespace Peliculas.Api.Entities
{
    public class Genero
    {
        public int Id { get; set; }


        [StringLength(maximumLength: 50)]
        public string Nombre { get; set; }
    }
}