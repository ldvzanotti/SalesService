using SalesService.Domain.Abstractions;

namespace SalesService.Domain.Aggregates.SalesRepresentatives
{
    public class SalesRepresentative(string firstName, string lastName, string email, string taxpayerRegistration, string phoneNumber) : Entity
    {
        public string FirstName { get; set; } = firstName;
        public string LastName { get; set; } = lastName;
        public string Email { get; set; } = email;
        public string TaxpayerRegistration { get; set; } = taxpayerRegistration;
        public string PhoneNumber { get; set; } = phoneNumber;

        public string FullName => string.Join(" ", FirstName, LastName);
    }
}
