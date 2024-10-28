namespace SalesService.Application.Features.Orders
{
    public record SalesRepresentativeDto(Guid Id, string Name, string TaxpayerRegistration, string Phone, string Email);
}
