using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using OnlineWeb.Models;
using OnlineWeb.Data;
using Microsoft.EntityFrameworkCore;
using MailKit.Net.Smtp;
using MimeKit;
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

    public IActionResult SendEmail()
    {
        var email = new MimeMessage();
        email.From.Add(new MailboxAddress("Name", "email@example.com"));
        email.To.Add(new MailboxAddress("Support", "support@example.com"));
        email.Subject = "Test Email Subject";
        email.Body = new TextPart("plain")
        {
            Text = @"Hello Support,

    This is a test email.

    Regards,
    User"
        };

        using var client = new SmtpClient();
        // client.Connect("smtp.example.com", 587, false);
        // client.Authenticate("email@example.com", "password");
        
        try 
        {
            client.Send(email);
            client.Disconnect(true);
            ViewBag.Message = "Email successfully sent";
        } 
        catch 
        {
            ViewBag.Message = "There was an error sending the email";
        }

        return View("Index");

    }
}
