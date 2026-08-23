using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Entities.Orders
{
    //infrastructureمش منفصله زي اللي في  owned من ضمن داتا الاوردر عشان كدا عملتها 
   
    public class OrderAddress
    {
        public string City { get; set; } = default!;
        public string Street { get; set; } = default!;
        public string Country { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
      
    }
}
