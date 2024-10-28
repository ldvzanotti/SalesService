using SalesService.Domain.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace SalesService.Domain.Aggregates.SalesRepresentatives
{
    public record SalesRepresentative : IEntity, IAggregateRoot
    {
        public Guid Id { get; init; }
        public DateTime CreationDate { get; init; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        [Length(11, 11)]
        public string TaxpayerRegistration { get; set; }

        [Length(13, 13)]
        public string PhoneNumber { get; set; }

        public string FullName => string.Join(" ", FirstName, LastName);

        public SalesRepresentative()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
        }

        public SalesRepresentative(string firstName, string lastName, string email, string taxpayerRegistration, string phoneNumber)
            : this()
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            TaxpayerRegistration = taxpayerRegistration;
            PhoneNumber = phoneNumber;
        }

        public SalesRepresentative(Guid id, DateTime creationDate, string firstName, string lastName,
            string email, string taxpayerRegistration, string phoneNumber)
            : this(firstName, lastName, email, taxpayerRegistration, phoneNumber)
        {
            Id = id;
            CreationDate = creationDate;
        }
    }
}
