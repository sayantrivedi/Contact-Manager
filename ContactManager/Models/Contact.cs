using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ContactManager.Models
{
    public class Contact
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [DisplayAttribute(Name = "First Name")]
        [Required(ErrorMessage="Required!")]
        public string FirstName { get; set; }

        [DisplayAttribute(Name = "Last Name")]
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }

        [DisplayAttribute(Name = "Pin Code")]
        [DataType(DataType.PostalCode)]
        public string PostalCode { get; set; }

        [DisplayAttribute(Name = "Phone Number")]
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public string Status { get; set; }
    }
}