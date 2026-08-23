using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.DTOs.Identity
{
    public class LoginDto
    {
        [Required]
        public string Password { get; set; } = default!;
        [Required,EmailAddress]
        public string Email { get; set; } = default!;
    }
}
