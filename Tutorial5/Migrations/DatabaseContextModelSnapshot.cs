using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Tutorial5.Data;

#nullable disable
namespace Tutorial5.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "8.0.15");

            // Medicament
            modelBuilder.Entity("Tutorial5.Models.Medicament", b =>
            {
                b.Property<int>("IdMedicament")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");
                b.Property<string>("Name")
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnType("nvarchar(100)");
                b.Property<string>("Description")
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnType("nvarchar(100)");
                b.Property<string>("Type")
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnType("nvarchar(100)");

                b.HasKey("IdMedicament");
                b.ToTable("Medicament");
            });

            // Doctor
            modelBuilder.Entity("Tutorial5.Models.Doctor", b =>
            {
                b.Property<int>("IdDoctor")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");
                b.Property<string>("FirstName")
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnType("nvarchar(100)");
                b.Property<string>("LastName")
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnType("nvarchar(100)");
                b.Property<string>("Email")
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnType("nvarchar(100)");

                b.HasKey("IdDoctor");
                b.ToTable("Doctor");
            });

            // Patient
            modelBuilder.Entity("Tutorial5.Models.Patient", b =>
            {
                b.Property<int>("IdPatient")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");
                b.Property<string>("FirstName")
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnType("nvarchar(100)");
                b.Property<string>("LastName")
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnType("nvarchar(100)");
                b.Property<DateTime>("Birthdate")
                    .HasColumnType("date");

                b.HasKey("IdPatient");
                b.ToTable("Patient");
            });

            // Prescription
            modelBuilder.Entity("Tutorial5.Models.Prescription", b =>
            {
                b.Property<int>("IdPrescription")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");
                b.Property<DateTime>("Date")
                    .HasColumnType("date");
                b.Property<DateTime>("DueDate")
                    .HasColumnType("date");
                b.Property<int>("IdDoctor")
                    .HasColumnType("int");
                b.Property<int>("IdPatient")
                    .HasColumnType("int");

                b.HasKey("IdPrescription");
                b.HasIndex("IdDoctor");
                b.HasIndex("IdPatient");
                b.ToTable("Prescription");
            });

            // Prescription_Medicament (join-entity)
            modelBuilder.Entity("Tutorial5.Models.PrescriptionMedicament", b =>
            {
                b.Property<int>("IdMedicament")
                    .HasColumnType("int");
                b.Property<int>("IdPrescription")
                    .HasColumnType("int");
                b.Property<int>("Dose")
                    .HasColumnType("int");
                b.Property<string>("Details")
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnType("nvarchar(100)");

                b.HasKey("IdMedicament", "IdPrescription");
                b.HasIndex("IdPrescription");
                b.ToTable("Prescription_Medicament");
            });

            // Навигации и связи
            modelBuilder.Entity("Tutorial5.Models.Prescription", b =>
            {
                b.HasOne("Tutorial5.Models.Doctor")
                    .WithMany()
                    .HasForeignKey("IdDoctor")
                    .OnDelete(DeleteBehavior.Cascade);

                b.HasOne("Tutorial5.Models.Patient")
                    .WithMany()
                    .HasForeignKey("IdPatient")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity("Tutorial5.Models.PrescriptionMedicament", b =>
            {
                b.HasOne("Tutorial5.Models.Medicament")
                    .WithMany()
                    .HasForeignKey("IdMedicament")
                    .OnDelete(DeleteBehavior.Cascade);

                b.HasOne("Tutorial5.Models.Prescription")
                    .WithMany()
                    .HasForeignKey("IdPrescription")
                    .OnDelete(DeleteBehavior.Cascade);
            });
#pragma warning restore 612, 618
        }
    }
}
