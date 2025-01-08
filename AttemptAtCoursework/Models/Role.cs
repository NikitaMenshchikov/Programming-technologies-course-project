using Microsoft.AspNetCore.Identity;

namespace AttemptAtCoursework.Models
{
    public class Role : IdentityRole
    {
        public string UserId { get; set; }
        public string RoleName { get; set; }
    }
}
