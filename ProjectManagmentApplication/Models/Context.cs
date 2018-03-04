using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ProjectManagementApplication.Models
{
    public class Context : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Board> Boards { get; set; }
        public DbSet<Column> Columns { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<TeamMember> TeamMembers { get; set; }
        public DbSet<FavoriteBoard> FavoriteBoards { get; set; }


        public Context() : base("databasemdf")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<Context>(null);

            //modelBuilder.Entity<User>().HasMany(t => t.BoardsAdmin).WithRequired(a => a.User).WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }

        
    }
}