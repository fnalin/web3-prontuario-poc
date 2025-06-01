import { type WalletClient, createPublicClient, http, defineChain } from 'viem'
import { medicalRecordAbi } from './MedicalRecordAbi'

const CONTRACT_ADDRESS = import.meta.env.VITE_CONTRACT_ADDRESS as `0x${string}`

export async function mintRecord(
    walletClient: WalletClient,
    patient: `0x${string}`,
    dataHash: `0x${string}`,
    timestamp: number
) {
    const [doctor] = await walletClient.getAddresses()
    const chainId = await walletClient.getChainId()

    const chain = defineChain({
        id: chainId,
        name: 'Hardhat',
        nativeCurrency: { name: 'ETH', symbol: 'ETH', decimals: 18 },
        rpcUrls: { default: { http: ['http://127.0.0.1:8545'] } }
    })

    const publicClient = createPublicClient({ chain, transport: http() })

    try {
        const { request } = await publicClient.simulateContract({
            address: CONTRACT_ADDRESS,
            abi: medicalRecordAbi,
            functionName: 'createRecord',
            args: [patient, dataHash, BigInt(timestamp)],
            account: doctor
        })

        const hash = await walletClient.writeContract({ ...request, chain, gas: 300000n })

        console.log('✅ Tx enviada:', hash)
        return hash
    } catch (error) {
        console.error('❌ Erro ao mintar:', error)
        throw error
    }
}