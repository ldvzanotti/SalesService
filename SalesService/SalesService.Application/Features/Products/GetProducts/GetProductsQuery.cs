using MediatR;

namespace SalesService.Application.Features.Products.GetProducts
{
    public record GetProductsQuery(string Barcode) : IRequest<GetProductsResponse>;
}
