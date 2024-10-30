using Alba;
using SalesService.Application.Features.Orders.GetOrderDetails;
using SalesService.Application.Features.Orders.UpdateOrder;

namespace SalesService.Tests.Features
{
    [Binding]
    public class UpdateOrderTests(AppFixture fixture, ScenarioContext scenarioContext) : IClassFixture<AppFixture>
    {
        [Given("que tenho um pedido de id {string} e status {string}")]
        public async Task GivenQueTenhoUmPedidoDeIdEStatus(string id, string currentStatus)
        {
            var result = await fixture.Host.Scenario(_ =>
            {
                _.WithBearerToken(fixture.Jwt);
                _.Get.Url($"/api/v1/orders/{id}");
                _.StatusCodeShouldBeOk();
            });

            var response = await result.ReadAsJsonAsync<GetOrderDetailsResponse>();

            response.Order.Status.Should().Be(currentStatus);

            scenarioContext["start"] = response;
        }

        [When("atualizo o status do pedido de {string} para {string}")]
        public async Task WhenAtualizoOStatusDoPedidoDePara(string id, string newStatus)
        {
            await fixture.Host.Scenario(_ =>
            {
                _.WithBearerToken(fixture.Jwt);
                _.Patch.Json(new UpdateOrderDto(newStatus, null)).ToUrl($"/api/v1/orders/{id}");
                _.IgnoreStatusCode();
            });
        }

        [Then("o status do pedido de id {string} atualiza para {string}")]
        [Then("o pedido de id {string} continua no status {string}")]
        public async Task ThenOStatusDoPedidoDeIdAtualizaPara(string id, string finalStatus)
        {
            var result = await fixture.Host.Scenario(_ =>
            {
                _.WithBearerToken(fixture.Jwt);
                _.Get.Url($"/api/v1/orders/{id}");
                _.StatusCodeShouldBeOk();
            });

            var response = await result.ReadAsJsonAsync<GetOrderDetailsResponse>();

            response.Order.Status.Should().BeEquivalentTo(finalStatus);
        }

        [When("atualizo os itens do pedido de id {string} para:")]
        public async Task WhenAtualizoOsItensDoPedidoDeIdPara(string id, DataTable dataTable)
        {
            var items = dataTable.CreateSet<Application.Features.Orders.UpdateOrder.ItemDto>().ToList();

            await fixture.Host.Scenario(_ =>
            {
                _.WithBearerToken(fixture.Jwt);
                _.Patch.Json(new UpdateOrderDto(null, items)).ToUrl($"/api/v1/orders/{id}");
                _.IgnoreStatusCode();
            });
        }

        [Then("o pedido de id {string} não é atualizado")]
        public async Task ThenOPedidoDeIdNaoEAtualizado(string id)
        {
            var result = await fixture.Host.Scenario(_ =>
            {
                _.WithBearerToken(fixture.Jwt);
                _.Get.Url($"/api/v1/orders/{id}");
                _.StatusCodeShouldBeOk();
            });

            var response = await result.ReadAsJsonAsync<GetOrderDetailsResponse>();

            response.Should().BeEquivalentTo(scenarioContext["start"] as GetOrderDetailsResponse);
        }

        [Then("o pedido de id {string} é atualizado para:")]
        public async Task ThenOPedidoDeIdEAtualizadoPara(string id, DataTable dataTable)
        {
            var items = dataTable.CreateSet<Application.Features.Orders.ItemDto>().ToList();

            var result = await fixture.Host.Scenario(_ =>
            {
                _.WithBearerToken(fixture.Jwt);
                _.Get.Url($"/api/v1/orders/{id}");
                _.StatusCodeShouldBeOk();
            });

            var response = await result.ReadAsJsonAsync<GetOrderDetailsResponse>();

            response.Order.Items.Should().BeEquivalentTo(items);
        }
    }
}
