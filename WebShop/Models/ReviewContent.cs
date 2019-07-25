using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebShop.Models
{
    public class ReviewContent
    {
        public Guid ReviewContentId { get; set; }

        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Location { get; set; }

        public int Rating { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public Guid ProductId { get; set; }

        

    }
}
