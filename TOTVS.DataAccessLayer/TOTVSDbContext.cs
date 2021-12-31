using Microsoft.EntityFrameworkCore;
using TOTVS.Domain.Entities;

#nullable disable

namespace TOTVS.Persistence
{
    public partial class TOTVSDbContext : DbContext
    {
        public TOTVSDbContext()
        {
        }

        public TOTVSDbContext(DbContextOptions<TOTVSDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Profile> Profiles { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "en_US.utf8");

            modelBuilder.Entity<Profile>(entity =>
            {
                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Profiles)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("profiles_users_identifier_fk");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Identifier)
                    .HasName("users_pk");

                entity.HasIndex(e => e.Email, "users_email_uindex")
                    .IsUnique();

                entity.HasIndex(e => e.Identifier, "users_identifier_uindex")
                    .IsUnique();

                entity.Property(e => e.Identifier).HasDefaultValueSql("gen_random_uuid()");

                entity.Property(e => e.CreatedIn).HasDefaultValueSql("now()");

                entity.Property(e => e.Email).IsRequired();

                entity.Property(e => e.Hash).IsRequired();

                entity.Property(e => e.Name).IsRequired();
            });
        }

    }
}
