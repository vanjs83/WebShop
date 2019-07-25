using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebShop.Data;

namespace WebShop.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext contex)
        {
            
            _context = contex;
        }



      
        public ActionResult Index()
        {
            
            List<IdentityUserRole<string>> identityUserRole = _context.UserRoles.ToList();


           
            List<string> userRole = new List<string>();
      

            foreach (var item in identityUserRole)
            {
                IdentityUser user = _context.Users.Where(x => x.Id == item.UserId).FirstOrDefault();
                IdentityRole role = _context.Roles.Where(x => x.Id == item.RoleId).FirstOrDefault();
            
                string dict = "";     
                dict += "User name: ";
                dict += user.UserName;
                dict += "\n";
                dict += "Role name: ";
                dict += role.Name;
           
                userRole.Add(dict); 
            }
            
     
            ViewBag.UserRole = userRole;


            return View(userRole);
        }

        // GET: Admin/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Admin/Create
        public ActionResult Create()
        {
           
            ViewBag.User = new SelectList(_context.Users, "Id", "UserName");
            ViewBag.Role = new SelectList(_context.Roles, "Id", "Name");

            return View();
        }

        // POST: Admin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateRole()
        {
            try
            {

                IdentityUserRole<string> identityUserRole = null;

                string userName = Request.Form["UserName"];
                string role = Request.Form["Role"];

                identityUserRole = _context.UserRoles.FirstOrDefault(x => x.RoleId == role && x.UserId == userName);
                if (identityUserRole != null)
                {
                    TempData["Message"] = "the user already has that roles";
                }
                else
                {
                    identityUserRole = new IdentityUserRole<string>();
                    identityUserRole.UserId = userName;
                    identityUserRole.RoleId = role;
                    // TODO: Add insert logic here
                    _context.UserRoles.Add(identityUserRole);
                    _context.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                TempData["Message"] = "Error! Details " + ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Admin/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Delete/5
        [HttpGet]
        public ActionResult Delete(string username, string role)
        {
            IdentityUser identityUser = _context.Users.Where(x => x.Email == username).FirstOrDefault();
            IdentityRole identityRole = _context.Roles.Where(x => x.Name == role).FirstOrDefault();

            IdentityUserRole<string> identityUserRole = _context.UserRoles.Where(x => x.UserId == identityUser.Id && x.RoleId == identityRole.Id).FirstOrDefault();
            if (identityUserRole != null)
            {



                try
                {
                    _context.UserRoles.Remove(identityUserRole);
                    _context.SaveChanges();

                    TempData["Message"] = "Remove User: " + identityUser.UserName + " " +  "Role name: " + identityRole.Name;
                }
                catch (Exception ex)
                {
                    TempData["Message"] = ex;
                }
            }

            return RedirectToAction(nameof(Index));

        }

        // POST: Admin/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}