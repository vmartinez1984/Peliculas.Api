using System.ComponentModel.DataAnnotations;

namespace Peliculas.Api.Dtos
{
    public class CredencialesUsuarioDto
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
