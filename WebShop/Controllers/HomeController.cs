using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebShop.Data;
using WebShop.Models;

namespace WebShop.Controllers
{
    public class HomeController : Controller
    {

        private readonly DbShop _context;

        public HomeController(DbShop context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Welcome to us webshop site!";

            return View();
        }

        [HttpGet]
        public IActionResult Contact()
        {

            ViewData["Message"] = "Your contact page.";
            return View();
        }


        [HttpPost]
        public IActionResult SendContact(string name, string email, string subject, string message, int updates)
        {
            ViewData["Message"] = "Your contact page.";

            //string name = Request.Form["name"];
            //string email = Request.Form["email"];
            //string subject = Request.Form["subject"];
            //string message = Request.Form["message"];
            //int updates = int.Parse(Request.Form["updates"]);

            Contact contact = new Contact();
            contact.Name = name;
            contact.Email = email;
            contact.Message = message;
            contact.Subject = subject;
            contact.Updates =  updates == 0 ? false : true;

            try
            {
                //SmtpClient client = new SmtpClient();
                //client.Host = "smtp.gmail.com";
                //client.Port = 587;
                //client.Credentials = new NetworkCredential("tihomir.vanjurek@gmail.com", "xxxxxx");
                //client.EnableSsl = true;

                //var mailMessage = new MailMessage();
                //mailMessage.From = new MailAddress(email);
                //mailMessage.To.Add("tihomir.vanjurek@gmail.com");
                //mailMessage.Subject = subject;
                //mailMessage.Body = message;
                //mailMessage.IsBodyHtml = true;

               // client.Send(mailMessage);

                _context.Contact.Add(contact);
                _context.SaveChanges();
            }
            catch(Exception ex)
            {
                TempData["Message"] = string.Format("Errors: + {0}", ex.Message );
                return RedirectToAction(actionName: "Index", controllerName: "WebShop");
            }

            ViewData["Message"] = "Send Contact is successful!";
            return View("Contact");
        }


        public IActionResult Dashboard()
        {
            Product scott = _context.Product.FirstOrDefault(x => x.Name.Contains("Scott"));
            Product trek = _context.Product.FirstOrDefault(x => x.Name.Contains("Trek"));
            Product fuji = _context.Product.FirstOrDefault(x => x.Name.Contains("Fuji"));

            DashboardViewModal modal = new DashboardViewModal();
            modal.PriceScott = scott.Price;
            modal.PriceTrek = trek.Price;
            modal.PriceFuji = fuji.Price;

            return View(modal);
        }



        public IActionResult Gallery()
        {

            return View();
        }



        public IActionResult ChatHub()
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
    }
}
