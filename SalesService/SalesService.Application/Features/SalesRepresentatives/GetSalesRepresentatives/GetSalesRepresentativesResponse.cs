using SalesService.Application.Dtos;

namespace SalesService.Application.Features.SalesRepresentatives.GetSalesRepresentatives
{
    public record GetSalesRepresentativesResponse(List<SalesRepresentativeDto> SalesRepresentatives) : ApiResponse;
}
