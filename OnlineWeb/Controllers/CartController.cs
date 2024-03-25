using Microsoft.AspNetCore.Mvc;
using OnlineWeb.Data;
using OnlineWeb.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using OnlineWeb.Models.ViewModels;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.ComponentModel;

namespace OnlineWeb.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public CartController(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // Helper method to get the cart from session
       

        // Helper method to save the cart to session

        // Action method to show the cart
        public async Task<IActionResult> ViewCart()
        {
            // Directly use the method to get cart from session
            var cartViewModel = new CartViewModel();

            var CartItems = await _context.CartItems.ToListAsync();

            foreach (var item in CartItems)
            {
                var itemViewModel = new CartItemViewModel
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    ProductImageUrl = item.ProductImageUrl,
                    Quantity = item.Quantity,
                    Price = item.Price
                };
                cartViewModel.Items.Add(itemViewModel);
            }

            return View(cartViewModel);
        }

    public async Task<IActionResult> AddToCart(int productId)
  { 
    var cartId = 1;
   
   var cart = await _context.Carts.FirstOrDefaultAsync(c => c.CartId == cartId);
    if (cart == null)
    {
        // Create a new Cart since it doesn't exist
        cart = new Cart { CartId = cartId };
        _context.Carts.Add(cart);
        await _context.SaveChangesAsync(); // Ensure the cart is saved and exists in the database
    }

    
    
    var product = await _context.Products
        .Include(p => p.ProductImageFiles) // Assuming you want to include related images
        .FirstOrDefaultAsync(p => p.Id == productId);

    if(product == null)
    {
        return NotFound();
    }

     var cartItem = await _context.CartItems
                                  .Where(c => c.ProductId == productId&&c.CartId == cartId )
                                  .FirstOrDefaultAsync();

    if (cartItem != null)
    {
        // Item already exists in the cart, increase quantity
        cartItem.Quantity++;
    }
    else
    {
        // Add new item to the cart
        cartItem = new CartItem 
        {   CartId = cartId,
            ProductId = product.Id,
            ProductName = product.ProductName, 
            ProductImageUrl = product.ProductImageFiles.FirstOrDefault()?.ImageUrl ?? "default-image-url", // Example transformation
            Price = product.Price, 
            Quantity = 1, 
        };
         _context.CartItems.Add(cartItem); // Save the updated cart back to session
        Console.WriteLine("Log message: product id."+ cartItem.ProductId);
    }

    await _context.SaveChangesAsync();
    var cartItems = await _context.CartItems.Where(c => c.CartId == cartId).ToListAsync();
    int TotalQuantity = cartItems.Sum(item => item.Quantity);
    float TotalPrice = cartItems.Sum(item => item.Quantity*item.Price);

     Console.WriteLine("Log message: total quatity "+ TotalQuantity);
     Console.WriteLine("Log message: total price "+ TotalPrice);
     cart.TotalQuantity = TotalQuantity;
     cart.AllTotalPrice = TotalPrice;

    await _context.SaveChangesAsync();

    return RedirectToAction("ViewCart");
}

 public async Task<IActionResult> MainAddToCart(int productId,int quantity)
  { 
    var cartId = 1;
   
   var cart = await _context.Carts.FirstOrDefaultAsync(c => c.CartId == cartId);
    if (cart == null)
    {
        // Create a new Cart since it doesn't exist
        cart = new Cart { CartId = cartId };
        _context.Carts.Add(cart);
        await _context.SaveChangesAsync(); // Ensure the cart is saved and exists in the database
    }

    var product = await _context.Products
        .Include(p => p.ProductImageFiles) // Assuming you want to include related images
        .FirstOrDefaultAsync(p => p.Id == productId);

    if(product == null)
    {
        return NotFound();
    }

     var cartItem = await _context.CartItems
                                  .Where(c => c.ProductId == productId&&c.CartId == cartId )
                                  .FirstOrDefaultAsync();

    if (cartItem != null)
    {
        // Item already exists in the cart, increase quantity
        cartItem.Quantity= quantity + cartItem.Quantity;
    }
    else
    {
        // Add new item to the cart
        cartItem = new CartItem 
        {   CartId = cartId,
            ProductId = product.Id,
            ProductName = product.ProductName, 
            ProductImageUrl = product.ProductImageFiles.FirstOrDefault()?.ImageUrl ?? "default-image-url", // Example transformation
            Price = product.Price, 
            Quantity = quantity, 
        };
         _context.CartItems.Add(cartItem); // Save the updated cart back to session
        Console.WriteLine("Log message: product id."+ cartItem.ProductId);
    }

   
    await _context.SaveChangesAsync();

    var cartItems = await _context.CartItems.Where(c => c.CartId == cartId).ToListAsync();
    int TotalQuantity = cartItems.Sum(item => item.Quantity);
    float TotalPrice = cartItems.Sum(item => item.Quantity*item.Price);

     Console.WriteLine("Log message: total quatity "+ TotalQuantity);
     Console.WriteLine("Log message: total price "+ TotalPrice);
     cart.TotalQuantity = TotalQuantity;
     cart.AllTotalPrice = TotalPrice;

    await _context.SaveChangesAsync();

    return RedirectToAction("ViewCart");
}

  [HttpPost]
