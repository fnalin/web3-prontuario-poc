import { useEffect, useState } from 'react'
import { useWallet } from '../contexts/WalletContext'
import { getRecordsByWallet } from '../api/medicalRecordService'
import type { MedicalRecord } from '../types/medicalRecord'
import Loading from '../components/Loading'
import { getWalletClient } from '../blockchain/getWalletClient'
import { mintRecord } from '../blockchain/mintRecord'

export default function PatientRecords() {
    const { address } = useWallet()
    const [records, setRecords] = useState<MedicalRecord[]>([])
    const [loading, setLoading] = useState(true)

    useEffect(() => {
        if (!address) return
        getRecordsByWallet(address).then(setRecords).finally(() => setLoading(false))
    }, [address])

    const handleApprove = async (dataHash: string) => {
        try {
            const walletClient = getWalletClient()
            const [doctorAddress] = await walletClient.getAddresses()
            const timestamp = Math.floor(Date.now() / 1000)

            console.log('[mint] dataHash:', dataHash)
            console.log('[mint] wallet address:', doctorAddress)
            console.log('[mint] patient address:', address)
            console.log('[mint] timestamp:', timestamp)

            const txHash = await mintRecord(
                walletClient,
                address as `0x${string}`,
                dataHash as `0x${string}`,
                timestamp
            )

            alert('✅ Registro aprovado: ' + txHash)
        } catch (err) {
            console.error('Erro ao aprovar:', err)
            alert('❌ Falha ao aprovar registro.')
        }
    }

    return (
        <div className="container mt-4">
            <h2>📄 Meus Prontuários</h2>

            {loading && <Loading message="Carregando prontuários..." />}

            {!loading && records.length === 0 && (
                <div className="alert alert-info">Nenhum prontuário encontrado.</div>
            )}

            {!loading && records.map(record => (
                <div key={record.id} className="card mb-3">
                    <div className="card-body">
                        <h5 className="card-title">📌 {record.summary}</h5>
                        <p className="card-text">
                            <strong>Hash:</strong> {record.dataHash}<br />
                            <strong>Status:</strong> {record.status}
                        </p>

                        {record.status === 'pending' && (
                            <button
                                className="btn btn-success"
                                onClick={() => handleApprove(record.dataHash)}
                            >
                                ✅ Aprovar
                            </button>
                        )}
                    </div>
                </div>
            ))}
        </div>
    )
}