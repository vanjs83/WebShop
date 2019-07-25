using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebShop.Models
{
    public class OrderDetails
    {

        public OrderDetails()
        {
            OrderDetailsId = Guid.NewGuid();
        }

        [Key]
        public Guid OrderDetailsId { get; set; } = Guid.NewGuid();

        public int Quantity { get; set; }

        [Display(Name = "Price for item")]
        public decimal PriceForItem { get; set; }
 

        public Guid? ProductId { get; set; }
        public  Product Product { get; set; }

    
        public Guid? OrderId { get; set; }
        public Order Order { get; set; }


    }
}
