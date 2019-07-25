using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebShop.Models
{
    public class MeasuringUnit
    {

        public MeasuringUnit()
        {
            MeasuringUnitId = Guid.NewGuid();
        }

        [Key]
        public Guid MeasuringUnitId { get; set; }

        [Display(Name = "Name measuring unit")]
        [Required(ErrorMessage = "Name is required!")]
        public string Name { get; set; }


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


        [Required(ErrorMessage = "Unit is required!")]
        public string Unit { get; set; }      

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public ICollection<Product> Products { get; set; }

    }
}
