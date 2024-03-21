using Microsoft.AspNetCore.Mvc;

namespace OnlineWeb.Controllers  // Replace "YourApplicationNamespace" with your project's actual namespace
{
    public class ProductController : Controller
    {
        // Action method for displaying the Add Product page
        public IActionResult AddProduct()
        {
            return View();
        }

        public IActionResult ProductDetails()
        {
            return View();
        }
    }
}
