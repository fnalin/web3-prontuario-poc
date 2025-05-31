namespace MedLedger.Api.Blockchain.Models;

public record RecordRequest(string PatientWallet, string DoctorWallet, string Data);