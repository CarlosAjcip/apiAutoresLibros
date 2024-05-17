using System.ComponentModel.DataAnnotations;

namespace apiAutoresLibros.DTOs
{
    public class EditarAdminDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
