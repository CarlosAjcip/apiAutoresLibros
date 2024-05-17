using apiAutoresLibros.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace apiAutoresLibros.Entidades
{
    public class Libro
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [PrimeraLetraMayuscula]
        public string Titulo { get; set; }
        public DateTime? FechaPublicacion { get; set; }
        public List<Comentario> Comentarios { get; set; }
        public List<AutorLibro> AutoresLibros  { get; set; }
    }
}
