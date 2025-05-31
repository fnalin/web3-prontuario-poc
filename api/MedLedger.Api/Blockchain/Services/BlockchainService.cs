using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Nethereum.Hex.HexTypes;
using Nethereum.Contracts;
using System.Numerics;

namespace MedLedger.Api.Blockchain.Services;

public class BlockchainService : IBlockchainService
{
    private readonly Web3 _web3;
    private readonly Contract _contract;
    private readonly string _accountAddress;

    public BlockchainService(IConfiguration config)
    {
        var privateKey = config["Blockchain:PrivateKey"];
        var rpcUrl = config["Blockchain:RpcUrl"];
        var contractAddress = config["Blockchain:ContractAddress"];
        var abi = File.ReadAllText(config["Blockchain:AbiMedicalRecordPath"]);

        var account = new Account(privateKey);
        _accountAddress = account.Address;
        _web3 = new Web3(account, rpcUrl);
        _contract = _web3.Eth.GetContract(abi, contractAddress);
    }

    public async Task<string> MintRecordAsync(string patientWallet, string doctorWallet, string data)
    {
        var hash = Web3.Sha3(data); // hash do conteúdo
        var function = _contract.GetFunction("mintRecord");
        var receipt = await function.SendTransactionAndWaitForReceiptAsync(
            _accountAddress, new HexBigInteger(300000), null, null, patientWallet, hash);
        return hash;
    }

    public async Task<string?> GetRecordAsync(string wallet)
    {
        var function = _contract.GetFunction("getRecordByWallet");
        return await function.CallAsync<string>(wallet);
    }
    
    public async Task GrantAccessAsync(string tokenId, string doctorWallet)
    {
        var function = _contract.GetFunction("grantAccess");
        await function.SendTransactionAndWaitForReceiptAsync(
            _accountAddress,
            new HexBigInteger(300000),
            null, null,
            BigInteger.Parse(tokenId), doctorWallet);
    }

    public async Task RevokeAccessAsync(string tokenId, string doctorWallet)
    {
        var function = _contract.GetFunction("revokeAccess");
        await function.SendTransactionAndWaitForReceiptAsync(
            _accountAddress,
            new HexBigInteger(300000),
            null, null,
            BigInteger.Parse(tokenId), doctorWallet);
    }
    
    public async Task<List<string>> GetAccessListByWalletAsync(string wallet)
    {
        var function = _contract.GetFunction("getAccessListByWallet");
        var result = await function.CallAsync<List<string>>(wallet);
        return result;
    }
}