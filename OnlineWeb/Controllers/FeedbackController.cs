using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using OnlineWeb.Models;
using OnlineWeb.Data;
using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
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
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("WebApp_Feedback", "519535143@qq.com"));
            message.To.Add(new MailboxAddress("Support", "hanalau.unsw@gmail.com"));
            message.Subject = feedback.Title + "from: " + feedback.UserEmail;
            message.Body = new TextPart("html") 
            {
                Text = string.Format("<h1>{0}</h1><br><p>{1}</p>", feedback.Title, feedback.Content)
            };

            using (var client = new SmtpClient(new ProtocolLogger (Console.OpenStandardOutput ())))
            {
                client.CheckCertificateRevocation = false;

                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                client.Connect("smtp.qq.com", 587, SecureSocketOptions.StartTls);
                client.Authenticate("519535143@qq.com", "pjlvpopquwhjbjdh");
                await client.SendAsync(message);
                client.Disconnect(true);
                return RedirectToAction(nameof(Success));
            }
            return RedirectToAction("Index");
        }
    }
}