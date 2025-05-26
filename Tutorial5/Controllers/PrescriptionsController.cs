using Microsoft.AspNetCore.Mvc;
using Tutorial5.Data;
using Tutorial5.Models;

namespace Tutorial5.Controllers

{
    [ApiController]
    [Route("api/[controller]")]
    public class PrescriptionsController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public PrescriptionsController(DatabaseContext context)
        {
            _context = context;
        }

        // POST api/prescriptions
        [HttpPost]
        public async Task<IActionResult> CreatePrescription([FromBody] PatientWithMedicamentsDto request)
        {
            // Validate list size
            var meds = request.Prescriptions.SelectMany(pr => pr.Medicaments).ToList();
            if (meds.Count > 10)
                return BadRequest("A prescription can include a maximum of 10 medications.");

            var prDto = request.Prescriptions.First();
            if (prDto.DueDate < prDto.Date)
                return BadRequest("DueDate must be greater than or equal to Date.");

            // Ensure patient exists or create
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

            // Ensure doctor exists or create
            var docDto = prDto.Doctor;
            var doctor = await _context.Doctors.FindAsync(docDto.IdDoctor);
            if (doctor == null)
            {
                return BadRequest("Doctor not found.");
            }

            // Create prescription
            var prescription = new Prescription
            {
                Date = prDto.Date,
                DueDate = prDto.DueDate,
                PatientId = patient.Id,
                DoctorId = doctor.Id
            };
            _context.Prescriptions.Add(prescription);
            await _context.SaveChangesAsync();

            // Add medications
            foreach (var m in prDto.Medicaments)
            {
                var med = await _context.Medicaments.FindAsync(m.IdMedicament);
                if (med == null)
                    return BadRequest($"Medicament with Id {m.IdMedicament} not found.");

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

            return CreatedAtAction(
                nameof(PatientsController.GetPatientWithPrescriptions),
                "Patients",
                new { id = patient.Id },
                null);
        }
    }
}