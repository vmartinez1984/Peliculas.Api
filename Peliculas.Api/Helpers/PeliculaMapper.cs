using AutoMapper;
using Peliculas.Api.Dtos;
using Peliculas.Api.Entities;

namespace Peliculas.Api.Helpers
{
    public class PeliculaMapper: Profile
    {
        public PeliculaMapper()
        {
            CreateMap<Genero, GeneroDto>();
            CreateMap<GeneroDtoIn, Genero>();

            CreateMap<Cine, CineDto>();
            CreateMap<CineDtoIn, Cine>();

            CreateMap<Pelicula, PeliculaDto>();
            CreateMap<PeliculaDtoIn, Pelicula>();
        }
    }
}
