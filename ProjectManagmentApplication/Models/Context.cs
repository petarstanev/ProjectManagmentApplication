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
        public DbSet<Board> Boards { get; set; }
        public DbSet<Column> Columns { get; set; }
        //public DbSet<Standard> Standards { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<Context>(null);
            base.OnModelCreating(modelBuilder);
        }

        public System.Data.Entity.DbSet<ProjectManagmentApplication.Models.Task> Tasks { get; set; }

        public System.Data.Entity.DbSet<ProjectManagmentApplication.Models.Comment> Comments { get; set; }
    }
}