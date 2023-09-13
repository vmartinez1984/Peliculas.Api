using AutoMapper;
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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
    public class PeliculasController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAlmacenadorDeArchivos _almacenadorDeArchivos;
        private readonly UserManager<IdentityUser> _userManager;
        private string _contenedor = "peliculas";

        public PeliculasController(AppDbContext context, IMapper mapper, IAlmacenadorDeArchivos almacenadorDeArchivos, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _almacenadorDeArchivos = almacenadorDeArchivos;
            this._userManager = userManager;
        }

        // GET: api/Peliculas
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<LandingPageDto>> Get()
        {
            var top = 6;
            var hoy = DateTime.Today;

            var proximosEstrenos = await _context.Pelicula
                .Where(x => x.FechaDeLanzamiento > hoy)
                .OrderBy(x => x.FechaDeLanzamiento)
                .Take(top)
                .ToListAsync();

            var peliculasEnCines = await _context.Pelicula
                .Where(x => x.EnCines)
                .OrderBy(x => x.FechaDeLanzamiento)
                .Take(top)
                .ToListAsync();

            return new LandingPageDto
            {
                PeliculasEnCines = _mapper.Map<List<PeliculaDto>>(peliculasEnCines),
                PeliculasProximosEstrenos = _mapper.Map<List<PeliculaDto>>(proximosEstrenos)
            };
        }

        [HttpGet("PutGet/{id:int}")]
        public async Task<ActionResult<PeliculaPutGetDto>> PutGet(int id)
        {
            var peliculaActionREsult = await GetPelicula(id);
            if (peliculaActionREsult == null) { return NotFound(); }

            var pelicula = peliculaActionREsult.Value;
            var generosSeleccionadosIds = pelicula.Generos.Select(x => x.Id).ToList();
            var generosNoSeleccionados = await _context.Genero.Where(x => !generosSeleccionadosIds.Contains(x.Id)).ToListAsync();
            var cinesSeleccionadosIds = pelicula.Cines.Select(x => x.Id).ToList();
            var cinessNoSeleccionados = await _context.Cine.Where(x => !cinesSeleccionadosIds.Contains(x.Id)).ToListAsync();
            //var actoresSeleccionadosIds = pelicula.Actores.Select(x => x.Id).ToList();
            //var actoresNoSeleccionados = await _context.Actor.Where(x => !actoresSeleccionadosIds.Contains(x.Id)).ToListAsync();
            var generosNoSeleccionadosDto = _mapper.Map<List<GeneroDto>>(generosNoSeleccionados);
            var cinesNoSelecciondaosDto = _mapper.Map<List<CineDto>>(cinessNoSeleccionados);

            var respuesta = new PeliculaPutGetDto
            {
                Pelicula = pelicula,
                GenerosSeleccionados = pelicula.Generos,
                GenerosNoSeleccionados = generosNoSeleccionadosDto,
                CinesSeleccionados = pelicula.Cines,
                CinesNoSeleccionados = cinesNoSelecciondaosDto,
                Actores = pelicula.Actores
            };

            return respuesta;
        }

        // GET: api/Peliculas/5
        [HttpGet("{peliculaId:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<PeliculaDto>> GetPelicula(int peliculaId)
        {
            if (_context.Pelicula == null)
            {
                return NotFound();
            }
            var pelicula = await _context.Pelicula
                .Include(x => x.PeliculasGeneros)
                    .ThenInclude(x => x.Genero)
                .Include(x => x.PeliculasActores)
                    .ThenInclude(x => x.Actor)
                .Include(x => x.PeliculasCines)
                     .ThenInclude(x => x.Cine)
                .FirstOrDefaultAsync(x => x.Id == peliculaId);
            var promedioVoto = 0.0;
            var usuarioVoto = 0;


            if (pelicula == null)
            {
                return NotFound();
            }
            if (await _context.Rating.AnyAsync(x => x.PeliculaId == pelicula.Id))
            {
                promedioVoto = await _context.Rating.Where(x => x.PeliculaId == peliculaId).AverageAsync(x => x.Puntuacion);
                if (HttpContext.User.Identity.IsAuthenticated)
                {
                    var email = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "email").Value;
                    var usuario = await _userManager.FindByNameAsync(email);
                    var rating = await _context.Rating.FirstOrDefaultAsync(x=> x.UsuarioId == usuario.Id && x.PeliculaId == peliculaId);
                    if(rating != null)
                    {
                        usuarioVoto = rating.Puntuacion;
                    }
                }
            }
            var dto = _mapper.Map<PeliculaDto>(pelicula);
            dto.Actores = dto.Actores.OrderBy(x => x.Orden).ToList();
            dto.VotoUsuario = usuarioVoto;
            dto.PromedioVoto = promedioVoto;

            return dto;
        }

        // PUT: api/Peliculas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPelicula(int id, [FromForm] PeliculaDtoIn peliculaDtoIn)
        {
            var pelicula = await _context.Pelicula
               .Include(x => x.PeliculasActores)
               .Include(x => x.PeliculasGeneros)
               .Include(x => x.PeliculasCines)
               .FirstOrDefaultAsync(x => x.Id == id);
            if (pelicula == null) { return NotFound(); }

            pelicula = _mapper.Map(peliculaDtoIn, pelicula);
            if (peliculaDtoIn.Poster != null)
            {
                await _almacenadorDeArchivos.EditarArchivo(pelicula.Poster, _contenedor, peliculaDtoIn.Poster);
            }
            EscribirOrdenDeLosActores(pelicula);
            _context.Entry(pelicula).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Peliculas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult> PostPelicula([FromForm] PeliculaDtoIn peliculaDtoIn)
        {
            Pelicula pelicula;

            pelicula = _mapper.Map<Pelicula>(peliculaDtoIn);
            if (peliculaDtoIn.Poster != null)
            {
                pelicula.Poster = await _almacenadorDeArchivos.Guardar(_contenedor, peliculaDtoIn.Poster);
            }
            EscribirOrdenDeLosActores(pelicula);
            _context.Pelicula.Add(pelicula);
            await _context.SaveChangesAsync();

            return Created("/Peliculas/" + pelicula.Id, new { id = pelicula.Id });
        }

        private void EscribirOrdenDeLosActores(Pelicula pelicula)
        {
            if (pelicula.PeliculasActores != null)
            {
                for (int i = 0; i < pelicula.PeliculasActores.Count; i++)
                {
                    pelicula.PeliculasActores[i].Orden = i;
                }
            }
        }

        // DELETE: api/Peliculas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePelicula(int id)
        {
            if (_context.Pelicula == null)
            {
                return NotFound();
            }
            var pelicula = await _context.Pelicula.FindAsync(id);
            if (pelicula == null)
            {
                return NotFound();
            }

            _context.Pelicula.Remove(pelicula);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PeliculaExists(int id)
        {
            return (_context.Pelicula?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
