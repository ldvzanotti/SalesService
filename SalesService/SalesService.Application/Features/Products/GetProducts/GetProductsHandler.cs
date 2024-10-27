using MediatR;
using SalesService.Application.Utils;
using SalesService.Domain.Aggregates.Products;
using SalesService.Persistence;

namespace SalesService.Application.Features.Products.GetProducts
{
    public class GetProductsHandler(UnitOfWork unitOfWork)
        : IRequestHandler<GetProductsQuery, GetProductsResponse>
    {
        public async Task<GetProductsResponse> Handle(GetProductsQuery query, CancellationToken cancellationToken)
        {
            var products = query.Barcode.IsEmpty() ?
                await unitOfWork.ListAsync<Product>(cancellationToken: cancellationToken) :
                await unitOfWork.ListAsync<Product>(p => query.Barcode.Equals(p.Barcode), cancellationToken: cancellationToken);

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
