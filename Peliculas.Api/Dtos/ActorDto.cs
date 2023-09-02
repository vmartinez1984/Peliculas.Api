using System.ComponentModel.DataAnnotations;

namespace Peliculas.Api.Dtos
{
    public class ActorDto : ActorBaseDto
    {
        public int Id { get; set; }

        public string Foto { get; set; }
    }

    public class ActorBaseDto
    {
        [StringLength(255)]
        public string Nombre { get; set; }

        public DateTime? FechaDeNacimiento { get; set; }

        public string Biografia { get; set; }        
    }

    public class ActorDtoIn: ActorBaseDto
    {     
        public IFormFile? Foto { get; set; }
    }
}
