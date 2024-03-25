using Microsoft.AspNetCore.Mvc;

namespace OnlineWeb.Controllers
{
    public class UserController : Controller
    {
        public IActionResult AddUser()
        {
            return View();
        }

        public IActionResult Detail()
     {
    // Creating a test user object with sample data
     var testUser = new User
    {
        UserId = 1, // Assuming an ID for the example
        UserName = "test",
        UserAddress = "123 Test Lane",
        UserPhone = "555-1234"
        // Assuming Order property is commented out or not needed for this example
    };

    // Passing the testUser object to the view
    return View(testUser);
    }

        public IActionResult UserEdit()
        {
            return View();
        }
    }
}
