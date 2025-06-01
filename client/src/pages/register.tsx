import { useState } from 'react'
import { useWallet } from '../contexts/WalletContext'
import api from '../api/axios'
import { keccak256, toHex } from 'viem'

export default function RegisterRecord() {
    const { address, role } = useWallet()
    const [patientWallet, setPatientWallet] = useState('')
    const [summary, setSummary] = useState('')
    const [loading, setLoading] = useState(false)
    const [success, setSuccess] = useState(false)

    if (role !== 'doctor') {
        return <div className="container mt-4">🚫 Acesso restrito a médicos.</div>
    }

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault()
        setLoading(true)

        try {
            const recordData = {
                patientWallet,
                summary,
                createdAt: new Date().toISOString()
            }

            const hashInput = JSON.stringify(recordData)
            const dataHash = toHex(keccak256(hashInput))

            console.log('📦 dataHash:', dataHash)

            // 🔹 Aqui será chamado o contrato (mint)
            alert('Simulando mint NFT com hash: ' + dataHash)

            // 🔹 Salva no backend
            await api.post('/v1/records', {
                ...recordData,
                dataHash
            })

            setSuccess(true)
        } catch (err) {
            console.error('Erro ao registrar:', err)
            alert('Erro ao registrar prontuário.')
        } finally {
            setLoading(false)
        }
    }

    return (
        <div className="container mt-4">
            <h2>📝 Registrar Novo Prontuário</h2>

            {success && (
                <div className="alert alert-success">
                    ✅ Prontuário registrado com sucesso!
                </div>
            )}

            <form onSubmit={handleSubmit}>
                <div className="mb-3">
                    <label className="form-label">Wallet do Paciente</label>
                    <input
                        type="text"
                        className="form-control"
                        value={patientWallet}
                        onChange={e => setPatientWallet(e.target.value)}
                        required
                    />
                </div>

                <div className="mb-3">
                    <label className="form-label">Resumo do Prontuário</label>
                    <textarea
                        className="form-control"
                        rows={4}
                        value={summary}
                        onChange={e => setSummary(e.target.value)}
                        required
                    ></textarea>
                </div>

                <button type="submit" className="btn btn-primary" disabled={loading}>
                    {loading ? 'Registrando...' : 'Registrar Prontuário'}
                </button>
            </form>
        </div>
    )
}