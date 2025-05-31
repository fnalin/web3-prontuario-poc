using MedLedger.Api.Data.Repositories;

namespace MedLedger.Api.Doctors;

public static class DoctorsEndpoints
{
    public static void MapDoctorsEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/v1/doctors").WithTags("Doctors");

        group.MapPost("/", async (Doctor doctor, IMongoRepository<Doctor> repo) =>
        {
            if (string.IsNullOrWhiteSpace(doctor.Wallet) ||
                string.IsNullOrWhiteSpace(doctor.Name) ||
                string.IsNullOrWhiteSpace(doctor.Crm) ||
                string.IsNullOrWhiteSpace(doctor.Specialty))
            {
                return Results.BadRequest("Wallet, name, CRM, and specialty are required.");
            }
            
            var existing = await repo.FirstOrDefaultAsync(d => d.Wallet == doctor.Wallet);
            if (existing is not null)
                return Results.Conflict("Doctor with this wallet already exists.");

            await repo.AddAsync(doctor);
            return Results.Created($"/api/v1/doctors/{doctor.Id}", doctor);
        });

        group.MapGet("/", async (IMongoRepository<Doctor> repo) =>
        {
            var doctors = await repo.GetAllAsync();
            return Results.Ok(doctors);
        });
    }
}