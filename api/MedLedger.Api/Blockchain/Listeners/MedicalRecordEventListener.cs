using MedLedger.Api.Blockchain.Models;
using MedLedger.Api.Data.Repositories;
using MedLedger.Api.Records;
using Microsoft.Extensions.Options;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Nethereum.RPC.Eth.DTOs;

namespace MedLedger.Api.Blockchain.Listeners;

public class MedicalRecordEventListener : BackgroundService
{
    private readonly ILogger<MedicalRecordEventListener> _logger;
    private readonly Web3 _web3;
    private readonly string _contractAddress;
    private readonly IMongoRepository<MedicalRecordDocument> _recordRepo;

    public MedicalRecordEventListener(
        ILogger<MedicalRecordEventListener> logger,
        IOptions<BlockchainOptions> options,
        IMongoRepository<MedicalRecordDocument> recordRepo)
    {
        _logger = logger;
        _recordRepo = recordRepo;

        var account = new Account(options.Value.PrivateKey);
        _web3 = new Web3(account, options.Value.RpcUrl);
        _contractAddress = options.Value.ContractAddress;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var handler = _web3.Eth.GetEvent<LogRecordCreated>(_contractAddress);

        var latestBlock = await _web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();

        var filterInput = handler.CreateFilterInput(
            fromBlock: new BlockParameter(latestBlock),
            toBlock: BlockParameter.CreateLatest()
        );

        var filterId = await handler.CreateFilterAsync(filterInput);

        _logger.LogInformation("Listening for RecordCreated events...");

        while (!stoppingToken.IsCancellationRequested)
        {
            var logs = await handler.GetFilterChangesAsync(filterId);

            foreach (var log in logs)
            {
                var data = log.Event;

                var hexDataHash = "0x" + BitConverter.ToString(data.DataHash).Replace("-", "").ToLowerInvariant();
                var exists = await _recordRepo.FirstOrDefaultAsync(r =>
                    r.PatientWallet.ToLower() == data.Patient.ToLower() &&
                    r.DoctorWallet.ToLower() == data.Doctor.ToLower() &&
                    r.Timestamp == (long)data.Timestamp);

                if (exists is null)
                {
                    var record = new MedicalRecordDocument
                    {
                        PatientWallet = data.Patient,
                        DoctorWallet = data.Doctor,
                        DataHash = hexDataHash,
                        Timestamp = (long)data.Timestamp,
                        Summary = "Prontuário recebido via evento blockchain",
                        Status = "confirmed",
                        CreatedAt = DateTime.UtcNow
                    };

                    await _recordRepo.AddAsync(record);
                    _logger.LogInformation("Record indexed for {wallet}", data.Patient);
                }
            }

            await Task.Delay(3000, stoppingToken);
        }
    }
}