using MedLedger.Api.Data.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MedLedger.Api.Doctors;

public class Doctor : EntityBase
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;

    [BsonElement("wallet")]
    public string Wallet { get; set; } = null!;

    [BsonElement("name")]
    public string Name { get; set; } = null!;

    [BsonElement("crm")]
    public string Crm { get; set; } = null!;
    
    [BsonElement("email")]
    public string Email { get; set; } = null!;

    [BsonElement("specialty")]
    public string Specialty { get; set; } = null!;

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}