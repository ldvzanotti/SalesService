using System.ComponentModel.DataAnnotations;

namespace SalesService.Application.Utils
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class MinLengthNotRequiredAttribute(int minLength) : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var stringValue = value as string;

            if (!string.IsNullOrEmpty(stringValue) && stringValue.Length < minLength)
            {
                return new ValidationResult($"Número de itens deve ser maior que {minLength}.");
            }

            return ValidationResult.Success;
        }
    }
}
