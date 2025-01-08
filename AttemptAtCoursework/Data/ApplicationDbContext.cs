using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AttemptAtCoursework.Models;
using Microsoft.AspNetCore.Identity;

namespace AttemptAtCoursework.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<string>, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<AttemptAtCoursework.Models.Vacancy> Vacancy { get; set; } = default!;
        public DbSet<AttemptAtCoursework.Models.WorkPosition> WorkPosition { get; set; } = default!;
        public DbSet<AttemptAtCoursework.Models.Company> Company { get; set; } = default!;
        public DbSet<AttemptAtCoursework.Models.Resume> Resume { get; set; } = default!;
        public DbSet<AttemptAtCoursework.Models.ApplicationUser> ApplicationUser { get; set; } = default!;

        public DbSet<AttemptAtCoursework.Models.ApplicationUser> Role { get; set; } = default!;

        public DbSet<AttemptAtCoursework.Models.ApplicationUser> UserRole { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
        }
}
