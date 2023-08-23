using Microsoft.EntityFrameworkCore;
using Peliculas.Api.Entities;

namespace Peliculas.Api.Contexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Actor> Actor { get; set; }

        public DbSet<Genero> Genero { get; set; }

        public DbSet<Cine> Cine { get; set; }

        public DbSet<Peliculas.Api.Entities.Pelicula>? Pelicula { get; set; }
    }
}