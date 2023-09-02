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

        public DbSet<Pelicula> Pelicula { get; set; }
        public DbSet<PeliculasActores> PeliculasActores { get; set; }
        public DbSet<PeliculasCines> PeliculasCines { get; set; }
        public DbSet<PeliculasGeneros> PeliculasGeneros { get; set; }

        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PeliculasActores>().HasKey(x => new { x.ActorId, x.PeliculaId });
            modelBuilder.Entity<PeliculasGeneros>().HasKey(x => new { x.GeneroId, x.PeliculaId });
            modelBuilder.Entity<PeliculasCines>().HasKey(x => new { x.CineId, x.PeliculaId });
            base.OnModelCreating(modelBuilder);
        }
    }
}