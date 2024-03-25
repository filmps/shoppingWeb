using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using OnlineWeb.Models;
using OnlineWeb.Data;
using Microsoft.EntityFrameworkCore;

namespace OnlineWeb.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
    {
            _context = context;
            _logger = logger;
    }
    public async Task<IActionResult> Index()
    {
        var cartId = 1;
        var cart = await _context.Carts.FirstOrDefaultAsync( c => c.CartId == cartId );
        
        if(cart != null)
        {
            ViewBag.TotalQuantity = cart.TotalQuantity;
        }
        else
        {
            ViewBag.TotalQuantity = 0; 
        }
         
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
