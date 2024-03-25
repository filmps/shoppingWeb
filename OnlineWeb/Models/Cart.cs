
using Microsoft.Identity.Client;

public class Cart
{
    public int CartId { get; set; }
    public List<CartItem> Items { get; set; } = new List<CartItem>();

    public float AllTotalPrice { get; set; }

    public int TotalQuantity { get; set; }
  
}