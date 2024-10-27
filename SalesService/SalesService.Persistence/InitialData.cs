using SalesService.Domain.Aggregates.Orders;
using SalesService.Domain.Aggregates.Products;
using SalesService.Domain.Aggregates.SalesRepresentatives;

namespace SalesService.Persistence
{
    public static class InitialData
    {
        public static readonly Product[] Products =
        [
            new("Resma de Papel A4", "A-0010-Z", new(25, "BRL")),
            new("Resma de Papel A4 - reciclado", "A-0023-Z", new(23, "BRL")),
            new("Tinta preta para impressora 100ml", "A-0036-Z", new(49.99m, "BRL"), "Aplicação: impressoras com tanque."),
            new("Tinta colorida para impressora 50ml", "A-0049-Z", new(45, "BRL"), "Aplicação: impressoras com tanque."),
            new("Cartolina 80x45", "A-0062-Z", new(5.50m, "BRL"))
        ];

        public static readonly SalesRepresentative[] SalesRepresentatives =
        [
            new("Dwight", "Schrute", "dwight@dundermifflin.com", "77271406000","5531971151732"),
            new("Michael", "Schott", "michael@dundermifflin.com", "07667880085", "5531993113697")
        ];

        public static readonly Order[] Orders =
        [
            new([new(Products[0].Id, 4), new(Products[1].Id, 2)], SalesRepresentatives[0].Id),
            new([new(Products[2].Id, 2)], SalesRepresentatives[1].Id)
        ];
    }
}
