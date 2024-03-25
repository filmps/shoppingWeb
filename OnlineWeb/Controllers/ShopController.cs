using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineWeb.Data;
using OnlineWeb.Models.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineWeb.Controllers
{
    public class ShopController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShopController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Shop()
        {
            var products = await _context.Products
                                         .Select(p => new ProductViewModel
                                         {
                                             ProductName = p.ProductName,
                                             Price = p.Price,
                                             Style = p.Style,
                                             Category = p.Category,
                                             Description = p.Description,
                                            ImageUrls = p.ProductImageFiles.Select(img => img.ImageUrl).ToList()
                                            
                                         })
                                         .ToListAsync();

            return View(products);
        }
    }
}
