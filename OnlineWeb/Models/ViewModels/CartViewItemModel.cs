using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineWeb.Models.ViewModels // Adjust the namespace to match your project's structure
{

    public class CartViewModel
{
    public List<CartItemViewModel> Items { get; set; } = new List<CartItemViewModel>();
    public float TotalPrice => Items.Sum(item => item.TotalPrice);

    public int TotalQuantity => Items.Sum(item=>item.Quantity);
}

public class CartItemViewModel
{
    
    public int Id { get; set; } 

    public int CartId{get;set;}
    public int ProductId {get;set;}
    public string ProductName { get; set; } ="";
    public string ProductImageUrl { get; set; }="";
    public int Quantity { get; set; }
    public float Price { get; set; } // Price per unit
    public float TotalPrice => Price * Quantity;
}

}
