using Microsoft.EntityFrameworkCore;
using P02_SalesDatabase.Models;

namespace P02_SalesDatabase.Data;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(SalesContext context)
    {
        if (await context.Products.AnyAsync())
        {
            return;
        }

        var random = new Random(2024);

        var products = new[]
        {
            new Product { Name = "Laptop", Quantity = random.Next(5, 31), Price = RandomPrice(random, 700, 1800), Description = "Portable workstation" },
            new Product { Name = "Mechanical Keyboard", Quantity = random.Next(10, 61), Price = RandomPrice(random, 60, 180) },
            new Product { Name = "Wireless Mouse", Quantity = random.Next(10, 81), Price = RandomPrice(random, 20, 90) },
            new Product { Name = "4K Monitor", Quantity = random.Next(5, 26), Price = RandomPrice(random, 250, 700), Description = "High-resolution display" }
        };

        var customers = new[]
        {
            new Customer { Name = "Mona Hassan", Email = "mona@example.com", CreditCardNumber = "TEST-CARD-001" },
            new Customer { Name = "Omar Khaled", Email = "omar@example.com", CreditCardNumber = "TEST-CARD-002" },
            new Customer { Name = "Nour Adel", Email = "nour@example.com", CreditCardNumber = "TEST-CARD-003" }
        };

        var stores = new[]
        {
            new Store { Name = "Cairo Central" },
            new Store { Name = "Alexandria Tech Hub" }
        };

        context.AddRange(products);
        context.AddRange(customers);
        context.AddRange(stores);
        await context.SaveChangesAsync();

        var sales = Enumerable.Range(0, 8)
            .Select(_ => new Sale
            {
                ProductId = products[random.Next(products.Length)].ProductId,
                CustomerId = customers[random.Next(customers.Length)].CustomerId,
                StoreId = stores[random.Next(stores.Length)].StoreId
                // Date is deliberately omitted so SQL Server supplies GETDATE().
            });

        context.Sales.AddRange(sales);
        await context.SaveChangesAsync();
    }

    private static decimal RandomPrice(Random random, int minimum, int maximum) =>
        Math.Round((decimal)(minimum + random.NextDouble() * (maximum - minimum)), 2);
}
