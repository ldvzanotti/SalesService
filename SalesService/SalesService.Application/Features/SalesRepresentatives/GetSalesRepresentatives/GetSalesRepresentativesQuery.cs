using MediatR;
using SalesService.Application.Utils;

namespace SalesService.Application.Features.SalesRepresentatives.GetSalesRepresentatives
{
    public record GetSalesRepresentativesQuery(string TaxpayerRegistration) : IRequest<GetSalesRepresentativesResponse>
    {
        public string TaxpayerRegistration { get; init; } = TaxpayerRegistration.RemoveSpecialCharacters();
    }
}
