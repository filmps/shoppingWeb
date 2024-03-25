using Microsoft.AspNetCore.Mvc;

namespace OnlineWeb.Controllers
{
    public class UserController : Controller
    {
        public IActionResult AddUser()
        {
            return View();
        }

        public IActionResult UserDetail()
        {
            return View();
        }

        public IActionResult UserEdit()
        {
            return View();
        }
    }
}
