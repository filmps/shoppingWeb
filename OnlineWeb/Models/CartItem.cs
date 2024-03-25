public class CartItem
{
    public int CartItemId { get; set; }
    
    public int ?CartId {get;set;}
    public int ProductId { get; set; } // Reference to a Product
    public string ProductName { get; set; }="";
    public string ProductImageUrl { get; set; } // Assuming only the first image is needed
    public int Quantity { get; set; }
    public float Price { get; set; } // Price per unit

    // Navigation property
    public Product Product { get; set; }
}
