using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebShop.Models
{
    public class Category
    {

        public Category()
        {
            CategoryId = Guid.NewGuid();
        }


        [Key]
        public Guid CategoryId { get; set; }

        [Display(Name = "Category name")]
        [Required (ErrorMessage ="Category Name is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Maxsimum  50 and minimum 3 characters are allowed")]
        public string Name { get; set; }

        [DataType(DataType.Date)]
        [Display(Name= "Created date")]
        public DateTime CreatedDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name= "Modified date")]
        public DateTime ModifiedDate { get; set; }

        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        [Display(Name = "Modified By")]
        public string ModifiedBy { get; set; }

        [DataType(DataType.MultilineText)]
        [StringLength(50)]
        public string Description { get; set; }



        public ICollection<ProductCategory> ProductCategory { get; set; }
    }
}
