using MedLedger.Api.Data.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MedLedger.Api.Records;

public class MedicalRecordDocument : EntityBase
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;

    [BsonElement("patientWallet")]
    public string PatientWallet { get; set; } = null!;

    [BsonElement("dataHash")]
    public string DataHash { get; set; } = null!;

    [BsonElement("summary")]
    public string Summary { get; set; } = null!;

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}