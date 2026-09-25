using Microsoft.EntityFrameworkCore;
using P02_SalesDatabase.Models;

namespace P02_SalesDatabase.Data;

public class SalesContext : DbContext
{
    public SalesContext()
    {
    }

    public SalesContext(DbContextOptions<SalesContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();

    public DbSet<Customer> Customers => Set<Customer>();

    public DbSet<Store> Stores => Set<Store>();

    public DbSet<Sale> Sales => Set<Sale>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured)
        {
            return;
        }

        var connectionString = Environment.GetEnvironmentVariable("SALES_DB_CONNECTION")
            ?? @"Server=(localdb)\MSSQLLocalDB;Database=SalesDatabase;Trusted_Connection=True;TrustServerCertificate=True;";

        optionsBuilder.UseSqlServer(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(product => product.Name)
                .HasMaxLength(50)
                .IsUnicode();

            entity.Property(product => product.Quantity)
                .HasColumnType("real");

            entity.Property(product => product.Price)
                .HasColumnType("decimal(18,2)");

            entity.Property(product => product.Description)
                .HasMaxLength(250)
                .IsUnicode()
                .HasDefaultValue("No description");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.Property(customer => customer.Name)
                .HasMaxLength(100)
                .IsUnicode();

            entity.Property(customer => customer.Email)
                .HasMaxLength(80)
                .IsUnicode(false);

        });

        modelBuilder.Entity<Store>()
            .Property(store => store.Name)
            .HasMaxLength(80)
            .IsUnicode();

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.Property(sale => sale.Date)
                .HasDefaultValueSql("GETDATE()");

            entity.HasOne(sale => sale.Product)
                .WithMany(product => product.Sales)
                .HasForeignKey(sale => sale.ProductId);

            entity.HasOne(sale => sale.Customer)
                .WithMany(customer => customer.Sales)
                .HasForeignKey(sale => sale.CustomerId);

            entity.HasOne(sale => sale.Store)
                .WithMany(store => store.Sales)
                .HasForeignKey(sale => sale.StoreId);
        });
    }
}
