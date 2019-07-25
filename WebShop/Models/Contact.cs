using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebShop.Models
{
    [Table(name:("Contact"))]
    public class Contact
    {
        public Contact()
        {
            ContactId = Guid.NewGuid();
        }

        public Guid ContactId { get; set; }

        public string Name  { get; set; }

        public string Email { get; set; }

        public string Subject { get; set; }

        public string Message { get; set; }

        public bool Updates { get; set; }

    }
}
