using Microsoft.EntityFrameworkCore;
using Tutorial5.Data;
using Tutorial5.DTOs;

namespace Tutorial5.Services;

public class PatientService : IPatientService
    {
        private readonly DatabaseContext _ctx;

        public PatientService(DatabaseContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<PatientWithPrescriptionsDto> GetPatientWithPrescriptionsAsync(int patientId)
        {
            var patient = await _ctx.Patients
                .AsSingleQuery()
                .Include(p => p.Prescriptions)
                    .ThenInclude(pr => pr.Doctor)
                .Include(p => p.Prescriptions)
                    .ThenInclude(pr => pr.PrescriptionMedicaments)
                        .ThenInclude(pm => pm.Medicament)
                .FirstOrDefaultAsync(p => p.IdPatient == patientId);

            if (patient == null)
                return null;

            var result = new PatientWithPrescriptionsDto
            {
                IdPatient  = patient.IdPatient,
                FirstName  = patient.FirstName,
                LastName   = patient.LastName,
                BirthDate  = patient.BirthDate,
                Prescriptions = patient.Prescriptions
                    .OrderBy(pr => pr.DueDate)
                    .Select(pr => new PrescriptionDetailsDto
                    {
                        IdPrescription = pr.Id,
                        Date           = pr.Date,
                        DueDate        = pr.DueDate,
                        Doctor = new DoctorInfoDto
                        {
                            IdDoctor  = pr.Doctor.IdDoctor,
                            FirstName = pr.Doctor.FirstName,
                            LastName  = pr.Doctor.LastName,
                            Email     = pr.Doctor.Email
                        },
                        Medicaments = pr.PrescriptionMedicaments
                            .Select(pm => new MedicamentInfoDto
                            {
                                IdMedicament = pm.Medicament.IdMedicament,
                                Name         = pm.Medicament.Name,
                                Dose         = pm.Dose,
                                Description  = pm.Details
                            })
                            .ToList()
                    })
                    .ToList()
            };

            return result;
        }
    }