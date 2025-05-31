using MedLedger.Api.Data.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MedLedger.Api.AccessLogs;

public class AccessLog : EntityBase
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;

    [BsonElement("patientWallet")]
    public string PatientWallet { get; set; } = null!;

    [BsonElement("doctorWallet")]
    public string DoctorWallet { get; set; } = null!;

    [BsonElement("action")]
    public string Action { get; set; } = null!; // "grant" ou "revoke"

    [BsonElement("timestamp")]
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    [BsonElement("txHash")]
    public string TransactionHash { get; set; } = null!;
}