using Enthro.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Enthro.Persistence.DbContexts
{
    public class EnthroDbContext : IdentityDbContext<User, Role, Guid>
    {
        public EnthroDbContext(
            DbContextOptions<EnthroDbContext> options
        ) : base(options)
        {
            //
        }

        public DbSet<Indicator> Indicators { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            IEnumerable<IMutableForeignKey> foreignKeys = modelBuilder.Model
                .GetEntityTypes()
                .SelectMany(et => et.GetForeignKeys());

            foreach (IMutableForeignKey foreignKey in foreignKeys)
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }

            modelBuilder.Entity<User>()
                .ToTable("Users");

            modelBuilder.Entity<Role>()
                .ToTable("Roles");

            modelBuilder.Entity<IdentityUserLogin<Guid>>()
                .ToTable("UserLogins");

            modelBuilder.Entity<IdentityUserClaim<Guid>>()
                .ToTable("UserClaims");

            modelBuilder.Entity<IdentityUserToken<Guid>>()
                .ToTable("UserTokens");

            modelBuilder.Entity<IdentityUserRole<Guid>>()
                .ToTable("UserRoles");

            modelBuilder.Entity<IdentityRoleClaim<Guid>>()
                .ToTable("RoleClaims");
        }
    }
}
