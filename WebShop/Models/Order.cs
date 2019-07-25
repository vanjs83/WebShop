using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebShop.Models
{
    public class Order
    {
        public Order()
        {
            OrderId = Guid.NewGuid();
        }   




        public Guid OrderId { get; set; }


        [Display(Name = "Date Create")]
        public DateTime DateCreate { get; set; }


        [Display(Name = "Date Delivery")]
        public DateTime DateDelivery { get; set; }

        public bool IsDelivery { get; set; }

     
        public Guid UserId { get; set; }
        public User User { get; set; }

        public ICollection<OrderDetails> OrderDetails { get; set; }

    }
}
