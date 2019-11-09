using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MOD.AdminLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MOD.AdminLibrary
{
    class AdminContext : IdentityDbContext
    {
        public AdminContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Skill> Skills { get; set; }
    }
}
