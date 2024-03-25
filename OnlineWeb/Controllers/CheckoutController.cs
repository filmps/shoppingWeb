using Microsoft.AspNetCore.Mvc;
using OnlineWeb.Data; // Use the correct namespace for your ApplicationDbContext
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class CheckoutController : Controller
{
    private readonly ApplicationDbContext _context;

    public CheckoutController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<IActionResult> Checkout()
        {
         
            return View();
        }
}
