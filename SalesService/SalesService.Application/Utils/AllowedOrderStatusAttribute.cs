using SalesService.Domain.Aggregates.Orders;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace SalesService.Application.Utils
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class AllowedOrderStatusAttribute() : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is not string status)
                return ValidationResult.Success;

            var validValues = typeof(OrderStatus)
                .GetFields(BindingFlags.Public | BindingFlags.Static)
                .Select(f => f.GetValue(null) as OrderStatus)
                .Where(status => status != null)
                .ToList();

            if (validValues.Select(v => v.Name).Contains(status))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult($"Valor inválido para a propriedade {validationContext.MemberName}. " +
                $"Os valores válidos são: '{string.Join("', '", validValues.Select(v => v.Name))}'.");
        }
    }
}
