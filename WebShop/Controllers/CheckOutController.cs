using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Braintree;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebShop.Data;
using WebShop.Extensions;
using WebShop.Models;





namespace WebShop.Controllers
{
    public class CheckOutController : Controller
    {

        private readonly DbShop _context;
        private List<Cart> carts;

        public CheckOutController(DbShop contex)
        {
            _context = contex;
            carts = new List<Cart>();
         }

        // GET: CheckOut/Create
        [HttpGet]
        public ActionResult CreateUser()
        {
            if(HttpContext.Session.GetObject<List<Cart>>("cartShop") == null)
            {
                
               TempData["Message"] = "Your cart is empty. please try buy something!";
                return   RedirectToAction(actionName: "Index", controllerName: "Cart");
                
            }

            return View(new User());
        }


        [HttpPost]
        public ActionResult CreateUser([Bind("UserId,Name,LastName,Country,City,Address,PostalCode,PhoneNumber,Email")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.User.Add(user);
                _context.SaveChanges();
                return RedirectToAction("CreateOrder", user);
            }

            return View(user);
        }


        
        public async Task<IActionResult> CreateOrder(User user)
        {
        

            try
            {
                Order order = new Order();
                order.UserId = user.UserId;
                order.DateCreate = DateTime.Now;
                order.DateDelivery = DateTime.Now.AddDays(7);
                order.IsDelivery = false;
                _context.Order.Add(order);
                await _context.SaveChangesAsync();



                if (HttpContext.Session.GetObject<List<Cart>>("cartShop") != null)
                {
                    carts = HttpContext.Session.GetObject<List<Cart>>("cartShop");


                    foreach (var item in carts)
                    {
                        OrderDetails orderDetails = new OrderDetails();
                        orderDetails.OrderId = order.OrderId;
                        orderDetails.ProductId = item.Product.ProductId;
                        orderDetails.Quantity = item.Quantity;
                        orderDetails.PriceForItem = item.Product.Price;
                       
                        _context.OrderDetail.Add(orderDetails);
                        await _context.SaveChangesAsync();
                    }

                    return RedirectToAction("OrderDetails", "CheckOut", new { id = order.OrderId });
                }
            }
            catch(Exception ex)
            {
                TempData["Message"] = "Error" + ex.Message;
                return View("CreateUser",user);
            }


        
                TempData["Message"] = "Your cart is not find!";
                return RedirectToAction("Index", "Cart");
            }


        
        public ActionResult OrderDetails(Guid id)
        {
            Order order = _context.Order.Include(x => x.User).FirstOrDefault(x => x.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }
     
            List<OrderDetails> orderDetails = _context.OrderDetail.Include(x => x.Product).Include(x => x.Product.MeasuringUnit).Where(x => x.OrderId == id).ToList();
            ViewBag.Details = orderDetails;
            return View(order);


        }



    // GET: CheckOut/Delete/5
    public ActionResult Delete(Guid id)
        {
            return View();
        }

        // POST: CheckOut/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, User collection)
        {
            try
            {
                // TODO: Add delete logic here
                return RedirectToAction(actionName: "Index", controllerName: "WebShop");
                
            }
            catch
            {
                return View();
            }
        }
    }
}