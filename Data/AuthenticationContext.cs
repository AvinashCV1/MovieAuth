using Microsoft.EntityFrameworkCore;
using MovieAuth.Models;
using MvcMovie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieAuth.Data
{
    public class AuthenticationContext : DbContext
    {
        public AuthenticationContext(DbContextOptions<AuthenticationContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Movie> Movie { get; set; }
    }
}
