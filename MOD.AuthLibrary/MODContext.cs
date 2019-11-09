using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MOD.AuthLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MOD.AuthLibrary
{
    public class MODContext : IdentityDbContext
    {
        public MODContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>(r => r.HasData(new IdentityRole
            {
                Id = "1",
                Name = "Admin",
                NormalizedName = "Admin"
            },
            new IdentityRole
            {
                Id = "2",
                Name = "Mentor",
                NormalizedName = "Mentor"
            },
            new IdentityRole
            {
                Id = "3",
                Name = "Student",
                NormalizedName = "Student"
            }));
            base.OnModelCreating(builder);
        }

        public DbSet<MODUser> MODUsers { get; set; }
        public DbSet<Skill> Skills { get; set; }
    }
}
