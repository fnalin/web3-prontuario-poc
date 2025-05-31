using MedLedger.Api.AccessLogs;
using MedLedger.Api.Blockchain.Models;
using MedLedger.Api.Data.Repositories;
using Microsoft.Extensions.Options;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Nethereum.RPC.Eth.DTOs;

namespace MedLedger.Api.Blockchain.Listeners;

public class MedicalAccessEventListener : BackgroundService
{
    private readonly ILogger<MedicalAccessEventListener> _logger;
    private readonly Web3 _web3;
    private readonly string _contractAddress;
    private readonly IMongoRepository<AccessLog> _accessLogRepo;

    public MedicalAccessEventListener(
        IMongoRepository<AccessLog> accessLogRepo,
        ILogger<MedicalAccessEventListener> logger,
        IOptions<BlockchainOptions> options)
    {
        _accessLogRepo = accessLogRepo;
        _logger = logger;

        var account = new Account(options.Value.PrivateKey);
        _web3 = new Web3(account, options.Value.RpcUrl);
        _contractAddress = options.Value.ContractAddress;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var grantedHandler = _web3.Eth.GetEvent<LogAccessGranted>(_contractAddress);
        var revokedHandler = _web3.Eth.GetEvent<LogAccessRevoked>(_contractAddress);

        var latestBlock = await _web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();

        var grantedFilter = await grantedHandler.CreateFilterAsync(
            grantedHandler.CreateFilterInput(fromBlock: new BlockParameter(latestBlock))
        );

        var revokedFilter = await revokedHandler.CreateFilterAsync(
            revokedHandler.CreateFilterInput(fromBlock: new BlockParameter(latestBlock))
        );

        _logger.LogInformation("Listening to AccessGranted and AccessRevoked events...");

        while (!stoppingToken.IsCancellationRequested)
        {
            var grantedLogs = await grantedHandler.GetFilterChangesAsync(grantedFilter);
            foreach (var log in grantedLogs)
            {
                _logger.LogInformation("AccessGranted: {Patient} → {Doctor}", log.Event.Patient, log.Event.Doctor);
                var accessLog = new AccessLog
                {
                    PatientWallet = log.Event.Patient,
                    DoctorWallet = log.Event.Doctor,
                    Action = "grant",
                    Timestamp = DateTime.UtcNow,
                    TransactionHash = log.Log.TransactionHash
                };

                await _accessLogRepo.AddAsync(accessLog);
            }

            var revokedLogs = await revokedHandler.GetFilterChangesAsync(revokedFilter);
            foreach (var log in revokedLogs)
            {
                _logger.LogInformation("AccessRevoked: {Patient} X {Doctor}", log.Event.Patient, log.Event.Doctor);
                var accessLog = new AccessLog
                {
                    PatientWallet = log.Event.Patient,
                    DoctorWallet = log.Event.Doctor,
                    Action = "revoke",
                    Timestamp = DateTime.UtcNow,
                    TransactionHash = log.Log.TransactionHash
                };

                await _accessLogRepo.AddAsync(accessLog);
            }

            await Task.Delay(3000, stoppingToken);
        }
    }
}