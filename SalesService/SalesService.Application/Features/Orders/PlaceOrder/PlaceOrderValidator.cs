using FluentValidation;

namespace SalesService.Application.Features.Orders.PlaceOrder
{
    internal class PlaceOrderValidator : AbstractValidator<NewOrderDto>
    {
        public PlaceOrderValidator()
        {
            RuleFor(order => order.SalesRepresentativeId)
                .NotEmpty()
                .WithMessage("Um pedido deve estar relacionado a um representante de vendas.");

            RuleFor(order => order.Items)
                .NotEmpty()
                .WithMessage("Um pedido deve conter pelo menos 1 produto.");

            RuleForEach(order => order.Items)
                .Must(product => product.Units > 0)
                .WithMessage("Um pedido deve conter pelo menos 1 unidade de cada produto.");
        }

        public PlaceOrderValidator(PlaceOrderData data)
        {
            RuleFor(order => data.SalesRepresentative)
                .NotEmpty()
                .WithMessage(order => $"Representante de vendas de ID {order.SalesRepresentativeId} não encontrado.");

            RuleForEach(order => order.Items)
                .Must(product => data.Products.Any(p => p.Id.Equals(product.ProductId)))
                .WithMessage((order, product) => $"Produto de ID {product.ProductId} não encontrado.");
        }
    }
}
