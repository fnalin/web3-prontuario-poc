using MedLedger.Api.Blockchain.Listeners;
using MedLedger.Api.Blockchain.Models;
using MedLedger.Api.Data;
using MedLedger.Api.Data.Repositories;

namespace MedLedger.Api.Extensions;

public static class DependenciesSetupExtension
{
    public static void AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddOptions<BlockchainOptions>()
            .Bind(configuration.GetSection("Blockchain"))
            .ValidateDataAnnotations()
            .ValidateOnStart(); // dispara exception se inválido ao iniciar
        
        services.AddOptions<MongoOptions>()
            .Bind(configuration.GetSection("Mongo"))
            .ValidateDataAnnotations()
            .ValidateOnStart(); // dispara exception se inválido ao iniciar
        
        services.AddSingleton(typeof(IMongoRepository<>), typeof(MongoRepository<>));
        
        services.AddHostedService<MedicalRecordEventListener>();
        services.AddHostedService<MedicalAccessEventListener>();
    }
    
    public static void UseDependencies(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        MongoSeeder.SeedAsync(scope.ServiceProvider).Wait();
    }
}