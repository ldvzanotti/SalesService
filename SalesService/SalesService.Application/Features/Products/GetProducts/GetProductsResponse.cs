using SalesService.Application.Dtos;

namespace SalesService.Application.Features.Products.GetProducts
{
    public record GetProductsResponse(List<ProductDto> Products) : ApiResponse;
}
