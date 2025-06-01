using MedLedger.Api.Data.Repositories;

namespace MedLedger.Api.Records;

public static class RecordsEndpoints
{
    public static void MapRecordsEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/v1/records").WithTags("Records");

        group.MapPost("/", async (MedicalRecordDocument record, IMongoRepository<MedicalRecordDocument> repo) =>
        {
            if (string.IsNullOrWhiteSpace(record.PatientWallet) ||
                string.IsNullOrWhiteSpace(record.DataHash) ||
                string.IsNullOrWhiteSpace(record.Summary))
            {
                return Results.BadRequest("PatientWallet, DataHash and Summary are required.");
            }
            
            // evitar duplicação pelo hash
            var existing = await repo.FirstOrDefaultAsync(r =>
                r.PatientWallet == record.PatientWallet &&
                r.DataHash == record.DataHash);

            if (existing is not null)
                return Results.Conflict("This record already exists.");

            await repo.AddAsync(record);
            return Results.Created($"/api/v1/records/{record.Id}", record);
        });

        group.MapGet("/{wallet}", async (string wallet, string? status, IMongoRepository<MedicalRecordDocument> repo) =>
        {
            var normalizedWallet = wallet.ToLowerInvariant();

            var records = string.IsNullOrEmpty(status)
                ? await repo.FilterAsync(r => r.PatientWallet.ToLowerInvariant() == normalizedWallet)
                : await repo.FilterAsync(r => r.PatientWallet.ToLowerInvariant() == normalizedWallet && r.Status == status);

            return Results.Ok(records);
        });
    }
}