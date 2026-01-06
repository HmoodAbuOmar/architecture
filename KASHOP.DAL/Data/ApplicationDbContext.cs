// هذا الملف هو كلاس ApplicationDbContext في ASP.NET Core يستخدم Entity Framework Core كطبقة البيانات (Data Access Layer).
// الكلاس يرث من IdentityDbContext<ApplicationUser>، يعني فيه دعم للهوية والمستخدمين وإدارة الصلاحيات من مايكروسوفت.
// دعني أوضح كل جزء فيه بالتفصيل وبالعربي:

using KASHOP.DAL.Models; // استيراد الموديلات من المشروع
using Microsoft.AspNetCore.Http; // للوصول للـ HttpContext (المستخدم الحالي وغيره)
using Microsoft.AspNetCore.Identity; // هويات المستخدمين
using Microsoft.AspNetCore.Identity.EntityFrameworkCore; // DbContext الخاص بإدارة الهوية
using Microsoft.EntityFrameworkCore; // EF Core
using Microsoft.EntityFrameworkCore.ChangeTracking; // تتبع التغييرات
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims; // لمعرفة من هو المستخدم الحالي
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

        public DbSet<ProductImage> ProductImages { get; set; }

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
                .OnDelete(DeleteBehavior.NoAction); // إذا حذف المستخدم لا تحذف التصنيفات
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<BaseModel>();

            var currentuserId = _httpContextAccessor?.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

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

            var currentuserId = _httpContextAccessor?.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

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
    }
}

