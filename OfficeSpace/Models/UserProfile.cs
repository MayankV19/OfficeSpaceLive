using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OfficeSpace.Models
{
    public class UserProfile
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public List<string> CompanyName { get; set; }
        public string Position { get; set; }
        public string CompanyEmail { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }    
        public string Password { get; set; }
        public string UserRole { get; set; }
    }
}