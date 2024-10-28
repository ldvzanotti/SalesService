using FluentValidation;
using Marten;
using MediatR;
using SalesService.Application.Utils;
using SalesService.Domain.Aggregates.Orders;
using SalesService.Domain.Aggregates.Orders.Events;
using SalesService.Domain.Aggregates.Products;
using SalesService.Domain.Aggregates.SalesRepresentatives;

namespace SalesService.Application.Features.Orders.PlaceOrder
{
    public class PlaceOrderHandler(IQuerySession querySession, IDocumentSession documentSession)
        : IRequestHandler<PlaceOrderCommand, PlaceOrderResponse>
    {
        private readonly PlaceOrderResponse _response = new();
        private PlaceOrderData _data;

        public async Task<PlaceOrderResponse> Handle(PlaceOrderCommand command, CancellationToken cancellationToken)
        {
            if (!ValidateOrder(command.Order))
                return _response;

            await LoadData(command.Order, cancellationToken);

            if (!ValidateData(command.Order))
                return _response;

            await PlaceOrder(command.Order, cancellationToken);

            return _response;
        }

        private bool ValidateOrder(NewOrderDto order)
        {
            var result = new PlaceOrderValidator().Validate(order);

            if (!result.IsValid)
            {
                _response.Success = result.IsValid;
                _response.Message = "Operação inválida.";
                _response.Errors = result.Errors.Select(e => e.ErrorMessage).ToList();
            }

            return result.IsValid;
        }

        private async Task LoadData(NewOrderDto order, CancellationToken cancellationToken)
        {
            var salesRepresentative = await querySession.LoadAsync<SalesRepresentative>(order.SalesRepresentativeId, cancellationToken);

            var productsIds = order.Items.Select(p => p.ProductId).Distinct().ToList();
            var products = await querySession.Query<Product>().Where(product => productsIds.Contains(product.Id)).ToListAsync(cancellationToken);

            _data = new(salesRepresentative, products);
        }

        private bool ValidateData(NewOrderDto order)
        {
            var result = new PlaceOrderValidator(_data).Validate(order);

            if (!result.IsValid)
            {
                _response.Success = result.IsValid;
                _response.Message = "Operação inválida.";
                _response.Errors = result.Errors.Select(e => e.ErrorMessage).ToList();
            }
            return result.IsValid;
        }

        private async Task PlaceOrder(NewOrderDto order, CancellationToken cancellationToken)
        {
            var items = order.Items.Select(p => new Item(p.ProductId, p.Units)).ToList();

            var @event = new OrderCreated(items, order.SalesRepresentativeId);

            documentSession.Events.StartStream<Order>(@event.Id, @event);
            await documentSession.SaveChangesAsync(cancellationToken);

            _response.Order = new(
                @event.Id
                , @event.CreationDate,
                OrderStatus.PaymentPending.Name,
                MapSalesRepresentative(),
                MapItems(order.Items)
            );
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

        private List<ItemDto> MapItems(List<CartItemDto> cartItems)
        {
            var items = new List<ItemDto>(cartItems.Count);

            foreach (var item in cartItems)
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
