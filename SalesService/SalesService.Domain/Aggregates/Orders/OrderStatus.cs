using SalesService.Domain.Abstractions;

namespace SalesService.Domain.Aggregates.Orders
{
    public class OrderStatus(int Id, string Name) : Enumeration(Id, Name)
    {
        public static readonly OrderStatus Cancelled = new(0, "Cancelado");
        public static readonly OrderStatus PaymentPending = new(1, "Aguardando pagamento");
        public static readonly OrderStatus PaymentApproved = new(2, "Pagamento aprovado");
        public static readonly OrderStatus ShippedToCarrier = new(3, "Enviado para transportadora");
        public static readonly OrderStatus Delivered = new(4, "Entregue");

        public static bool IsValidTransition(OrderStatus currentStatus, OrderStatus newStatus)
        {
            return ValidStatusTransitions.TryGetValue(currentStatus, out var validNextStatus) && validNextStatus.Contains(newStatus);
        }

        private static readonly Dictionary<OrderStatus, HashSet<OrderStatus>> ValidStatusTransitions = new()
        {
            { PaymentPending, new HashSet<OrderStatus> { PaymentApproved, Cancelled } },
            { PaymentApproved, new HashSet<OrderStatus> { ShippedToCarrier, Cancelled } },
            { ShippedToCarrier, new HashSet<OrderStatus> { Delivered } }
        };
    }
}
