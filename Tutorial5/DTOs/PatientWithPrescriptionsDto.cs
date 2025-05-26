
namespace Tutorial5.DTOs;

public class PatientWithPrescriptionsDto
{
    public int IdPatient     { get; set; }
    public string FirstName  { get; set; }
    public string LastName   { get; set; }
    public DateTime BirthDate{ get; set; }
    public List<PrescriptionDto> Prescriptions { get; set; } 
        = new List<PrescriptionDto>();
}

public class PrescriptionDto
{
    public int    IdPrescription { get; set; }
    public DateTime Date         { get; set; }
    public DateTime DueDate      { get; set; }
    public DoctorDto Doctor               { get; set; }
    public List<MedicamentDto> Medicaments{ get; set; }
        = new List<MedicamentDto>();
}

public class MedicamentDto
{
    public int    IdMedicament { get; set; }
    public string Name         { get; set; }
    public string Description  { get; set; }
    public string Type         { get; set; }
    public int    Dose         { get; set; }
}

public class DoctorDto
{
    public int    IdDoctor   { get; set; }
    public string FirstName  { get; set; }
    public string LastName   { get; set; }
    public string Email      { get; set; }
}
