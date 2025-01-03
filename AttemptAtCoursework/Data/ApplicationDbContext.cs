﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AttemptAtCoursework.Models;

namespace AttemptAtCoursework.Data
{
    public class ApplicationDbContext : IdentityDbContext <ApplicationUser>
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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
        }
}
