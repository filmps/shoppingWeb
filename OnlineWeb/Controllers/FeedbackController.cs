using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using OnlineWeb.Models;
using OnlineWeb.Data;
using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Options;
// using System.Net.Mail;
using System.Net;
using System;
using System.Security.Authentication;


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

        // [HttpPost]
        // public async Task<IActionResult> SendFeedback(Feedback feedback)
        // {
        //     if (ModelState.IsValid)
        //     {
        //         _context.Add(feedback);
        //         await _context.SaveChangesAsync();
        //         return RedirectToAction(nameof(Success));
        //     }
        //     return View(feedback);
        // }


        [HttpPost]
        public async Task<IActionResult> SendFeedback(Feedback feedback)

        {
            var message = new MimeMessage();
            // message.From.Add(new MailboxAddress("hanaliu2024", "hanaliu2024@gmail.com"));
            message.From.Add(new MailboxAddress("WebApp_Feedback", "519535143@qq.com"));
            // message.To.Add(new MailboxAddress("Support", "xbwudi20092@gmail.com"));
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
 
                // client.Connect("smtp.office365.com", 587,  false);

                // client.Connect("smtp.gmail.com", 587, SecureSocketOptions.Auto);
                // client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                client.Connect("smtp.qq.com", 587, SecureSocketOptions.StartTls);



                // Note: only needed if the SMTP server requires authentication
                // client.Authenticate("outlook_1A7BA034D7772E06@outlook.com", "eMbX!DJc67ac$u");
                // client.Authenticate("hanaliu2024@gmail.com", "vnefvfwptovciubz");
                client.Authenticate("519535143@qq.com", "pjlvpopquwhjbjdh");

                // client.Send(message);
                await client.SendAsync(message);
                client.Disconnect(true);
                return RedirectToAction(nameof(Success));
            }



            // var mailMessage = new MailMessage
            // {
            //     From = new MailAddress("hanaliu2024@gmail.com", "SenderName"),
            //     Subject = feedback.Title,
            //     Body = feedback.Content,
            //     IsBodyHtml = false
            // };
            // mailMessage.To.Add(new MailAddress("xbwudi20092@gmail.com"));

            // using (var client = new SmtpClient("smtp.gmail.com", 465))
            // {
            //     client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //     client.EnableSsl = true;
            //     client.UseDefaultCredentials = false;// disable it
            //     client.Credentials = new NetworkCredential("hanaliu2024@gmail.com", "vnefvfwptovciubz");
            //     await client.SendMailAsync(mailMessage);
            // }

            return RedirectToAction("Index");
        }
    }
}
