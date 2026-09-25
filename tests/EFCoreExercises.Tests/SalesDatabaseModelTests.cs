using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using P02_SalesDatabase.Data;
using P02_SalesDatabase.Models;

namespace EFCoreExercises.Tests;

public sealed class SalesDatabaseModelTests
{
    private static readonly SalesContext Context = CreateContext();
    private static readonly IModel Model = Context.Model;

    [Fact]
    public void Product_customer_and_store_constraints_match_the_exercise()
    {
        var product = Model.FindEntityType(typeof(Product))!;
        var customer = Model.FindEntityType(typeof(Customer))!;
        var store = Model.FindEntityType(typeof(Store))!;

        Assert.Equal(50, product.FindProperty(nameof(Product.Name))!.GetMaxLength());
        Assert.Equal("real", product.FindProperty(nameof(Product.Quantity))!.GetColumnType());

        Assert.Equal(100, customer.FindProperty(nameof(Customer.Name))!.GetMaxLength());
        Assert.Equal(80, customer.FindProperty(nameof(Customer.Email))!.GetMaxLength());
        Assert.False(customer.FindProperty(nameof(Customer.Email))!.IsUnicode());
        Assert.Equal(80, store.FindProperty(nameof(Store.Name))!.GetMaxLength());
    }

    [Fact]
    public void Product_description_has_the_required_length_and_default()
    {
        var description = Model.FindEntityType(typeof(Product))!
            .FindProperty(nameof(Product.Description))!;

        Assert.Equal(250, description.GetMaxLength());
        Assert.Equal("No description", description.GetDefaultValue());
    }

    [Fact]
    public void Sale_date_uses_the_database_getdate_function()
    {
        var date = Model.FindEntityType(typeof(Sale))!
            .FindProperty(nameof(Sale.Date))!;

        Assert.Equal("GETDATE()", date.GetDefaultValueSql());
        Assert.Equal(ValueGenerated.OnAdd, date.ValueGenerated);
    }

    [Fact]
    public void Required_sales_migrations_are_discoverable_in_order()
    {
        var migrationNames = Context.Database.GetMigrations()
            .Select(id => id[(id.IndexOf('_') + 1)..])
            .ToArray();

        Assert.Equal(
            ["InitialCreate", "ProductsAddColumnDescription", "SalesAddDateDefault"],
            migrationNames);
    }

    [Fact]
    public void Generated_migration_script_contains_both_database_defaults()
    {
        var script = Context.GetService<IMigrator>().GenerateScript();

        Assert.Contains("No description", script, StringComparison.Ordinal);
        Assert.Contains("GETDATE()", script, StringComparison.OrdinalIgnoreCase);
    }

    private static SalesContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<SalesContext>()
            .UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=SalesModelTests;Trusted_Connection=True;TrustServerCertificate=True")
            .Options;

        return new SalesContext(options);
    }
}
