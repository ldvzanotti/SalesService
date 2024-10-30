using Alba;
using Microsoft.AspNetCore.Http;
using SalesService.Application.Features.Orders.GetOrderDetails;
using SalesService.Application.Features.Orders.PlaceOrder;

namespace SalesService.Tests.Features
{
    [Binding]
    public class PlaceOrderTests(AppFixture fixture, ScenarioContext scenarioContext) : IClassFixture<AppFixture>
    {
        [Given("que fui atendido pelo representante de vendas de id {string}")]
        public void GivenQueFuiAtendidoPeloRepresentanteDeVendasDeId(string id)
        {
            scenarioContext["id"] = id;
        }

        [Given("que selecionei os produtos:")]
        public void GivenQueSelecioneiOsProdutos(DataTable dataTable)
        {
            scenarioContext["items"] = dataTable.CreateSet<CartItemDto>();
        }

        [When("registro uma nova venda")]
        public async Task WhenRegistroUmaNovaVenda()
        {
            var id = scenarioContext["id"] as string;
            var items = scenarioContext["items"] as List<CartItemDto>;

            scenarioContext["result"] = await fixture.Host.Scenario(_ =>
            {
                _.WithBearerToken(fixture.Jwt);
                _.Post.Json(new NewOrderDto(Guid.Parse(id), items)).ToUrl($"/api/v1/orders");
                _.IgnoreStatusCode();
            });
        }

        [Then("o pedido é criado com o status {string}")]
        public async Task ThenOPedidoECriadoComOStatus(string currentStatus)
        {
            var previousResult = scenarioContext["result"] as IScenarioResult;
            previousResult.Context.Response.StatusCode.Should().Be(StatusCodes.Status201Created);

            var previousResponse = await previousResult.ReadAsJsonAsync<GetOrderDetailsResponse>();

            var result = await fixture.Host.Scenario(_ =>
            {
                _.WithBearerToken(fixture.Jwt);
                _.Get.Url($"/api/v1/orders/{previousResponse.Order.Id}");
                _.StatusCodeShouldBeOk();
            });

            var response = await result.ReadAsJsonAsync<GetOrderDetailsResponse>();

            response.Order.Status.Should().Be(currentStatus);

        }

        [Then("o pedido não é criado")]
        public void ThenOPedidoNaoECriado()
        {
            (scenarioContext["result"] as IScenarioResult).Context.Response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }
    }
}