public async Task<IActionResult> UpdateItemQuantity(int productId, int quantity)
{
   
   var cartId = 1;
   var cart = await _context.Carts.FirstOrDefaultAsync(c => c.CartId == cartId);
   Console.WriteLine("Log message: quantity."+quantity);
   Console.WriteLine("Log message: itemid."+productId);
    if (quantity < 1)
        {
            // Handle error, e.g., by returning to the cart view with an error message
            // ModelState.AddModelError("", "Quantity must be at least 1");
            // return View("YourCartView");
            return RedirectToAction("ViewCart", new { errorMessage = "Quantity must be at least 1" });
        }

        var cartItem = await _context.CartItems.FirstOrDefaultAsync(c => c.ProductId == productId);
       

        if(cartItem == null)
             {
            // Handle error, e.g., by returning to the cart view with an error message
            // ModelState.AddModelError("", "Item not found");
            // return View("YourCartView");
            return RedirectToAction("ViewCart", new { errorMessage = "Item not found" });
        }
        
        cartItem.Quantity = quantity;
        Console.WriteLine("Log message: quantity.111"+cartItem.Quantity);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            // Log the error (uncomment ex variable name and write a log.)
            // ModelState.AddModelError("", "Unable to save changes.");
            // return View("YourCartView");
            return RedirectToAction("ViewCart", new { errorMessage = "Unable to save changes" });
        }

         var cartItems = await _context.CartItems.Where(c => c.CartId == cartId).ToListAsync();
         int TotalQuantity = cartItems.Sum(item => item.Quantity);
         float TotalPrice = cartItems.Sum(item => item.Quantity*item.Price);

         Console.WriteLine("Log message: total quatity "+ TotalQuantity);
         Console.WriteLine("Log message: total price "+ TotalPrice);
         cart.TotalQuantity = TotalQuantity;
         cart.AllTotalPrice = TotalPrice;

    await _context.SaveChangesAsync();

        // Redirect to the cart view, or wherever appropriate
        return RedirectToAction("ViewCart");
    }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteItem(int id)
        {
           var cartId = 1;
   
           var cart = await _context.Carts.FirstOrDefaultAsync(c => c.CartId == cartId);
           if (cart == null)
           {
        // Create a new Cart since it doesn't exist
               cart = new Cart { CartId = cartId };
               _context.Carts.Add(cart);
               await _context.SaveChangesAsync(); // Ensure the cart is saved and exists in the database
            }
           
           var cartItem = await _context.CartItems
                                  .Where(c => c.ProductId == id )
                                  .FirstOrDefaultAsync();

            if (cartItem != null)
            {
                 Console.WriteLine("Log message: item name"+cartItem.ProductName);
                _context.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();
                 var cartItems = await _context.CartItems.Where(c => c.CartId == cartId).ToListAsync();
                int TotalQuantity = cartItems.Sum(item => item.Quantity);
                float TotalPrice = cartItems.Sum(item => item.Quantity*item.Price);

                Console.WriteLine("Log message: total quatity "+ TotalQuantity);
                Console.WriteLine("Log message: total price "+ TotalPrice);
                cart.TotalQuantity = TotalQuantity;
                cart.AllTotalPrice = TotalPrice;

               await _context.SaveChangesAsync();
                // Redirect to a different action/controller as needed
               return RedirectToAction("ViewCart");
            }
            else 
            {
            Console.WriteLine("null");
            return NotFound();
            }
        }

}



        // Remember to implement other actions for adding items to the cart, etc.
}
