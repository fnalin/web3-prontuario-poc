import { Navigate, Outlet } from 'react-router-dom'
import { useWallet } from '../contexts/WalletContext'

export default function PrivateRoute() {
    const { address, isLoading } = useWallet()

    if (isLoading) {
        return <div className="container mt-4">🔄 Verificando carteira...</div>
    }

    if (!address) {
        return <Navigate to="/" replace />
    }

    return <Outlet />
}