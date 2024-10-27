using MediatR;
using SalesService.Application.Utils;
using SalesService.Domain.Aggregates.SalesRepresentatives;
using SalesService.Persistence;

namespace SalesService.Application.Features.SalesRepresentatives.GetSalesRepresentatives
{
    public class GetSalesRepresentativesHandler(UnitOfWork unitOfWork)
        : IRequestHandler<GetSalesRepresentativesQuery, GetSalesRepresentativesResponse>
    {
        public async Task<GetSalesRepresentativesResponse> Handle(GetSalesRepresentativesQuery query, CancellationToken cancellationToken)
        {
            var salesRepresentatives = query.TaxpayerRegistration.IsEmpty() ?
                await unitOfWork.ListAsync<SalesRepresentative>(cancellationToken: cancellationToken) :
                await unitOfWork.ListAsync<SalesRepresentative>(s => query.TaxpayerRegistration.Equals(s.TaxpayerRegistration), cancellationToken: cancellationToken);

            return new GetSalesRepresentativesResponse(Map(salesRepresentatives));
        }

        private static List<SalesRepresentativeDto> Map(IReadOnlyCollection<SalesRepresentative> salesRepresentatives)
        {
            return salesRepresentatives.Select(s => new SalesRepresentativeDto(
                FullName: s.FullName,
                TaxpayerRegistration: s.TaxpayerRegistration,
                Phone: s.PhoneNumber,
                Email: s.Email)
            ).ToList();
        }
    }
}
