using MedLedger.Api.AccessLogs;
using MedLedger.Api.Data.Repositories;

namespace MedLedger.Api.Patients;

public static class PatientsEndpoints
{
    public static void MapPatientsEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/v1/patients").WithTags("Patients");

        group.MapPost("/", async (Patient patient, IMongoRepository<Patient> repo) =>
        {
            if (string.IsNullOrWhiteSpace(patient.Wallet) ||
                string.IsNullOrWhiteSpace(patient.Name) ||
                string.IsNullOrWhiteSpace(patient.Email))
            {
                return Results.BadRequest("Wallet, name and email are required.");
            }
            
            var existing = await repo.FirstOrDefaultAsync(p => p.Wallet == patient.Wallet);
            if (existing is not null)
                return Results.Conflict("Patient with this wallet already exists.");

            await repo.AddAsync(patient);
            return Results.Created($"/api/v1/patients/{patient.Id}", patient);
        });

        group.MapGet("/", async (IMongoRepository<Patient> repo) =>
        {
            var patients = await repo.GetAllAsync();
            return Results.Ok(patients);
        });
        
        group.MapGet("/{wallet}/access-logs", async (
            string wallet,
            IMongoRepository<AccessLog> accessLogRepo) =>
        {
            var logs = await accessLogRepo.FilterAsync(l =>
                l.PatientWallet.ToLower() == wallet.ToLower());

            return Results.Ok(logs.OrderByDescending(l => l.Timestamp));
        });
    }
}