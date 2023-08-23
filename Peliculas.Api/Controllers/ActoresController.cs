using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class ActoresController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ActoresController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Actores
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Actor>>> GetActor()
        {
          if (_context.Actor == null)
          {
              return NotFound();
          }
            return await _context.Actor.ToListAsync();
        }

        // GET: api/Actores/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Actor>> GetActor(int id)
        {
          if (_context.Actor == null)
          {
              return NotFound();
          }
            var actor = await _context.Actor.FindAsync(id);

            if (actor == null)
            {
                return NotFound();
            }

            return actor;
        }

        // PUT: api/Actores/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutActor(int id, Actor actor)
        {
            if (id != actor.Id)
            {
                return BadRequest();
            }

            _context.Entry(actor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActorExists(id))
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

        // POST: api/Actores
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Actor>> PostActor([FromForm]ActorDtoIn actorIn)
        {
          if (_context.Actor == null)
          {
              return Problem("Entity set 'AppDbContext.Actor'  is null.");
          }
            Actor actor;

            actor = new Actor();
            _context.Actor.Add(actor);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetActor", new { id = actor.Id }, actor);
        }

        // DELETE: api/Actores/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActor(int id)
        {
            if (_context.Actor == null)
            {
                return NotFound();
            }
            var actor = await _context.Actor.FindAsync(id);
            if (actor == null)
            {
                return NotFound();
            }

            _context.Actor.Remove(actor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ActorExists(int id)
        {
            return (_context.Actor?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
