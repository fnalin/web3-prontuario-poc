namespace MedLedger.Api.Blockchain.Models;

public record AccessRequest(string TokenId, string DoctorWallet, bool Grant);