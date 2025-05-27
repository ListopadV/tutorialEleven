using Microsoft.EntityFrameworkCore;
using Tutorial5.Data;
using Tutorial5.Services;
using Tutorial5.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<DatabaseContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
);
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IPrescriptionService, PrescriptionService>();

builder.Services.AddControllers();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
    db.Database.Migrate();
    if (!db.Doctors.Any())
    {
        db.Doctors.AddRange(
            new Doctor { FirstName="Alice", LastName="Smith", Email="alice@hospital.com" },
            new Doctor { FirstName="Bob",   LastName="Jones", Email="bob@clinic.com" }
        );
        db.Medicaments.AddRange(
            new Medicament { Name="Aspirin", Description="Pain killer", Type="Analgesic" },
            new Medicament { Name="Penicillin", Description="Antibiotic", Type="Antibiotic" }
        );
        db.SaveChanges();
    }
}

app.MapControllers();
app.Run();