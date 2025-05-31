using MedLedger.Api.Data.Repositories;
using MedLedger.Api.Doctors;
using MedLedger.Api.Patients;

namespace MedLedger.Api.Data;

public static class MongoSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        var patientRepo = services.GetRequiredService<IMongoRepository<Patient>>();
        var doctorRepo = services.GetRequiredService<IMongoRepository<Doctor>>();

        var hasPatients = await patientRepo.AnyAsync();
        var hasDoctors = await doctorRepo.AnyAsync();

        if (!hasPatients)
        {
            await patientRepo.AddAsync(new Patient
            {
                Name = "Alice Santos",
                Email = "alice@medledger.io",
                Wallet = "0x70997970C51812dc3A010C7d01b50e0d17dc79C8"
            });
        }

        if (!hasDoctors)
        {
            await doctorRepo.AddAsync(new Doctor
            {
                Name = "Dr. Bruno Lima",
                Email = "bruno@hospitaltech.org",
                Wallet = "0x3C44CdDdB6a900fa2b585dd299e03d12FA4293BC"
            });
        }
    }
}