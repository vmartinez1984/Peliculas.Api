﻿using System.ComponentModel.DataAnnotations;

namespace Peliculas.Api.Entities
{
    public class Actor
    {
        public int Id { get; set; }

        [StringLength(255)]
        public string Nombre { get; set; }

        public DateTime FechaDeNacimiento { get; set; }

        public string? Foto { get; set; }

        public string Biografia { get; set; }

        public List<PeliculasActores> PeliculasActores { get; set; }

    }
}
