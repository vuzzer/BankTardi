using System.ComponentModel.DataAnnotations;

namespace BanqueTardi.CustomValidators
{
    public class CheckAmountValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null) {
                return null;
            }
           bool isAmount = double.TryParse(value.ToString(), out double amount);
            if (isAmount && amount >= 0) {
                return ValidationResult.Success;
            }
            return new ValidationResult(string.Format(ErrorMessage ?? "Le solde d'ouverture doit être positive"));
        }
    }
}
