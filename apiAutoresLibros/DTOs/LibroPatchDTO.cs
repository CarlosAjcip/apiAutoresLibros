using apiAutoresLibros.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace apiAutoresLibros.DTOs
{
    public class LibroPatchDTO
    {
        [PrimeraLetraMayuscula]
        [StringLength(maximumLength: 300)]
        public string Titulo { get; set; }
        public DateTime FechaPublicacion { get; set; }
    }
}
