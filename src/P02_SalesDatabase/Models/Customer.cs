namespace P02_SalesDatabase.Models;

public class Customer
{
    public int CustomerId { get; set; }

    public required string Name { get; set; }

    public required string Email { get; set; }

    public required string CreditCardNumber { get; set; }

    public ICollection<Sale> Sales { get; set; } = new HashSet<Sale>();
}
