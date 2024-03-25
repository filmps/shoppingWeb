using Microsoft.AspNetCore.Mvc;
using OnlineWeb.Data; // Make sure this using directive matches your project's structure
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace OnlineWeb.Components // Adjust the namespace based on your project's naming conventions
{
    public class CartViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public CartViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var cartId = 1; // Example, adjust based on your application's logic
            var cart = await _context.Carts.FirstOrDefaultAsync(c => c.CartId == cartId);
            int totalQuantity = cart?.TotalQuantity ?? 0;

            return View(totalQuantity); // This returns an IViewComponentResult
        }
    }
}
