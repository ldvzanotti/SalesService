using Marten;
using MediatR;
using SalesService.Domain.Abstractions;
using SalesService.Domain.Aggregates.Orders;
using SalesService.Domain.Services;

namespace SalesService.Application.Features.Orders.UpdateOrder
{
    public class UpdateOrderHandler(IQuerySession querySession, IDocumentSession documentSession)
        : IRequestHandler<UpdateOrderCommand, UpdateOrderResponse>
    {
        private readonly UpdateOrderResponse _response = new();

        public async Task<UpdateOrderResponse> Handle(UpdateOrderCommand command, CancellationToken cancellationToken)
        {
            var order = await querySession.LoadAsync<Order>(command.OrderId, cancellationToken);

            if (!ValidateCommand(command, order))
                return _response;

            await UpdateOrder(command.Order, order, cancellationToken);

            return _response;
        }

        private bool ValidateCommand(UpdateOrderCommand command, Order order)
        {
            var result = new UpdateOrderValidator(order).Validate(command);

            if (!result.IsValid)
            {
                _response.Success = false;
                _response.Message = "Operação inválida.";
                _response.Errors = result.Errors.Select(e => e.ErrorMessage).ToList();
            }

            return result.IsValid;
        }

        private async Task UpdateOrder(UpdateOrderDto updatedOrder, Order order, CancellationToken cancellationToken)
        {
            var events = new List<IEntityEvent>();

            if (updatedOrder.Items is not null)
            {
                var items = updatedOrder.Items.Select(i => new Item(i.ProductId, i.Units)).ToList();
                var updateItemsResult = OrderService.TryUpdateItems(order, items, out var orderItemsUpdated);

                if (!updateItemsResult.IsValid)
                {
                    _response.Success = false;
                    _response.Message = "Operação inválida.";
                    _response.Errors = updateItemsResult.Errors;
                    return;
                }

                if (orderItemsUpdated is not null)
                    events.Add(orderItemsUpdated);
            }

            if (updatedOrder.Status is not null)
            {
                var status = Enumeration.FromName<OrderStatus>(updatedOrder.Status);
                var updateStatusResult = OrderService.TryUpdateStatus(order, status, out var orderStatusUpdated);

                if (!updateStatusResult.IsValid)
                {
                    _response.Success = false;
                    _response.Message = "Operação inválida.";
                    _response.Errors = updateStatusResult.Errors;
                    return;
                }

                if (orderStatusUpdated is not null)
                    events.Add(orderStatusUpdated);
            }

            if (events.Count > 0)
            {
                await documentSession.Events.WriteToAggregate<Order>(order.Id, stream => stream.AppendMany(events), cancellationToken);
            }

            _response.Success = true;
            _response.Message = "Operação concluída com sucesso.";
        }
    }
}
