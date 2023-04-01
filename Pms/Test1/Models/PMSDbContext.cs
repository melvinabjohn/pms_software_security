using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Xml;
using Microsoft.EntityFrameworkCore;

namespace Test1.Models
{

    public class PMSDbContext : DbContext
	{
        public PMSDbContext(DbContextOptions<PMSDbContext> options)
                : base(options)
        {

        }

        public DbSet<UserDetails> UserDetails { get; set; } = null!;
        public DbSet<MasterPassword> MasterPassword { get; set; } = null!;
        public DbSet<LegacyAppData> LegacyAppData { get; set; } = null!;
        public DbSet<LegacyAppPassword> LegacyAppPassword { get; set; } = null!;

    }
}

