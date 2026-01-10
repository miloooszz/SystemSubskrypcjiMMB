using System.ComponentModel.DataAnnotations;

namespace SystemSubskrypcjiMMB.Models
{
    public class PastDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime date && date.Date > DateTime.Today)
            {
                return new ValidationResult("Data rozpoczęcia nie może być z przyszłości");
            }
            return ValidationResult.Success;
        }
    }

    public class FutureDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime date && date.Date <= DateTime.Today)
            {
                return new ValidationResult("Data zakończenia musi być w przyszłości");
            }
            return ValidationResult.Success;
        }
    }
}
