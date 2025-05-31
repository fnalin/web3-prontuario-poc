using MedLedger.Api.Blockchain.Models;
using MedLedger.Api.Blockchain.Services;

namespace MedLedger.Api.Blockchain.Endpoints;

public static class BlockchainEndpoints
{
    public static void MapBlockchainsEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/v1/blockchains")
            .WithTags("Blockchain Endpoints");
        
        group.MapPost("/", async (RecordRequest request, BlockchainService blockchain) =>
        {
            var hash = await blockchain.MintRecordAsync(request.PatientWallet, request.DoctorWallet, request.Data);
            return Results.Ok(new { hash });
        });

        group.MapGet("/{wallet}", async (string wallet, BlockchainService blockchain) =>
        {
            var hash = await blockchain.GetRecordAsync(wallet);
            return Results.Ok(new { hash });
        });
    }
}