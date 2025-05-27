using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Tutorial5.Services;
using Tutorial5.DTOs;

namespace Tutorial5.Controllers

{
    [ApiController]
    [Route("api/[controller]")]
    public class PrescriptionsController : ControllerBase
    {
        private readonly IPrescriptionService _prescriptionService;

        public PrescriptionsController(IPrescriptionService prescriptionService)
        {
            _prescriptionService = prescriptionService;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePrescription([FromBody] PatientWithMedicamentsDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var patientId = await _prescriptionService.CreatePrescriptionAsync(request);

                return CreatedAtAction(
                    nameof(PatientsController.GetPatientWithPrescriptions),
                    "Patients",
                    new { id = patientId },
                    (object)null
                );
            }
            catch (ValidationException vex)
            {
                return BadRequest(vex.Message);
            }
            catch (KeyNotFoundException knf)
            {
                return NotFound(knf.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
    }
}