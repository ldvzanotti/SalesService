using Marten;
using MediatR;
using SalesService.Application.Utils;
using SalesService.Domain.Aggregates.Orders;
using SalesService.Domain.Aggregates.Products;
using SalesService.Domain.Aggregates.SalesRepresentatives;

namespace SalesService.Application.Features.Orders.GetOrderDetails
{
    public class GetOrderDetailsHandler(IQuerySession querySession)
        : IRequestHandler<GetOrderDetailsQuery, GetOrderDetailsResponse>
    {
        private readonly GetOrderDetailsResponse _response = new();
        private GetOrderDetailsData _data;

        public async Task<GetOrderDetailsResponse> Handle(GetOrderDetailsQuery query, CancellationToken cancellationToken)
        {
            await LoadData(query, cancellationToken);

            if (!ValidateQuery(query))
                return _response;

            _response.Order = new(
                Id: _data.Order.Id,
                CreationDate: _data.Order.CreationDate,
                Status: _data.Order.Status.Name,
                SalesRepresentative: MapSalesRepresentative(),
                Items: MapItems()
            );

            return _response;
        }

        private async Task LoadData(GetOrderDetailsQuery query, CancellationToken cancellationToken)
        {
            var order = await querySession.LoadAsync<Order>(query.OrderId, cancellationToken);

            var salesRepresentative = await querySession.LoadAsync<SalesRepresentative>(order.SalesRepresentativeId, cancellationToken);

            var products = await querySession.LoadManyAsync<Product>(cancellationToken, order.Items.Select(i => i.ProductId).Distinct());

            _data = new(order, salesRepresentative, products);
        }

        private bool ValidateQuery(GetOrderDetailsQuery query)
        {
            var result = new GetOrderDetailsValidator(_data).Validate(query);

            if (!result.IsValid)
            {
                _response.Success = false;
                _response.Message = "Operação inválida.";
                _response.Errors = result.Errors.Select(e => e.ErrorMessage).ToList();
            }

            return result.IsValid;
        }

        private SalesRepresentativeDto MapSalesRepresentative()
        {
            return new(
                Id: _data.SalesRepresentative.Id,
                Name: _data.SalesRepresentative.FullName,
                TaxpayerRegistration: _data.SalesRepresentative.TaxpayerRegistration.MaskTaxpayerRegistration(),
                Phone: _data.SalesRepresentative.PhoneNumber.MaskPhoneNumber(),
                Email: _data.SalesRepresentative.Email
            );
        }

        private List<ItemDto> MapItems()
        {
            var items = new List<ItemDto>(_data.Order.Items.Count);

            foreach (var item in _data.Order.Items)
            {
                var product = _data.Products.First(product => product.Id.Equals(item.ProductId));

                items.Add(new(
                    ProductId: product.Id,
                    ProductName: product.Name,
                    Units: item.Units)
                );
            }

            return items;
        }
    }
}
