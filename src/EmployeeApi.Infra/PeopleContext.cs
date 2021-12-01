using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace EmployeeApi.Infra
{
    public partial class PeopleContext : DbContext
    {
        public PeopleContext()
        {
        }

        public PeopleContext(DbContextOptions<PeopleContext> options)
            : base(options)
        {
        }

        public virtual DbSet<VPersonDepAndBu> VPersonDepAndBus { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");

            modelBuilder.Entity<VPersonDepAndBu>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vPersonDepAndBU");

                entity.Property(e => e.Ad).HasMaxLength(100);

                entity.Property(e => e.BusinessUnit)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Department)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.NameFirstRu).HasMaxLength(100);

                entity.Property(e => e.NameLastRu).HasMaxLength(100);

                entity.Property(e => e.Position)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
