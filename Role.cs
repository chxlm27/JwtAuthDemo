using Microsoft.AspNetCore.Identity;

namespace JwtAuthDemo
{
    public class Role : IdentityRole<int>
    {
        public List<User> Users { get; set; } = new List<User>();
    }
}
