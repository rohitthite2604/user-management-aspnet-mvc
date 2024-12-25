using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Application.Models
{
    public class DataContext : DbContext
    {
        public DataContext() : base("name=DefaultConnection") { }
        public DbSet<Product> Products { get; set; }
    }
}