using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using To_Do.Areas.Identity.Data;
using To_Do.Models;

namespace To_Do.Data
{
    public class ApplicationDbContext : IdentityDbContext<To_DoUser>
    {
        public DbSet<ToDoTask> TodoTasks { get; set; }
        public DbSet<Folder> Folders { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ToDoTask>()
                .Property(t => t.CreatedOn)
                .HasDefaultValueSql("NOW(6)");
            base.OnModelCreating(builder);
        }
    }
}
