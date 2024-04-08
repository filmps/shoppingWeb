using Microsoft.AspNetCore.Mvc;
using OnlineWeb.Data; // Use the correct namespace for your ApplicationDbContext
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using OnlineWeb.Models.ViewModels;
using OnlineWeb.Models;

public class CheckoutController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public CheckoutController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }
    public async Task<IActionResult> Checkout()
    {
        // Assuming _context is your database context
        var cart = await _context.Carts
                                 .Include(c => c.Items)
                                 .FirstOrDefaultAsync(c => c.CartId == 1);

        if (cart == null)
        {
            // Handle the case where the cart doesn't exist
            return NotFound("Cart not found.");
        }

        // Calculate the total price and total quantity if not already set
        // This step is optional if your model always keeps these properties up-to-date
        cart.AllTotalPrice = cart.Items.Sum(item => item.Price * item.Quantity);
        cart.TotalQuantity = cart.Items.Sum(item => item.Quantity);

        // Correcting the variable name from cartEntity to cart for creating the ViewModel
        var cartViewModel = new CartViewModel
        {

            CartId = 1,
            Items = cart.Items.Select(i => new CartItemViewModel
            {
                Id = i.CartItemId,
                CartId = 1,
                ProductId = i.ProductId,
                ProductName = i.ProductName,
                ProductImageUrl = i.ProductImageUrl,
                Quantity = i.Quantity,
                Price = i.Price
            }).ToList()
        };
        // Make sure to pass the correct ViewModel to your view
        return View(cartViewModel);
    }


    public async Task<IActionResult> PlaceOrder(Order order)
    {
        Console.WriteLine("Log message: image." + order.FirstName);
        if (!ModelState.IsValid)
        {
            foreach (var modelStateKey in ViewData.ModelState.Keys)
            {
                var modelStateVal = ViewData.ModelState[modelStateKey];
                foreach (var error in modelStateVal.Errors)
                {
                    var key = modelStateKey;
                    var errorMessage = error.ErrorMessage;
                    Console.WriteLine($"Error in {key}: {errorMessage}");
                }
            }
        }
        if (ModelState.IsValid)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            order.UserId = userId;

            // Optionally, add cart items to the order here
            // var cartItems = ... Fetch cart items for the user
            // Add cart items details to the order
            var cart = await _context.Carts.Include(c => c.Items)
                                    .FirstOrDefaultAsync(c => c.CartId == order.CartId);
            if (cart == null)
            {
                ModelState.AddModelError("", "Cart not found.");
                return View("Checkout", order); // Return with error if the cart is not found
            }
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Clear the cart after saving the order
            // ClearCart(userId);

            return RedirectToAction("OrderConfirmation", new { orderId = order.OrderId });
        }

        return View("Checkout");
    }

    public IActionResult OrderConfirmation(int orderId)
    {
        // Retrieve order details using orderId, if necessary
        // For demonstration, simply pass orderId to the view
        var order = _context.Orders.Find(orderId);
        if (order == null)
        {
            // Handle the case where the order doesn't exist
            return NotFound();
        }

        return View(order);
    }

    public async Task<IActionResult> OrderDetails(int orderId)
    {
        var order = await _context.Orders
                                  .Include(o => o.Cart)
                                  .ThenInclude(cart => cart.Items)
                                  .ThenInclude(item => item.Product) // Assuming each cart item links to a product
                                  .FirstOrDefaultAsync(o => o.OrderId == orderId);

        if (order == null)
        {
            return NotFound();
        }

        return View(order);
    }

    public async Task<IActionResult> MainOrderDetails()
    {
        var userId = _userManager.GetUserId(User);
        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToPage("/Account/Login", new { area = "Identity" }); // Redirect to login if user is not logged in


        }

        var order = await _context.Orders
                                  .Include(o => o.Cart)
                                  .ThenInclude(c => c.Items)
                                  .ThenInclude(item => item.Product) // Assuming each cart item links to a product
                                  .FirstOrDefaultAsync(o => o.UserId == userId);

        // if (order == null)
        // {
        //     return NotFound();
        // }

        return View(order);
    }

}
