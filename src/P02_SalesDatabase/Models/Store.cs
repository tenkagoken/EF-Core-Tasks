namespace P02_SalesDatabase.Models;

public class Store
{
    public int StoreId { get; set; }

    public required string Name { get; set; }

    public ICollection<Sale> Sales { get; set; } = new HashSet<Sale>();
}
