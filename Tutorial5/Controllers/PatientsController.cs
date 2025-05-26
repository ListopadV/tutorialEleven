using Microsoft.AspNetCore.Mvc;
using Tutorial5.DTOs;
using Tutorial5.Data;

namespace Tutorial5.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientsController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public PatientsController(DatabaseContext context)
        {
            _context = context;
        }

        // GET api/patients/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPatientWithPrescriptions(int id)
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
                return NotFound();

            var dto = new PatientWithPrescriptionsDto
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

            return Ok(dto);
        }
    }
}