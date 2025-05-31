using System.ComponentModel.DataAnnotations;

namespace MedLedger.Api.Blockchain.Models;

public class BlockchainOptions
{
    [Required]
    public string PrivateKey { get; set; } = null!;
    [Required]
    public string RpcUrl { get; set; } = null!;
    [Required]
    public string ContractAddress { get; set; } = null!;
    [Required]
    public string AbiMedicalRecordPath { get; set; } = null!;
}