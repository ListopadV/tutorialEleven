namespace Tutorial5.DTOs;

public class PatientWithPrescriptionsDto
{
    public int IdPatient { get; set; }
    public string FirstName { get; set; }
    public string LastName  { get; set; }
    public DateTime BirthDate { get; set; }

    public List<PrescriptionDetailsDto> Prescriptions { get; set; }
}

public class PrescriptionDetailsDto
{
    public int IdPrescription { get; set; }
    public DateTime Date      { get; set; }
    public DateTime DueDate   { get; set; }
    public DoctorInfoDto Doctor { get; set; }
    public List<MedicamentInfoDto> Medicaments { get; set; }
}

public class DoctorInfoDto
{
    public int IdDoctor    { get; set; }
    public string FirstName { get; set; }
    public string LastName  { get; set; }
    public string Email     { get; set; }
}

public class MedicamentInfoDto
{
    public int IdMedicament { get; set; }
    public string Name      { get; set; }
    public int Dose         { get; set; }
    public string Description { get; set; }
}
