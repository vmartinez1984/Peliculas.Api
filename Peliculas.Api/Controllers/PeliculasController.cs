using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Peliculas.Api.Contexts;
using Peliculas.Api.Dtos;
using Peliculas.Api.Entities;

namespace Peliculas.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeliculasController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public PeliculasController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        // GET: api/Peliculas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pelicula>>> GetPelicula()
        {
          if (_context.Pelicula == null)
          {
              return NotFound();
          }
            return await _context.Pelicula.ToListAsync();
        }

        // GET: api/Peliculas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Pelicula>> GetPelicula(int id)
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

            return pelicula;
        }

        // PUT: api/Peliculas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPelicula(int id, Pelicula pelicula)
        {
            if (id != pelicula.Id)
            {
                return BadRequest();
            }

            _context.Entry(pelicula).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PeliculaExists(id))
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

        // POST: api/Peliculas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult> PostPelicula([FromForm]PeliculaDtoIn peliculaDtoIn)
        {
          if (_context.Pelicula == null)
          {
              return Problem("Entity set 'AppDbContext.Pelicula'  is null.");
          }
            Pelicula pelicula;

            pelicula = _mapper.Map<Pelicula>(peliculaDtoIn);
            _context.Pelicula.Add(pelicula);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPelicula", new { id = pelicula.Id }, pelicula);
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
