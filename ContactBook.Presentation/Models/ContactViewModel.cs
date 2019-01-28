using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContactBook.Presentation.Models
{
    public class ContactViewModel
    {
        public int ContactId { get; set; }
        public string ContactName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        public string PhoneNo { get; set; }

    }
}