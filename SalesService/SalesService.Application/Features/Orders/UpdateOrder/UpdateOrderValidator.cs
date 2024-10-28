using FluentValidation;
using SalesService.Domain.Abstractions;
using SalesService.Domain.Aggregates.Orders;

namespace SalesService.Application.Features.Orders.UpdateOrder
{
    internal class UpdateOrderValidator : AbstractValidator<UpdateOrderCommand>
    {
        public UpdateOrderValidator(Order order)
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(command => order)
                .NotEmpty()
                .WithMessage(command => $"Pedido de ID {command.OrderId} não encontrado.");

            RuleFor(command => command.Order)
                .Cascade(CascadeMode.Stop)
                .Must((command, updatedOrder) => updatedOrder.Status is not null)
                .When(command => command.Order.Items is null)
                .WithMessage("Alguma informação do pedido deve ser atualizada.")

                .Must((command, updatedOrder) => updatedOrder.Items is not null)
                .When(command => command.Order.Status is null)
                .WithMessage("Alguma informação do pedido deve ser atualizada.")


                .Must(updatedOrder => Enumeration.GetAll<OrderStatus>().Select(s => s.Name).Contains(updatedOrder.Status))
                .When(command => command.Order.Status is not null)
                .WithMessage((command, updatedOrder) => $"Status {updatedOrder.Status} inválido.")

                .Must(updatedOrder => updatedOrder.Items is not null && updatedOrder.Items.TrueForAll(i => i.Units > 0))
                .When(command => command.Order.Status is not null)
                .WithMessage((command, updatedOrder) => $"Um pedido deve conter pelo menos 1 unidade de cada produto.");
        }
    }
}
