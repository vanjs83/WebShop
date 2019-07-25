using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebShop.Models
{
    public class Cart
    {

        public Guid ÇartId { get; set; } = Guid.NewGuid();

        public int Quantity { get; set; }

        public Product Product { get; set; }

    }
}
