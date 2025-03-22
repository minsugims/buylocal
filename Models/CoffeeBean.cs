namespace BuyLocalApp.Models;

public class CoffeeBean
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Roaster { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public List<string> FlavorNotes { get; set; } = [];
    public string ImageUrl { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}