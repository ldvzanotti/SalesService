using FluentValidation;

namespace SalesService.Application.Features.Orders.GetOrder
{
    internal class GetOrderDetailsValidator : AbstractValidator<GetOrderDetailsQuery>
    {
        public GetOrderDetailsValidator(GetOrderDetailsData data)
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(query => data.Order)
                .NotEmpty()
                .WithMessage(query => $"Pedido de ID {query.OrderId} não encontrado.");

            RuleFor(query => data.Order)
                .Must(salesRepresentativeId => data.SalesRepresentative is not null)
                .WithMessage((query, order) => $"Pedido de ID {order.Id}: não foi possível encontrar um representante de vendas de ID {order.SalesRepresentativeId}")
                .ChildRules(order =>
                {
                    order.RuleForEach(order => order.Items)
                        .Must(item => data.Products.Any(product => product.Id.Equals(item.ProductId)))
                        .WithMessage((order, item) => $"Pedido de ID {order.Id}: " +
                            $"não foi posssível encontrar um produto de ID {item.ProductId}");
                });
        }
    }
}
