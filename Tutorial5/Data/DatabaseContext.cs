using Microsoft.EntityFrameworkCore;
using Tutorial5.Models;

namespace Tutorial5.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {}
        
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<Medicament> Medicaments { get; set; }
        public DbSet<PrescriptionMedicament> PrescriptionMedicaments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>().ToTable("Patient");
            modelBuilder.Entity<Doctor>().ToTable("Doctor");
            modelBuilder.Entity<Medicament>().ToTable("Medicament");
            modelBuilder.Entity<Prescription>().ToTable("Prescription");
            modelBuilder.Entity<PrescriptionMedicament>().ToTable("Prescription_Medicament");
            
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Prescription>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity
                    .HasOne(e => e.Patient)
                    .WithMany(p => p.Prescriptions)
                    .HasForeignKey(e => e.PatientId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity
                    .HasOne(e => e.Doctor)
                    .WithMany(d => d.Prescriptions)
                    .HasForeignKey(e => e.DoctorId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.Date)
                      .IsRequired();

                entity.Property(e => e.DueDate)
                      .IsRequired();
            });

            modelBuilder.Entity<PrescriptionMedicament>(entity =>
            {
                entity.HasKey(e => new { e.PrescriptionId, e.MedicamentId });

                entity
                    .HasOne(e => e.Prescription)
                    .WithMany(p => p.PrescriptionMedicaments)
                    .HasForeignKey(e => e.PrescriptionId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity
                    .HasOne(e => e.Medicament)
                    .WithMany(m => m.PrescriptionMedicaments)
                    .HasForeignKey(e => e.MedicamentId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.Dose)
                      .IsRequired();

                entity.Property(e => e.Details)
                      .IsRequired()
                      .HasMaxLength(200);
            });

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.HasKey(e => e.IdPatient);
                entity.Property(e => e.FirstName)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.Property(e => e.LastName)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.Property(e => e.BirthDate)
                      .IsRequired()
                      .HasColumnType("date");
            });

            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.HasKey(e => e.IdDoctor);
                entity.Property(e => e.FirstName)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.Property(e => e.LastName)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.Property(e => e.Email)
                      .IsRequired()
                      .HasMaxLength(100);
            });

            modelBuilder.Entity<Medicament>(entity =>
            {
                entity.HasKey(e => e.IdMedicament);
                entity.Property(e => e.Name)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.Property(e => e.Description)
                      .IsRequired()
                      .HasMaxLength(200);
                entity.Property(e => e.Type)
                      .IsRequired()
                      .HasMaxLength(100);
            });
        }
    }
}