using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebShop.Models;

namespace WebShop.Data
{
    public class DbShop : DbContext
    {
        public DbShop(DbContextOptions<DbShop> options)
           : base(options)
        {
        }

        public DbSet<MeasuringUnit> MeasuringUnit { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<ProductCategory> ProductCategory { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderDetails> OrderDetail { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Contact> Contact { get; set; }
        public DbSet<ReviewContent> ReviewContent { get; set; }




    }
}
