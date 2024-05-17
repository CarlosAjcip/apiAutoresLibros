using System.ComponentModel.DataAnnotations;

namespace apiAutoresLibros.Validaciones
{
    public class PrimeraLetraMayuscula : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }
            var primeraLetraMayuscula = value.ToString()[0].ToString();
            if (primeraLetraMayuscula != primeraLetraMayuscula.ToUpper())
            {
                return new ValidationResult("La Primera letra debe ser mayuscula");
            }

            return ValidationResult.Success;
        }
    }
}
