using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Application.Migrations;

namespace Application.Models
{
    public class CompanyDbContext:DbContext
    {
        public CompanyDbContext() : base("MyConnectionString") 
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<CompanyDbContext, Configuration>());
        }
        public DbSet<Brand>Brands { get; set; } 
    }
}