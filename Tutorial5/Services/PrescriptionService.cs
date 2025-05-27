using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Tutorial5.Data;
using Tutorial5.DTOs;
using Tutorial5.Models;

namespace Tutorial5.Services
{
    public class PrescriptionService : IPrescriptionService
    {
        private readonly DatabaseContext _ctx;

        public PrescriptionService(DatabaseContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<int> CreatePrescriptionAsync(PatientWithMedicamentsDto dto)
        {
            var prDto = dto.Prescriptions?.FirstOrDefault()
                        ?? throw new ValidationException("Prescription list cannot be empty.");

            if (prDto.Medicaments == null || prDto.Medicaments.Count == 0)
                throw new ValidationException("Prescription must include at least one medicament.");

            if (prDto.Medicaments.Count > 10)
                throw new ValidationException("A prescription can include a maximum of 10 medications.");

            if (prDto.DueDate < prDto.Date)
                throw new ValidationException("DueDate must be greater than or equal to Date.");

            Patient patient = null;
            if (dto.IdPatient.HasValue)
            {
                patient = await _ctx.Patients.FirstOrDefaultAsync(p => p.IdPatient == dto.IdPatient.Value);
            }
            if (patient == null)
            {
                patient = new Patient
                {
                    FirstName = dto.FirstName,
                    LastName  = dto.LastName,
                    BirthDate = dto.BirthDate
                };
                _ctx.Patients.Add(patient);
                await _ctx.SaveChangesAsync();
            }

            var doctor = await _ctx.Doctors
                .FindAsync(prDto.Doctor.IdDoctor);
            if (doctor == null)
                throw new KeyNotFoundException($"Doctor with Id {prDto.Doctor.IdDoctor} not found.");

            var prescription = new Prescription
            {
                Date      = prDto.Date,
                DueDate   = prDto.DueDate,
                PatientId = patient.IdPatient,
                DoctorId  = doctor.IdDoctor
            };
            _ctx.Prescriptions.Add(prescription);
            await _ctx.SaveChangesAsync();

            foreach (var m in prDto.Medicaments)
            {
                var med = await _ctx.Medicaments.FindAsync(m.IdMedicament);
                if (med == null)
                    throw new KeyNotFoundException($"Medicament with Id {m.IdMedicament} not found.");

                var pm = new PrescriptionMedicament
                {
                    PrescriptionId = prescription.Id,
                    MedicamentId   = med.IdMedicament,
                    Dose            = m.Dose,
                    Details         = m.Description
                };
                _ctx.PrescriptionMedicaments.Add(pm);
            }
            await _ctx.SaveChangesAsync();

            return patient.IdPatient;
        }
    }
}
