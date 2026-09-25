using Microsoft.EntityFrameworkCore;
using P02_SalesDatabase.Data;

try
{
    await using var context = new SalesContext();

    Console.WriteLine("Applying Sales Database migrations...");
    await context.Database.MigrateAsync();

    await DatabaseSeeder.SeedAsync(context);

    var sales = await context.Sales
        .AsNoTracking()
        .Include(sale => sale.Product)
        .Include(sale => sale.Customer)
        .Include(sale => sale.Store)
        .OrderBy(sale => sale.SaleId)
        .ToListAsync();

    Console.WriteLine($"Database ready. Showing {sales.Count} sales:");
    foreach (var sale in sales)
    {
        Console.WriteLine(
            $"#{sale.SaleId}: {sale.Product.Name} sold to {sale.Customer.Name} " +
            $"at {sale.Store.Name} on {sale.Date:yyyy-MM-dd HH:mm:ss}");
    }
}
catch (Exception exception)
{
    Console.Error.WriteLine("Could not connect to SQL Server or initialize the Sales Database.");
    Console.Error.WriteLine("Set SALES_DB_CONNECTION to a valid SQL Server connection string and try again.");
    Console.Error.WriteLine(exception.Message);
    return 1;
}

return 0;
