using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebShop.Data;
using WebShop.Models;
using WebShop.Extensions;
using Microsoft.EntityFrameworkCore;

namespace WebShop.Controllers
{
    public class CartController : Controller
    {
        private readonly DbShop _context;
        
        private List<Cart> carts;

        public CartController(DbShop dbShop)
        {
            _context = dbShop;
            carts = new List<Cart>();
          
        }


        public IActionResult Index()
        {
            
            //nova kartica
            if(HttpContext.Session.GetObject<List<Cart>>("cartShop") != null)
            {

                carts = HttpContext.Session.GetObject<List<Cart>>("cartShop");

                foreach (var item in carts)
                {
                    item.Product.MeasuringUnit = _context.MeasuringUnit.FirstOrDefault(x => x.MeasuringUnitId == item.Product.MeasuringUnitId);
                }

            }
            if(!carts.Any())
            {
                TempData["Message"] = "Shopping cart is empty";
            }



            return View("Cart", carts);
        }


        public async Task<IActionResult> AddToCart(Guid id, int q)
        {

            if (id == null)
            {
                return  NotFound();
            }
            else if(q == 0)
            {
                TempData["Message"] = "Quantity products is 0, Please insert quantity products into your cart";
                return Json(Url.Action("Details", "WebShop", new { id = id.ToString()}));
            }


            Cart cart = null;
            Int32? numberElementInCart = 0;

            try
            {
                Product product = await  _context.Product.FirstOrDefaultAsync(x => x.ProductId == id);

                if (HttpContext.Session.GetInt32("count") != null)
                {
                    numberElementInCart = HttpContext.Session.GetInt32("count");
                }


                if (product != null)
                {

                    if (product.Quantity > 0 && product.Available)
                    {
                        //nova kartica
                        if (HttpContext.Session.GetObject<List<Cart>>("cartShop") != null)
                        {
                            carts = HttpContext.Session.GetObject<List<Cart>>("cartShop");

                            if (carts.Where(x => x.Product.ProductId == product.ProductId).Any())
                            {
                                cart =  carts.Where(x => x.Product.ProductId == product.ProductId).FirstOrDefault();
                                carts.Remove(cart);
                                cart.Quantity = cart.Quantity + q;
                                product.Quantity = product.Quantity - q;
                                carts.Add(cart);
                            }
                            else
                            {
                                cart = new Cart();
                                cart.Product = product;
                                cart.Quantity = cart.Quantity + q;
                                product.Quantity = product.Quantity - q;
                                numberElementInCart = numberElementInCart + 1;
                                carts.Add(cart);
                              
                            }

                           
                        }
                        else
                        {

                            cart = new Cart();
                            cart.Product = product;
                            cart.Quantity = cart.Quantity + q;
                            product.Quantity = product.Quantity - q;
                            numberElementInCart = numberElementInCart + 1;
                            carts.Add(cart); 
                        }


                        if (product.Quantity <= 0)
                        {
                            product.Available = false;
                        }
                        _context.Entry(product).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                        TempData["Message"] = "Product " + product.Name + " add to cart!";
                    }
                    else
                    {
                        TempData["Message"] = "Product " + product.Name + " not available!";
                        return Json(Url.Action("Index", "WebShop"));
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
            }

            //vračam košaricu u session 
            HttpContext.Session.SetInt32("count", numberElementInCart.Value);
            //nova kartica
            HttpContext.Session.SetObject("cartShop", carts);
            return Json(Url.Action("Index", "WebShop"));
        }



        public ActionResult RemoveFromCart(Guid id)
        {

            if (id == null)
            {
                return NotFound();
            }
            try
            {
                Product product = _context.Product.Find(id);
                if (product == null)
                {
                    return NotFound();
                }

                if (HttpContext.Session.GetObject<List<Product>>("cartShop") != null)
                {

                    carts = HttpContext.Session.GetObject<List<Cart>>("cartShop");
                    Cart cart = carts.Find(x => x.Product.ProductId == id);
                  
                    product.Quantity = product.Quantity + cart.Quantity;
                    carts.Remove(cart);

                    if (product.Quantity > 0 && product.Available == false)
                    {
                        product.Available = true;
                    }
                    _context.Entry(product).State = EntityState.Modified;
                    _context.SaveChanges();


                    HttpContext.Session.SetObject("cartShop", carts);

                }


                if(HttpContext.Session.GetInt32("count") != null)
                {
                    Int32? i = 0;
                    i = HttpContext.Session.GetInt32("count");
                    i = i - 1;
                    HttpContext.Session.SetInt32("count", i.Value);
                }

                TempData["Message"] = "You remove product from your Cart: " + product.Name;
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
            }

            return RedirectToAction(actionName: "Index", controllerName: "WebShop");
        }


    }
}