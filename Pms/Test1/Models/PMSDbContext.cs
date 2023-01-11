using System;
using System.Collections.Generic;
using System.Xml;
using Microsoft.EntityFrameworkCore;

namespace Test1.Models
{

    //public interface IDbContext : IDisposable
    //{
    //    DbContext Instance { get; }
    //}

    //public interface IApplicationDbContext : IDbContext
    //{
    //    DbSet<UserDetails> UserDetails { get; set; }
    //    DbSet<MasterPassword> MasterPassword { get; set; } 
    //    DbSet<LegacyAppData> LegacyAppData { get; set; }
    //    DbSet<LegacyAppPassword> LegacyAppPassword { get; set; }
    //}

    public class PMSDbContext : DbContext//, IApplicationDbContext
	{
        public PMSDbContext(DbContextOptions<PMSDbContext> options)
                : base(options)
        {

        }
        //public DbContext Instance => this;


        public DbSet<UserDetails> UserDetails { get; set; } = null!;
        public DbSet<MasterPassword> MasterPassword { get; set; } = null!;
        public DbSet<LegacyAppData> LegacyAppData { get; set; } = null!;
        public DbSet<LegacyAppPassword> LegacyAppPassword { get; set; } = null!;



    }
}

