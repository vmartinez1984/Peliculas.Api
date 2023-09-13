using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Peliculas.Api.Contexts;
using Peliculas.Api.Dtos;
using Peliculas.Api.Entities;

namespace Peliculas.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingsController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly AppDbContext context;

        public RatingsController(UserManager<IdentityUser> userManager, AppDbContext context)
        {
            this.userManager = userManager;
            this.context = context;
        }



        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Post([FromBody] RatingDto dto)
        {
            var email = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "email").Value;
            var usuario = await userManager.FindByNameAsync(email);
            var usuarioId = usuario.Id;
            var ratingActual = await context.Rating.FirstOrDefaultAsync(x => x.PeliculaId == dto.PeliculaId && x.UsuarioId == usuarioId);
            if (ratingActual == null)
            {
                var rating = new Rating
                {
                    PeliculaId = dto.PeliculaId,
                    Puntuacion = dto.Puntuacion,
                    UsuarioId = usuarioId
                };
                context.Add(rating);
            }
            else
            {
                ratingActual.Puntuacion = dto.Puntuacion;
            }

            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}
