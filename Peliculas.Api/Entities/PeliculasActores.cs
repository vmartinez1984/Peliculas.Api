﻿using System.ComponentModel.DataAnnotations;

namespace Peliculas.Api.Entities
{
    public class PeliculasActores
    {
        public int PeliculaId { get; set; }

        public int ActorId { get; set; }

        public Pelicula Pelicula { get; set; }

        public Actor Actor { get; set; }

        [StringLength(maximumLength: 100)]
        public string Personaje { get; set; }

        public int Orden { get; set; }

    }
}
