using Microsoft.EntityFrameworkCore;

#nullable disable

namespace EmployeeApi.Infra
{
    public partial class PracticeManagementContext : DbContext
    {
        public PracticeManagementContext()
        {
        }

        public PracticeManagementContext(DbContextOptions<PracticeManagementContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Employee> Employees { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            /*if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=RUMOS0929.ATREMA.DELOITTE.COM;Database=PracticeManagementQA;MultipleActiveResultSets=true;Integrated Security=SSPI;");
            }*/
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("Employee");

                entity.HasIndex(e => e.AccountName, "IX_Employee_AccountName");

                entity.HasIndex(e => e.DisplayName, "IX_Employee_DisplayName");

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

                entity.Property(e => e.AccountName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Department)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.DisplayName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.DisplayNameLocal).HasMaxLength(255);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Grade)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.ManagerId).HasColumnName("ManagerID");

                entity.Property(e => e.NameFirstLocal).HasMaxLength(100);

                entity.Property(e => e.NameLastLocal).HasMaxLength(155);

                entity.Property(e => e.ObjectSid)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("objectSID");

                entity.Property(e => e.OfficeCity).HasMaxLength(100);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d.Manager)
                    .WithMany(p => p.InverseManager)
                    .HasForeignKey(d => d.ManagerId)
                    .HasConstraintName("FK__Employee__Manage__3A4CA8FD");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
