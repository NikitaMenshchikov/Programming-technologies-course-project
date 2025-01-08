using Microsoft.AspNetCore.Identity;

namespace AttemptAtCoursework.Models
{
    public class UserRole : IdentityUserRole<String>
    {
        public virtual ApplicationUser User { get; set; }

        public virtual Role Role { get; set; }
    }
}
