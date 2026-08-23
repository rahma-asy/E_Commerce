using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.DTOs.Identity
{
    public class AddressDto
    {
        [Required]//will use it in update
        public string City { get; set; } = default!;
        [Required]
        public string Street { get; set; } = default!;
        [Required]
        public string Country { get; set; } = default!;
        [Required]
        public string FirstName { get; set; } = default!;
        [Required]
        public string LastName { get; set; } = default!;
    }
}
