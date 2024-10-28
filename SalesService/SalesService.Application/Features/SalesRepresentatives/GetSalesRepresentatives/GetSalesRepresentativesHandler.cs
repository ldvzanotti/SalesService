using Marten;
using MediatR;
using SalesService.Application.Utils;
using SalesService.Domain.Aggregates.SalesRepresentatives;

namespace SalesService.Application.Features.SalesRepresentatives.GetSalesRepresentatives
{
    public class GetSalesRepresentativesHandler(IQuerySession querySession)
        : IRequestHandler<GetSalesRepresentativesQuery, GetSalesRepresentativesResponse>
    {
        public async Task<GetSalesRepresentativesResponse> Handle(GetSalesRepresentativesQuery query, CancellationToken cancellationToken)
        {
            var salesRepresentatives = query.TaxpayerRegistration.IsEmpty() ?
                await querySession.Query<SalesRepresentative>().ToListAsync(cancellationToken) :
                await querySession.Query<SalesRepresentative>()
                    .Where(s => query.TaxpayerRegistration.Equals(s.TaxpayerRegistration)).ToListAsync(cancellationToken);

            return new GetSalesRepresentativesResponse(Map(salesRepresentatives));
        }

        private static List<SalesRepresentativeDto> Map(IReadOnlyCollection<SalesRepresentative> salesRepresentatives)
        {
            return salesRepresentatives.Select(s => new SalesRepresentativeDto(
                FullName: s.FullName,
                TaxpayerRegistration: s.TaxpayerRegistration.MaskTaxpayerRegistration(),
                Phone: s.PhoneNumber.MaskPhoneNumber(),
                Email: s.Email)
            ).ToList();
        }
    }
}
