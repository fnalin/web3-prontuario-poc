using System.ComponentModel.DataAnnotations;

namespace MedLedger.Api.Data;

public class MongoOptions
{
    [Required]
    public string ConnectionString { get; set; } = null!;
    [Required]
    public string DatabaseName { get; set; } = null!;
}