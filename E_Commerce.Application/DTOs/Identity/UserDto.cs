using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.DTOs.Identity
{
    public class UserDto
    {
    
        public string DisplayName { get; set; } = default!;
        public string Token { get; set; } = default!;
        public string Email { get; set; } = default!;
    }                                        
}
