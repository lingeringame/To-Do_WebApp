using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using To_Do.Models;

namespace To_Do.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<ToDoTask> TodoTasks { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
