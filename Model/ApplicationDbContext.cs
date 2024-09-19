using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace AuthentcationServiceForTradingMarket.Model
{
        
    public class ApplicationDbContext: IdentityDbContext<ApplicationUser>{

        public DbSet<OtpAndEmail> OtpsAndEmails { get; set; }
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //   modelBuilder.ApplyConfiguration(new CourseConfiguration()); Not best practice
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}   
