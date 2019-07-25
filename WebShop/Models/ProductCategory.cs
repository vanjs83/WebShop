using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebShop.Models
{
    public class ProductCategory
    {

        public ProductCategory()
        {
            ProductCategoryId = Guid.NewGuid();
        }

        [Key]
        public Guid ProductCategoryId { get; set; }


        public Guid? ProductId { get; set; }
        public Product Product { get; set; }


        public Guid? CategoryId { get; set; }
        public Category Category { get; set; }


        [DataType(DataType.Date)]
        [Display(Name = "Created date")]
        public DateTime CreatedDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Modified date")]
        public DateTime ModifiedDate { get; set; }

        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        [Display(Name = "Modified By")]
        public string ModifiedBy { get; set; }


    }
}
