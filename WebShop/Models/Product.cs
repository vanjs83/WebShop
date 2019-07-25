using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebShop.Models
{
    [Table("Product")]
    public class Product
    {


        public Guid ProductId { get; set; }

        [Required(ErrorMessage ="Name product is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Maxsimum  50 and minimum 3 characters are allowed")]
        public string Name { get; set; }

        [Display(Name ="Created date")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage ="Created date is required")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Modified date")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Created date is required")]
        public DateTime ModifiedDate { get; set; }


        [Required(ErrorMessage  = "Quantity is required")]
        public int Quantity { get; set; }


        public bool Available { get; set; }

        
        [Required(ErrorMessage ="Price is Required")]
        public decimal Price { get; set; }


        public byte[] Image { get; set; }

        
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }


        public Guid? MeasuringUnitId { get; set; }
        public MeasuringUnit MeasuringUnit { get; set; }


        public virtual ICollection<OrderDetails> OrderDetail { get; set; }
        public virtual ICollection<ProductCategory> CategoryProduct { get; set; }
    }
}
