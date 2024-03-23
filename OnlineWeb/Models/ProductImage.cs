public class ProductImage
{
    public int Id { get; set; } // Primary key
    public string ImageUrl { get; set; }
    public int ProductId { get; set; } // Foreign key to Product

    // Navigation property back to the associated Product
    public Product Product { get; set; }
}
