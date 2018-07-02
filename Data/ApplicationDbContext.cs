using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication5.Models;

namespace WebApplication5.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Models.Task> Tasks { get; set; }
        public DbSet<Protocol> Protocols { get; set; }
        public DbSet<ApplicationUser_has_Task> ApplicationUser_Has_Tasks { get; set; }
        public DbSet<ApplicationUser_has_Protocol> ApplicationUser_Has_Protocols { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>().ToTable("ApplicationsUser");
            builder.Entity<Models.Task>().ToTable("Task");
            builder.Entity<Protocol>().ToTable("Protocol");
            builder.Entity<Organization>().ToTable("Organization");
            builder.Entity<ApplicationUser_has_Task>().ToTable("ApplicationUser_has_Task");
            builder.Entity<ApplicationUser_has_Protocol>().ToTable("ApplicationUser_has_Protocol");
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);


            builder.Entity<Protocol>()
                   .HasOne(c => c.Organization)
                   .WithMany(e => e.Protocols)
                   .HasForeignKey(p => p.OrganizationID)
                   .IsRequired();

            builder.Entity<Models.Task>()
                   .HasOne(c => c.Protocol)
                   .WithMany(e => e.Tasks)
                   .HasForeignKey(p => p.ProtocolID)
                   .IsRequired();
            

            builder.Entity<ApplicationUser_has_Task>()
                   .HasKey(t => new { t.TaskID, t.AppID });
            builder.Entity<ApplicationUser_has_Task>()
                   .HasOne(pt => pt.Task)
                    .WithMany(p => p.ApplicationUser_has_Tasks)
                   .HasForeignKey(pt => pt.TaskID);
            builder.Entity<ApplicationUser_has_Task>()
                   .HasOne(pt => pt.ApplicationUser)
                   .WithMany(t => t.ApplicationUser_has_Tasks)
                   .HasForeignKey(pt => pt.AppID)
                   .HasPrincipalKey(pt => pt.AppID);


            builder.Entity<ApplicationUser_has_Protocol>()
                   .HasKey(t => new { t.ProtocolID, t.AppID });
            builder.Entity<ApplicationUser_has_Protocol>()
                   .HasOne(pt => pt.Protocol)
                .WithMany(p => p.ApplicationUser_has_Protocols)
                   .HasForeignKey(pt => pt.ProtocolID);
            builder.Entity<ApplicationUser_has_Protocol>()
                   .HasOne(pt => pt.ApplicationUser)
                   .WithMany(t => t.ApplicationUser_has_Protocols)
                   .HasForeignKey(pt => pt.AppID)
                   .HasPrincipalKey(pt => pt.AppID);

      
        }

        public DbSet<WebApplication5.Models.Organization> Organization { get; set; }
    }
}
