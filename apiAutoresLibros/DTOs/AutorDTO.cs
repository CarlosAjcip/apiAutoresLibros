﻿

using apiAutoresLibros.Migrations;

namespace apiAutoresLibros.DTOs
{
    public class AutorDTO : Recurso
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
    }
}
