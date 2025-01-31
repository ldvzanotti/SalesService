﻿using MediatR;
using System.ComponentModel.DataAnnotations;

namespace SalesService.Application.Features.Orders.UpdateOrder
{
    public record UpdateOrderCommand : IRequest<UpdateOrderResponse>
    {
        [Required(ErrorMessage = "Deve ser informado o ID do pedido.")]
        public Guid OrderId { get; set; }

        [Required(ErrorMessage = "Devem ser informadas as atualizações do pedido.")]
        public UpdateOrderDto Order { get; set; }

        public UpdateOrderCommand(Guid orderId, UpdateOrderDto order)
        {
            OrderId = orderId;
            Order = order;
        }
    }
}
