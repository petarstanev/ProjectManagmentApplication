using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ProjectManagmentApplication.Models
{
    public class Context : DbContext
    {
        public DbSet<User> Users { get; set; }
        //public DbSet<Standard> Standards { get; set; }
    }
}