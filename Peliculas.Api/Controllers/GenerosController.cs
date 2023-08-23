using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Peliculas.Api.Contexts;
using Peliculas.Api.Dtos;
using Peliculas.Api.Entities;

namespace Peliculas.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenerosController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;

        public GenerosController(
            AppDbContext appDbContext,
            IMapper mapper
        )
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<GeneroDto> generos;
            List<Genero> entities;

            entities = await _appDbContext.Genero.ToListAsync();
            generos = _mapper.Map<List<GeneroDto>>(entities);

            return Ok(generos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            GeneroDto genero;
            Genero entity;

            entity = await _appDbContext.Genero.Where(x=> x.Id == id).FirstOrDefaultAsync();
            if (entity is null)            
                return NotFound();
            
            genero = _mapper.Map<GeneroDto>(entity);

            return Ok(genero);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GeneroDtoIn generoDtoIn)
        {
            Genero entity;

            entity = _mapper.Map<Genero>(generoDtoIn);
            await _appDbContext.Genero.AddAsync(entity);
            await _appDbContext.SaveChangesAsync();

            return Created($"/Generos/{entity.Id}", new { Id = entity.Id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] GeneroDtoIn generoDtoIn)
        {
            Genero genero;

            genero = await _appDbContext.Genero.Where(x => x.Id == id).FirstAsync();
            if (genero == null)
                return NotFound();
            genero.Nombre = generoDtoIn.Nombre;
            _appDbContext.Genero.Update(genero);
            await _appDbContext.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Cines/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGenero(int id)
        {
            if (_appDbContext.Genero == null)
            {
                return NotFound();
            }
            var cine = await _appDbContext.Cine.FindAsync(id);
            if (cine == null)
            {
                return NotFound();
            }

            _appDbContext.Cine.Remove(cine);
            await _appDbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
