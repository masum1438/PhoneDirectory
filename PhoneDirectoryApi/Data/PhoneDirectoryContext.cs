using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PhoneDirectoryApi.Models.Domain;

namespace PhoneDirectoryApi.Data
{
    public class PhoneDirectoryContext : IdentityDbContext<ApplicationUser>
    {
        public PhoneDirectoryContext(DbContextOptions<PhoneDirectoryContext> options)
            : base(options)
        {
        }

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<AutoDeleteSettings> AutoDeleteSettings { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Contact>(entity =>
            {
                entity.Property(c => c.Name).IsRequired();
                entity.Property(c => c.PhoneNumber).IsRequired();
                entity.Property(c => c.Status).HasDefaultValue(true);
                entity.Property(c => c.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            });
            builder.Entity<AutoDeleteSettings>()
    .Property(e => e.Id)
    .ValueGeneratedNever();
        }
    }
}
