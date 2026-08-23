using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Identity.Entities
{
    public class ApplicationUser:IdentityUser
    {
        public string DisplayName { get; set; } = default!;
        public Address? Address { get; set; }//optional
    }
}
