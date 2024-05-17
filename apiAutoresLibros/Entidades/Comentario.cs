using Microsoft.AspNetCore.Identity;

namespace apiAutoresLibros.Entidades
{
    public class Comentario
    {
        public int Id { get; set; }
        public string Contenido { get; set; }
        public int LibroId { get; set; }
        public Libro Libro { get; set; }

        ////relacion con el usuario
        public string UsuarioId { get; set; }
        public IdentityUser Usuario { get; set; }
    }
}
