using apiAutoresLibros.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace apiAutoresLibros.DTOs
{
    public class AutorCreacionDTO
    {
        [Required(ErrorMessage = "El campo {0} debe ser obligatorio")]
        [PrimeraLetraMayuscula]
        public string Nombre { get; set; }
    }
}
