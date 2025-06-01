import { Link } from 'react-router-dom'
import { useWallet } from '../contexts/WalletContext'

export default function Navbar() {
    const { address, isConnecting, connect, disconnect, role } = useWallet()

    const shortAddress = address
        ? `${address.slice(0, 6)}...${address.slice(-4)}`
        : null

    const roleLabel = {
        doctor: '👨‍⚕️ Médico',
        patient: '🧑‍🦰 Paciente',
        none: '👤 Visitante'
    }

    console.log('[Navbar] Role atual:', role)

    return (
        <nav className="navbar navbar-expand-lg navbar-dark bg-dark">
            <div className="container">
                <Link className="navbar-brand" to="/">🧬 MedLedger</Link>

                <div className="collapse navbar-collapse">
                    <ul className="navbar-nav me-auto">
                        <li className="nav-item">
                            <Link className="nav-link" to="/">Home</Link>
                        </li>
                        <li className="nav-item">
                            <Link className="nav-link" to="/protected">Área Protegida</Link>
                        </li>
                        {role === 'doctor' && (
                            <li className="nav-item">
                                <Link className="nav-link" to="/register">Registrar Prontuário</Link>
                            </li>
                        )}
                        {role === 'patient' && (
                            <li className="nav-item">
                                <Link className="nav-link" to="/records">Meus Prontuários</Link>
                            </li>
                        )}
                    </ul>
                </div>

                <div className="ms-auto d-flex align-items-center gap-2">
                    <span className="text-light small">{roleLabel[role]}</span>

                    {isConnecting ? (
                        <span className="text-light">Conectando...</span>
                    ) : address ? (
                        <div className="dropdown">
                            <button
                                className="btn btn-success dropdown-toggle"
                                type="button"
                                id="walletDropdown"
                                data-bs-toggle="dropdown"
                                aria-expanded="false"
                            >
                                🟢 {shortAddress}
                            </button>
                            <ul className="dropdown-menu dropdown-menu-end" aria-labelledby="walletDropdown">
                                <li>
                                    <button className="dropdown-item text-danger" onClick={disconnect}>
                                        🔌 Desconectar
                                    </button>
                                </li>
                            </ul>
                        </div>
                    ) : (
                        <button className="btn btn-outline-light" onClick={() => connect()}>
                            🔌 Conectar Carteira
                        </button>
                    )}
                </div>
            </div>
        </nav>
    )
}