using AuthApi.Models.Application;
using AuthApi.Models.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthApi.Models
{
    public class AuthApiContext : IdentityDbContext<User>
    {
        //public class ApplicationDbContext //: IdentityDbContext<ApplicationUser>
        //{
        public AuthApiContext(DbContextOptions<AuthApiContext> options) : base(options)
        {

        }

        public DbSet<Application.Application> Applications { get; set; }
        public DbSet<UserApplication> UserApplications { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>()
                   .HasMany(e => e.Applications)
                   .WithMany(e => e.Users)
                   .UsingEntity<UserApplication>();
                //l => l.HasOne<Application.Application>().WithMany().HasForeignKey(e => e.ApplicationId),
                //r => r.HasOne<User>().WithMany().HasForeignKey(e => e.UserId));

            base.OnModelCreating(builder);
        }
        //}
    }
}