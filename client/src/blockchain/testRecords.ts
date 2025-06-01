import { createPublicClient, http } from 'viem'
import { defineChain } from 'viem'
import { medicalRecordAbi } from './MedicalRecordAbi'

const CONTRACT_ADDRESS = '0x5fbdb2315678afecb367f032d93f642f64180aa3' // atualize se necessário
const PATIENT_ADDRESS = '0x70997970c51812dc3a010c7d01b50e0d17dc79c8' // endereço que você quer testar

const chain = defineChain({
    id: 31337,
    name: 'Hardhat',
    nativeCurrency: { name: 'Ether', symbol: 'ETH', decimals: 18 },
    rpcUrls: {
        default: { http: ['http://127.0.0.1:8545'] }
    }
})

const client = createPublicClient({
    chain,
    transport: http()
})

async function main() {
    try {
        const result = await client.readContract({
            address: CONTRACT_ADDRESS,
            abi: medicalRecordAbi,
            functionName: 'getRecordsByPatient',
            args: [PATIENT_ADDRESS]
        })

        console.log(`✅ ${result.length} registros encontrados:\n`)
        for (const [i, record] of result.entries()) {
            console.log(`📄 Registro #${i + 1}`)
            console.log('Paciente:', record.patient)
            console.log('Médico:', record.doctor)
            console.log('Hash:', record.dataHash)
            console.log('Timestamp:', new Date(Number(record.timestamp) * 1000).toISOString())
            console.log('---')
        }
    } catch (err) {
        console.error('❌ Erro ao consultar registros:', err)
    }
}

main()