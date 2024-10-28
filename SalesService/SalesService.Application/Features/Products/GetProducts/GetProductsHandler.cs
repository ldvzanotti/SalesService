using Marten;
using MediatR;
using SalesService.Application.Utils;
using SalesService.Domain.Aggregates.Products;

namespace SalesService.Application.Features.Products.GetProducts
{
    public class GetProductsHandler(IQuerySession querySession)
        : IRequestHandler<GetProductsQuery, GetProductsResponse>
    {
        public async Task<GetProductsResponse> Handle(GetProductsQuery query, CancellationToken cancellationToken)
        {
            var products = query.Barcode.IsEmpty() ?
                await querySession.Query<Product>().ToListAsync(cancellationToken) :
                await querySession.Query<Product>().Where(p => query.Barcode.Equals(p.Barcode)).ToListAsync(cancellationToken);

            return new GetProductsResponse(Map(products));
        }

        private static List<ProductDto> Map(IReadOnlyCollection<Product> products)
        {
            return products.Select(p => new ProductDto(
                Name: p.Name,
                Barcode: p.Barcode,
                UnitaryPrice: p.UnitaryPrice.ToString(),
                Description: p.Description)
            ).ToList();
        }
    }
}
