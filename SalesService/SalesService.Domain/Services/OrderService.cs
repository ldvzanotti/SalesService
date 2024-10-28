using SalesService.Domain.Abstractions;
using SalesService.Domain.Aggregates.Orders;
using SalesService.Domain.Aggregates.Orders.Events;

namespace SalesService.Domain.Services
{
    public static class OrderService
    {
        public static BusinessLogicValidationResult TryUpdateStatus(Order order, OrderStatus newStatus, out OrderStatusUpdated statusUpdated)
        {
            if (order.Status.Equals(newStatus))
            {
                statusUpdated = null;
                return BusinessLogicValidationResult.Valid();
            }

            if (!OrderStatus.IsValidTransition(order.Status, newStatus))
            {
                statusUpdated = null;
                return BusinessLogicValidationResult.Invalid($"Não é possível alterar o status de {order.Status} para {newStatus}.");
            }

            statusUpdated = new OrderStatusUpdated(order.Id, newStatus);
            return BusinessLogicValidationResult.Valid();
        }

        public static BusinessLogicValidationResult TryUpdateItems(Order order, List<Item> items, out OrderItemsUpdated @event)
        {
            if (!order.Status.Equals(OrderStatus.PaymentPending))
            {
                @event = null;
                return BusinessLogicValidationResult.Invalid($"Os itens de um pedido só podem ser atualizados" +
                    $@"enquanto ele estiver no status ""{OrderStatus.PaymentPending.Name}""");
            }

            if (items.Count == 0)
            {
                @event = null;
                return BusinessLogicValidationResult.Invalid("Um pedido deve conter no mínimo 1 item.");
            }

            if (!items.TrueForAll(i => i.Units > 0))
            {
                @event = null;
                return BusinessLogicValidationResult.Invalid("Um pedido deve conter pelo menos 1 unidade de cada produto.");
            }

            if (!items.Except(order.Items).Any() || !order.Items.Except(items).Any())
            {
                @event = null;
                return BusinessLogicValidationResult.Valid();
            }

            @event = new(order.Id, items);
            return BusinessLogicValidationResult.Valid();
        }
    }
}
