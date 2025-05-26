using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Tutorial5.Data;
using Tutorial5.Models;
using Tutorial5.DTOs;
using Tutorial5.Services;

namespace Tutorial5.Controllers
{
    public class DbService : IDbService
    {
        private readonly DatabaseContext _context;
        public DbService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<PrescriptionResultDto> CreatePrescriptionAsync(PatientWithPrescriptionsDto request)
        {
            var meds = request.Prescriptions.SelectMany(pr => pr.Medicaments).ToList();
            if (meds.Count > 10)
                return PrescriptionResultDto.Failure("A prescription can include a maximum of 10 medications.");

            var prDto = request.Prescriptions.First();
            if (prDto.DueDate < prDto.Date)
                return PrescriptionResultDto.Failure("DueDate must be greater than or equal to Date.");

            var patient = await _context.Patients
                .FirstOrDefaultAsync(p => p.Id == request.IdPatient);
            if (patient == null)
            {
                patient = new Patient
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    BirthDate = request.BirthDate
                };
                _context.Patients.Add(patient);
                await _context.SaveChangesAsync();
            }

            var docDto = prDto.Doctor;
            var doctor = await _context.Doctors.FindAsync(docDto.IdDoctor);
            if (doctor == null)
                return PrescriptionResultDto.Failure("Doctor not found.");

            var prescription = new Prescription
            {
                Date = prDto.Date,
                DueDate = prDto.DueDate,
                PatientId = patient.Id,
                DoctorId = doctor.Id
            };
            _context.Prescriptions.Add(prescription);
            await _context.SaveChangesAsync();

            foreach (var m in prDto.Medicaments)
            {
                var med = await _context.Medicaments.FindAsync(m.IdMedicament);
                if (med == null)
                    return PrescriptionResultDto.Failure($"Medicament with Id {m.IdMedicament} not found.");

                var pm = new PrescriptionMedicament
                {
                    PrescriptionId = prescription.Id,
                    MedicamentId = m.IdMedicament,
                    Dose = m.Dose,
                    Details = m.Details
                };
                _context.PrescriptionMedicaments.Add(pm);
            }

            await _context.SaveChangesAsync();
            return PrescriptionResultDto.Success(patient.Id);
        }

        public async Task<PatientWithPrescriptionsDto> GetPatientWithPrescriptionsAsync(int id)
        {
            var patient = await _context.Patients
                .AsNoTracking()
                .Include(p => p.Prescriptions)
                    .ThenInclude(pr => pr.Doctor)
                .Include(p => p.Prescriptions)
                    .ThenInclude(pr => pr.PrescriptionMedicaments)
                        .ThenInclude(pm => pm.Medicament)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (patient == null)
                return null;

            return new PatientWithPrescriptionsDto
            {
                IdPatient = patient.Id,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                BirthDate = patient.BirthDate,
                Prescriptions = patient.Prescriptions
                    .OrderBy(pr => pr.DueDate)
                    .Select(pr => new PrescriptionDto
                    {
                        IdPrescription = pr.Id,
                        Date = pr.Date,
                        DueDate = pr.DueDate,
                        Doctor = new DoctorDto
                        {
                            IdDoctor = pr.Doctor.Id,
                            FirstName = pr.Doctor.FirstName,
                            LastName = pr.Doctor.LastName,
                            Email = pr.Doctor.Email
                        },
                        Medicaments = pr.PrescriptionMedicaments
                            .Select(pm => new MedicamentDto
                            {
                                IdMedicament = pm.MedicamentId,
                                Name = pm.Medicament.Name,
                                Description = pm.Medicament.Description,
                                Type = pm.Medicament.Type,
                                Dose = pm.Dose,
                            }).ToList()
                    }).ToList()
            };
        }
    }
}
