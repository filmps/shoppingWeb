using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using OnlineWeb.Models;
using OnlineWeb.Data;
using MailKit.Net.Smtp;
using MimeKit;


namespace OnlineWeb.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly ApplicationDbContext _context;
        public FeedbackController(ApplicationDbContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Success()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> SendFeedback(Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                _context.Add(feedback);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Success));
            }
            return View(feedback);
        }


        // [HttpPost]
        // public IActionResult SendFeedback(Feedback feedback)
        // {
        //     var email = new MimeMessage();
        //     email.From.Add(new MailboxAddress("Name", "hanalau.unsw_bot@gmail.com"));
        //     email.To.Add(new MailboxAddress("Support", "hanalau.unsw@gmail.com"));

        //     email.Subject = feedback.Title;
        //     email.Body = new TextPart("plain")
        //     {
        //         Text = feedback.Content
        //     };

        //     using var client = new SmtpClient();
        //     client.Connect("smtp.example.com", 587, false);
        //     client.Authenticate("outlook@bot.com", "dasfasdfadsfasd");
        //     client.Send(email);
        //     client.Disconnect(true);

        //     return RedirectToAction("Index");
        // }
    }
}
