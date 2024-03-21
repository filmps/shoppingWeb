using Microsoft.AspNetCore.Mvc;

namespace OnlineWeb.Controllers  // Replace "YourApplicationNamespace" with your project's actual namespace
{
    public class CheckoutController : Controller
    {
        // Action method for displaying the Add Product page
        public IActionResult Checkout()
        {
            return View();
        }
    }
}
