using Tutorial5.DTOs;

namespace Tutorial5.Services;
using Tutorial5.DTOs;

public interface IDbService
{
    Task<> GetPatientWithPrescriptionsAsync(int id);
    Task<PrescriptionResultDto> CreatePrescriptionAsync(PatientWithPrescriptionsDto request);
}