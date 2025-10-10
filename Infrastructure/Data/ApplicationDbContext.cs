using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MovieWebApp.Domain.Entities;
using System.Collections.Generic;

namespace MovieWebApp.Infrastructure.Data
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Movie> movies { get; set; }
        public DbSet<Genre> genres { get; set; }
        public DbSet<User> users { get; set; }
        public DbSet<Favorite> favorites { get; set; }
    }
}
