using SalesService.Domain.Abstractions;
using SalesService.Domain.Aggregates.Orders;
using SalesService.Domain.Aggregates.Orders.Events;
using SalesService.Domain.Aggregates.Products;
using SalesService.Domain.Aggregates.SalesRepresentatives;

namespace SalesService.Persistence
{
    public static class InitialData
    {
        public static readonly Product[] Products =
        [
            new(Guid.Parse("AD96BD12-6A54-4CA4-B606-52BE1BEE160F"), new DateTime(2024, 10, 01, 08, 01, 55, DateTimeKind.Utc), "Resma de Papel A4", "A-0010-Z", new(25, "BRL")),
            new(Guid.Parse("2296BA48-0D69-457B-AB86-B703C06DC8BE"), new DateTime(2024, 10, 01, 11, 03, 31, DateTimeKind.Utc), "Resma de Papel A4 - reciclado", "A-0023-Z", new(23, "BRL")),
            new(Guid.Parse("996F6038-4B6F-446D-B399-73E9F9139876"), new DateTime(2024, 10, 02, 14, 23, 33, DateTimeKind.Utc), "Tinta preta para impressora 100ml", "A-0036-Z", new(49.99m, "BRL"), "Aplicação: impressoras com tanque."),
            new(Guid.Parse("A0557406-471A-4889-9D35-784A36DEC927"), new DateTime(2024, 10, 03, 13, 44, 00, DateTimeKind.Utc), "Tinta colorida para impressora 50ml", "A-0049-Z", new(45, "BRL"), "Aplicação: impressoras com tanque."),
            new(Guid.Parse("8945CD1D-1F6A-4E4B-A89E-55D9A598B6C2"), new DateTime(2024, 10, 04, 17, 55, 59, DateTimeKind.Utc), "Cartolina 80x45", "A-0062-Z", new(5.50m, "BRL"))
        ];

        public static readonly SalesRepresentative[] SalesRepresentatives =
        [
            new(Guid.Parse("90B443C6-2711-4380-B101-F2DF684F473E"), new DateTime(2024, 10, 01, 07, 30, 00, DateTimeKind.Utc), "Dwight", "Schrute", "dwight@dundermifflin.com", "77271406000","5531971151732"),
            new(Guid.Parse("325D6625-C3B6-430C-8246-F0BC4F6F6575"), new DateTime(2024, 10, 02, 10, 55, 41, DateTimeKind.Utc), "Michael", "Schott", "michael@dundermifflin.com", "07667880085", "5531993113697")
        ];

        public static readonly Dictionary<Guid, IEntityEvent[]> OrderEvents = new()
        {
            {
                Guid.Parse("51922E52-C325-4F96-B548-01D8BEF5E475"),
                [
                    new OrderCreated([new(Products[0].Id, 4), new(Products[1].Id, 2)], SalesRepresentatives[0].Id, Guid.Parse("51922E52-C325-4F96-B548-01D8BEF5E475"), new DateTime(2024, 10, 11, 17, 12, 14, DateTimeKind.Utc)),
                    new OrderStatusUpdated(Guid.Parse("51922E52-C325-4F96-B548-01D8BEF5E475"), OrderStatus.PaymentApproved)
                ]
            },
            {
                Guid.Parse("91E48B69-6E2A-433B-ABA4-631BBAACE912"),
                [
                    new OrderCreated([new(Products[2].Id, 2)], SalesRepresentatives[1].Id, Guid.Parse("91E48B69-6E2A-433B-ABA4-631BBAACE912"), new DateTime(2024, 10, 12, 02, 57, 33, DateTimeKind.Utc)),
                    new OrderItemsUpdated(Guid.Parse("91E48B69-6E2A-433B-ABA4-631BBAACE912"), [new(Products[2].Id, 1)]),
                    new OrderStatusUpdated(Guid.Parse("91E48B69-6E2A-433B-ABA4-631BBAACE912"), OrderStatus.Cancelled)
                ]
            },
            {
                Guid.Parse("FA0167FC-85BC-4039-B5B0-B9EAF525AB65"),
                [
                    new OrderCreated([new(Products[3].Id, 1)], SalesRepresentatives[0].Id, Guid.Parse("FA0167FC-85BC-4039-B5B0-B9EAF525AB65"), new DateTime(2024, 10, 25, 21, 03, 00, DateTimeKind.Utc)),
                    new OrderItemsUpdated(Guid.Parse("FA0167FC-85BC-4039-B5B0-B9EAF525AB65"), [new(Products[3].Id, 4)]),
                    new OrderStatusUpdated(Guid.Parse("FA0167FC-85BC-4039-B5B0-B9EAF525AB65"), OrderStatus.PaymentApproved),
                    new OrderStatusUpdated(Guid.Parse("FA0167FC-85BC-4039-B5B0-B9EAF525AB65"), OrderStatus.ShippedToCarrier)
                ]
            },
            {
                Guid.Parse("CDFD5449-23EE-472A-A0D7-0034BC3DA0D2"),
                [
                    new OrderCreated([new(Products[4].Id, 5)], SalesRepresentatives[1].Id, Guid.Parse("CDFD5449-23EE-472A-A0D7-0034BC3DA0D2"), new DateTime(2024, 10, 24, 09, 44, 59, DateTimeKind.Utc)),
                    new OrderStatusUpdated(Guid.Parse("CDFD5449-23EE-472A-A0D7-0034BC3DA0D2"), OrderStatus.PaymentApproved),
                    new OrderStatusUpdated(Guid.Parse("CDFD5449-23EE-472A-A0D7-0034BC3DA0D2"), OrderStatus.Cancelled)
                ]
            },
            {
                Guid.Parse("ED7F46D1-5D35-4FF2-93E1-EF8881CB4B59"),
                [
                    new OrderCreated([new(Products[0].Id, 1)], SalesRepresentatives[0].Id, Guid.Parse("ED7F46D1-5D35-4FF2-93E1-EF8881CB4B59"), new DateTime(2024, 10, 27, 02, 33, 11, DateTimeKind.Utc)),
                    new OrderStatusUpdated(Guid.Parse("ED7F46D1-5D35-4FF2-93E1-EF8881CB4B59"), OrderStatus.PaymentApproved),
                    new OrderStatusUpdated(Guid.Parse("ED7F46D1-5D35-4FF2-93E1-EF8881CB4B59"), OrderStatus.ShippedToCarrier),
                    new OrderStatusUpdated(Guid.Parse("ED7F46D1-5D35-4FF2-93E1-EF8881CB4B59"),OrderStatus.Delivered)
                ]
            }
        };
    }
}
