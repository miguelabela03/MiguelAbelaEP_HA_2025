using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DataContext
{
    // The context class is an abstract representation of the database
    public class PollDbContext : IdentityDbContext<IdentityUser>
    {
        public PollDbContext(DbContextOptions<PollDbContext> options) : base(options)
        {

        }

        public DbSet<Poll> Polls { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}
