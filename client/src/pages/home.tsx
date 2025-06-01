import { useEffect, useState } from 'react'
import type {MedicalRecord} from '../types/medicalRecord'
import { getMedicalRecordsByWallet } from '../api/medicalRecordService'
import { useWallet } from '../hooks/useWallet'

export default function Home() {
    const { address, connect } = useWallet()
    const [records, setRecords] = useState<MedicalRecord[]>([])
    const [loading, setLoading] = useState(false)
    const [error, setError] = useState<string | null>(null)

    useEffect(() => {
        if (!address) return
        setLoading(true)
        getMedicalRecordsByWallet(address)
            .then(setRecords)
            .catch(() => setError('Failed to load records'))
            .finally(() => setLoading(false))
    }, [address])

    return (
        <div className="container mt-4">
            <h1 className="mb-4">Medical Records</h1>

            {!address && (
                <button className="btn btn-primary" onClick={connect}>
                    Connect Wallet
                </button>
            )}

            {address && (
                <>
                    <p className="text-muted mb-2">Connected wallet: {address}</p>

                    {loading && <p>Loading records...</p>}
                    {error && <p className="text-danger">{error}</p>}

                    {!loading && records.length === 0 && <p>No records found.</p>}

                    {!loading && records.length > 0 && (
                        <table className="table table-striped">
                            <thead>
                            <tr>
                                <th>Summary</th>
                                <th>Hash</th>
                                <th>Created At</th>
                            </tr>
                            </thead>
                            <tbody>
                            {records.map(record => (
                                <tr key={record.id}>
                                    <td>{record.summary || '-'}</td>
                                    <td>{record.dataHash?.slice(0, 10)}...</td>
                                    <td>{new Date(record.createdAt).toLocaleString()}</td>
                                </tr>
                            ))}
                            </tbody>
                        </table>
                    )}
                </>
            )}
        </div>
    )
}