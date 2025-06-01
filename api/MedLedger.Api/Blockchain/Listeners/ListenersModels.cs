using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace MedLedger.Api.Blockchain.Listeners;

[Event("RecordCreated")]
public class LogRecordCreated : IEventDTO
{
    [Parameter("address", "patient", 1, true)]
    public string Patient { get; set; } = null!;

    [Parameter("address", "doctor", 2, true)]
    public string Doctor { get; set; } = null!;

    [Parameter("bytes32", "dataHash", 3, false)]
    public byte[] DataHash { get; set; } = null!;

    [Parameter("uint256", "timestamp", 4, false)]
    public BigInteger Timestamp { get; set; }
}

[Event("AccessGranted")]
public class LogAccessGranted : IEventDTO
{
    [Parameter("address", "patient", 1, true)]
    public string Patient { get; set; } = null!;

    [Parameter("address", "doctor", 2, true)]
    public string Doctor { get; set; } = null!;
}

[Event("AccessRevoked")]
public class LogAccessRevoked : IEventDTO
{
    [Parameter("address", "patient", 1, true)]
    public string Patient { get; set; } = null!;

    [Parameter("address", "doctor", 2, true)]
    public string Doctor { get; set; } = null!;
}