namespace P02_SalesDatabase.Models;

public class Product
{
    public int ProductId { get; set; }

    public required string Name { get; set; }

    public float Quantity { get; set; }

    public decimal Price { get; set; }

    public string Description { get; set; } = "No description";

    public ICollection<Sale> Sales { get; set; } = new HashSet<Sale>();
}
