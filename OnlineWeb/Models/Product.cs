public class Product
{
    public int Id { get; set; } // Primary key
    public string ProductName { get; set; } // Add ProductName property
    public float Price { get; set; }
    public string? Style { get; set; }

    public string? Category { get; set; }
    public string? Description { get; set; }

    // Navigation property for related ProductImage entries
    public List<ProductImage>? ProductImageFiles { get; set; } = new List<ProductImage>();
    
     // Add this field to keep track of update time
    public DateTime LastUpdated { get; set; }
}