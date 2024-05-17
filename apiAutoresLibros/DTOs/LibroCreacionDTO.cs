using apiAutoresLibros.Entidades;
using apiAutoresLibros.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace apiAutoresLibros.DTOs
{
    public class LibroCreacionDTO
    {
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [PrimeraLetraMayuscula]
        public string Titulo { get; set; }
        public DateTime FechaPublicacion { get; set; }
        public List<int> AutoresIds { get; set; }
    }
}
