using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Comman
{
    public class IdentityUserResult
    {
        public IdentityUserResult(string id, string displayName, string email, string userName)
        {
            Id = id;
            DisplayName = displayName;
            Email = email;
            UserName = userName;
        }

        public string Id { get; set; }
        public string DisplayName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string UserName { get; set; } = default!;
    }                                         
}
