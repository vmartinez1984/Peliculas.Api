using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NetTopologySuite.Geometries;
using Peliculas.Api.Dtos;
using Peliculas.Api.Entities;

namespace Peliculas.Api.Helpers
{
    public class PeliculaMapper : Profile
    {
        public PeliculaMapper(GeometryFactory geometry)
        {
            CreateMap<Genero, GeneroDto>();
            CreateMap<GeneroDtoIn, Genero>();

            CreateMap<Cine, CineDto>()
                .ForMember(x => x.Latitud, dto => dto.MapFrom(y => y.Ubicacion.Y))
                .ForMember(x => x.Longitud, dto => dto.MapFrom(y => y.Ubicacion.X));

            CreateMap<CineDtoIn, Cine>()
                .ForMember(
                    x => x.Ubicacion,
                    x => x.MapFrom(
                        dto => geometry.CreatePoint(new Coordinate(dto.Longitud, dto.Latitud))
                    )
                );

            CreateMap<Pelicula, PeliculaDto>()
                .ForMember(x => x.Generos, options => options.MapFrom(MapearPeliculasGeneros))
                .ForMember(x => x.Actores, options => options.MapFrom(MapearPeliculasActores))
                .ForMember(x => x.Cines, options => options.MapFrom(MapearPeliculasCines))
                ;
            CreateMap<PeliculaDtoIn, Pelicula>()
                .ForMember(x => x.Poster, opciones => opciones.Ignore())
                .ForMember(x => x.PeliculasGeneros, opciones => opciones.MapFrom(MapearPeliculasGeneros))
                .ForMember(x => x.PeliculasCines, opciones => opciones.MapFrom(MapearPeliculasCines))
                .ForMember(x => x.PeliculasActores, opciones => opciones.MapFrom(MapearPeliculasActores))
                ;

            CreateMap<Actor, ActorDto>();
            CreateMap<ActorDtoIn, Actor>();
            CreateMap<IdentityUser, UsuarioDto>();
        }

        private List<PeliculaActorDto> MapearPeliculasActores(Pelicula pelicula, PeliculaDto peliculaDto)
        {
            var resultado = new List<PeliculaActorDto>();

            if (pelicula.PeliculasGeneros != null)
                foreach (var item in pelicula.PeliculasActores)
                {
                    resultado.Add(new PeliculaActorDto
                    {
                        Id = item.Actor.Id,
                        Nombre = item.Actor.Nombre,
                        Foto = item.Actor.Foto,
                        Orden = item.Orden,
                        Personaje = item.Personaje
                    });
                }

            return resultado;
        }

        private List<CineDto> MapearPeliculasCines(Pelicula pelicula, PeliculaDto peliculaDto)
        {
            var resultado = new List<CineDto>();

            if (pelicula.PeliculasGeneros != null)
                foreach (var item in pelicula.PeliculasCines)
                {
                    resultado.Add(new CineDto
                    {
                        Id = item.CineId,
                        Nombre = item.Cine.Nombre,
                        Latitud = item.Cine.Ubicacion.Y,
                        Longitud = item.Cine.Ubicacion.X
                    });
                }

            return resultado;
        }

        private List<GeneroDto> MapearPeliculasGeneros(Pelicula pelicula, PeliculaDto peliculaDto)
        {
            var resultado = new List<GeneroDto>();
            if (pelicula.PeliculasGeneros != null)
                foreach (var item in pelicula.PeliculasGeneros)
                {
                    resultado.Add(new GeneroDto { Id = item.GeneroId, Nombre = item.Genero.Nombre });
                }

            return resultado;
        }

        private List<PeliculasGeneros> MapearPeliculasGeneros(PeliculaDtoIn peliculaDtoIn, Pelicula pelicula)
        {
            var resultado = new List<PeliculasGeneros>();

            if (peliculaDtoIn.GenerosId == null) { return resultado; }

            foreach (var item in peliculaDtoIn.GenerosId)
            {
                resultado.Add(new PeliculasGeneros { GeneroId = item });
            }

            return resultado;
        }

        private List<PeliculasCines> MapearPeliculasCines(PeliculaDtoIn peliculaDtoIn, Pelicula pelicula)
        {
            var resultado = new List<PeliculasCines>();

            if (peliculaDtoIn.CinesId == null) { return resultado; }

            foreach (var item in peliculaDtoIn.GenerosId)
            {
                resultado.Add(new PeliculasCines { CineId = item });
            }

            return resultado;
        }

        private List<PeliculasActores> MapearPeliculasActores(PeliculaDtoIn peliculaDtoIn, Pelicula pelicula)
        {
            var resultado = new List<PeliculasActores>();

            if (peliculaDtoIn.Actores == null) { return resultado; }

            foreach (var item in peliculaDtoIn.Actores)
            {
                resultado.Add(new PeliculasActores { ActorId = item.Id, Personaje = item.Personaje });
            }

            return resultado;
        }
    }
}
