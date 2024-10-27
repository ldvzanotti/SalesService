using SalesService.Domain.Abstractions;

namespace SalesService.Domain.Aggregates.Orders
{
    public class OrderStatus(int Id, string Name) : Enumeration(Id, Name)
    {
        public static readonly OrderStatus Cancelled = new(0, "Cancelada");
        public static readonly OrderStatus PaymentPending = new(1, "Aguardando Pagamento");
        public static readonly OrderStatus PaymentApproved = new(2, "Pagamento Aprovado");
        public static readonly OrderStatus ShippedToCarrier = new(3, "Enviado para Transportadora");
        public static readonly OrderStatus Delivered = new(4, "Entregue");
    }
}
