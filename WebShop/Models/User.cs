using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebShop.Models
{
    public class User
    {

        public User()
        {
            UserId = Guid.NewGuid();
        }

        [Key]
        public Guid UserId { get; set; }

        [Required]
        [StringLength(50,ErrorMessage = "Maxsimum  50 and minimum 3 characters are allowed", MinimumLength =3)]
        public string Name { get; set; }


        [Required]
        [StringLength(50, ErrorMessage = "Maxsimum  50 and minimum 3 characters are allowed", MinimumLength = 3)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

   

        [Required]
        public string Country { get; set; }


        [Required]
        public string City { get; set; }


        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Delivery address")]
        public string Address { get; set; }



        [Required]
        [Display(Name = "Postal Code")]
        [DataType(DataType.PostalCode)]     
        public string PostalCode { get; set; }



        [Required]
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = ("Email"))]
        [EmailAddress]
        public string Email { get; set; }



        public ICollection<Order> Orders { get; set; }

    }
}
