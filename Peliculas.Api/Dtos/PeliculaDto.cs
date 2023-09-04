using Microsoft.AspNetCore.Mvc;
using Peliculas.Api.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Peliculas.Api.Dtos
{
    public class PeliculaDto : PeliculaBaseDto
    {
        public int Id { get; set; }

        public string Poster { get; set; }

        public List<GeneroDto> Generos { get; set; }

        public List<PeliculaActorDto> Actores { get; set; }

        public List<CineDto> Cines { get; set; }
    }

    public class PeliculaBaseDto
    {
        [MaxLength(100)]
        public string Titulo { get; set; }

        public bool EnCines { get; set; } = false;

        public DateTime FechaDeLanzamiento { get; set; }

        [MaxLength(1000)]
        public string Trailer { get; set; }

        [MaxLength(1000)]
        public string Resumen { get; set; }
    }

    public class PeliculaDtoIn//: PeliculaBaseDto
    {
        [MaxLength(100)]
        public string Titulo { get; set; }

        public bool EnCines { get; set; } = false;

        public DateTime FechaDeLanzamiento { get; set; }

        [StringLength(1000)]
        public string? Trailer { get; set; }

        [StringLength(1000)]
        public string? Resumen { get; set; }
        public IFormFile? Poster { get; set; }

        [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
        public List<int> GenerosId { get; set; }

        [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
        public List<int> CinesId { get; set; }

        [ModelBinder(BinderType = typeof(TypeBinder<List<ActorPeliculaDtoIn>>))]
        public List<ActorPeliculaDtoIn> Actores { get; set; }
    }

    public class LandingPageDto
    {
        public List<PeliculaDto> PeliculasEnCines { get; set; }

        public List<PeliculaDto> PeliculasProximosEstrenos { get; set; }
    }

}
