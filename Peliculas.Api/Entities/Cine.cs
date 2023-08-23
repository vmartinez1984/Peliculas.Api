using System.ComponentModel.DataAnnotations;

namespace Peliculas.Api.Entities
{
    public class Cine
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string Nombre { get; set; }
                
        public double Latitud { get; set; }
                
        public double Longitud { get; set; }
    }
}
