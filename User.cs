using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace JwtAuthDemo
{
    public class User : IdentityUser<int>
    {
        public List<Role> Roles { get; set; } = new List<Role>();
    }
}
