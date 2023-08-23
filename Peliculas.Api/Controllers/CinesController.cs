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
    public class CinesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CinesController(
            AppDbContext context,
            IMapper mapper
        )
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Cines
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CineDto>>> GetCine()
        {
            if (_context.Cine == null)
            {
                return NotFound();
            }
            List<CineDto> lista;
            List<Cine> cines;

            cines = await _context.Cine.ToListAsync();
            lista = _mapper.Map<List<CineDto>>(cines);

            return Ok(lista);
        }

        // GET: api/Cines/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CineDto>> GetCine(int id)
        {
            if (_context.Cine == null)
            {
                return NotFound();
            }
            var cine = await _context.Cine.FindAsync(id);

            if (cine == null)
            {
                return NotFound();
            }


            return _mapper.Map<CineDto>(cine);
        }

        // PUT: api/Cines/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCine(int id, CineDtoIn cineDtoIn)
        {

            Cine cine;

            cine = await _context.Cine.FindAsync(id);
            cine.Nombre = cineDtoIn.Nombre;
            cine.Latitud = cineDtoIn.Latitud;
            cine.Longitud = cineDtoIn.Longitud;
            _context.Entry(cine).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CineExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Cines
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Cine>> PostCine(CineDtoIn cineDtoIn)
        {
            if (_context.Cine == null)
            {
                return Problem("Entity set 'AppDbContext.Cine'  is null.");
            }
            Cine cine;

            cine = _mapper.Map<Cine>(cineDtoIn);
            _context.Cine.Add(cine);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCine", new { id = cine.Id }, cineDtoIn);
        }

        // DELETE: api/Cines/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCine(int id)
        {
            if (_context.Cine == null)
            {
                return NotFound();
            }
            var cine = await _context.Cine.FindAsync(id);
            if (cine == null)
            {
                return NotFound();
            }

            _context.Cine.Remove(cine);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CineExists(int id)
        {
            return (_context.Cine?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
