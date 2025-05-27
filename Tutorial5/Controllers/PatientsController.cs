using Microsoft.AspNetCore.Mvc;
using Tutorial5.Services;

namespace Tutorial5.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientsController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPatientWithPrescriptions(int id)
        {
            try
            {
                var dto = await _patientService.GetPatientWithPrescriptionsAsync(id);
                if (dto == null)
                    return NotFound($"Patient with Id {id} not found.");
                return Ok(dto);
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
    }
}