using KASHOP.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryTranslation> CategoryTranslations { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductTranslation> ProductTranslations { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
            IHttpContextAccessor httpContextAccessor
            )
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>().ToTable("Users");
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");


            builder.Entity<Category>().HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.CreatedBy)
                .OnDelete(DeleteBehavior.NoAction);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<BaseModel>();

            var currentuserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            foreach (var entityEntry in entries)
            {
                if (entityEntry.State == EntityState.Added)
                {
                    entityEntry.Property(e => e.CreatedBy).CurrentValue = currentuserId;
                    entityEntry.Property(e => e.CreatedAt).CurrentValue = DateTime.UtcNow;
                }
                else if (entityEntry.State == EntityState.Modified)
                {
                    entityEntry.Property(e => e.UpdatedBy).CurrentValue = currentuserId;
                    entityEntry.Property(e => e.UpdatedAt).CurrentValue = DateTime.UtcNow;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }


        public override int SaveChanges()
        {
            var entries = ChangeTracker.Entries<BaseModel>();

            var currentuserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            foreach (var entityEntry in entries)
            {
                if (entityEntry.State == EntityState.Added)
                {
                    entityEntry.Property(e => e.CreatedBy).CurrentValue = currentuserId;
                    entityEntry.Property(e => e.CreatedAt).CurrentValue = DateTime.UtcNow;
                }
                else if (entityEntry.State == EntityState.Modified)
                {
                    entityEntry.Property(e => e.UpdatedBy).CurrentValue = currentuserId;
                    entityEntry.Property(e => e.UpdatedAt).CurrentValue = DateTime.UtcNow;
                }
            }

            return base.SaveChanges();
        }
        //public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        //{
        //    var entries = ChangeTracker.Entries<BaseModel>()
        //        .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
        //        .ToList();

        //    // ✅ مهم: seeding للـ Roles ما فيه BaseModel أصلاً، فارجع بدون ما تلمس HttpContext
        //    if (entries.Count == 0)
        //        return base.SaveChangesAsync(cancellationToken);

        //    var currentUserId = _httpContextAccessor?.HttpContext?.User?
        //        .FindFirstValue(ClaimTypes.NameIdentifier);

        //    foreach (var entityEntry in entries)
        //    {
        //        if (entityEntry.State == EntityState.Added)
        //        {
        //            entityEntry.Property(e => e.CreatedAt).CurrentValue = DateTime.UtcNow;
        //            if (currentUserId != null)
        //                entityEntry.Property(e => e.CreatedBy).CurrentValue = currentUserId;
        //        }
        //        else if (entityEntry.State == EntityState.Modified)
        //        {
        //            entityEntry.Property(e => e.UpdatedAt).CurrentValue = DateTime.UtcNow;
        //            if (currentUserId != null)
        //                entityEntry.Property(e => e.UpdatedBy).CurrentValue = currentUserId;
        //        }
        //    }

        //    return base.SaveChangesAsync(cancellationToken);
        //}

        //public override int SaveChanges()
        //{
        //    var entries = ChangeTracker.Entries<BaseModel>()
        //        .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
        //        .ToList();

        //    if (entries.Count == 0)
        //        return base.SaveChanges();

        //    var currentUserId = _httpContextAccessor?.HttpContext?.User?
        //        .FindFirstValue(ClaimTypes.NameIdentifier);

        //    foreach (var entityEntry in entries)
        //    {
        //        if (entityEntry.State == EntityState.Added)
        //        {
        //            entityEntry.Property(e => e.CreatedAt).CurrentValue = DateTime.UtcNow;
        //            if (currentUserId != null)
        //                entityEntry.Property(e => e.CreatedBy).CurrentValue = currentUserId;
        //        }
        //        else if (entityEntry.State == EntityState.Modified)
        //        {
        //            entityEntry.Property(e => e.UpdatedAt).CurrentValue = DateTime.UtcNow;
        //            if (currentUserId != null)
        //                entityEntry.Property(e => e.UpdatedBy).CurrentValue = currentUserId;
        //        }
        //    }

        //    return base.SaveChanges();
        //}
    }
}
