namespace Tutorial5.DTOs;

public class PatientWithMedicamentsDto
{
    public int? IdPatient { get; set; }

    public string FirstName { get; set; }
    public string LastName  { get; set; }
    public DateTime BirthDate { get; set; }

    public List<PrescriptionCreateDto> Prescriptions { get; set; }
}

public class PrescriptionCreateDto
{
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public DoctorDto Doctor { get; set; }
    public List<MedicamentDto> Medicaments { get; set; }
}

public class DoctorDto
{
    public int IdDoctor { get; set; }
}

public class MedicamentDto
{
    public int IdMedicament { get; set; }
    public int Dose { get; set; }
    public string Description { get; set; }
}