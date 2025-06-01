import { createContext, useContext, useEffect, useState } from 'react'
import api from '../api/axios' // axios configurado com baseURL

type UserRole = 'doctor' | 'patient' | 'none'

interface WalletContextValue {
    address: string | null
    isConnecting: boolean
    isLoading: boolean
    connect: () => Promise<void>
    disconnect: () => void
    role: UserRole
}

const WalletContext = createContext<WalletContextValue | undefined>(undefined)

export function WalletProvider({ children }: { children: React.ReactNode }) {
    const [address, setAddress] = useState<string | null>(null)
    const [isConnecting, setIsConnecting] = useState(false)
    const [isLoading, setIsLoading] = useState(true)
    const [role, setRole] = useState<UserRole>('none')

    const connect = async () => {
        if (!window.ethereum) {
            alert('MetaMask não encontrada.')
            return
        }

        try {
            setIsConnecting(true)
            const accounts = await window.ethereum.request({ method: 'eth_requestAccounts' })
            setAddress(accounts[0])
        } catch (err) {
            console.error('Erro ao conectar carteira:', err)
        } finally {
            setIsConnecting(false)
        }
    }

    const disconnect = () => {
        setAddress(null)
        setRole('none')
        alert('Para desconectar completamente, vá até MetaMask > Conexões e remova este site.')
    }

    useEffect(() => {
        if (!window.ethereum) {
            setIsLoading(false)
            return
        }

        const handleAccountsChanged = (accounts: string[]) => {
            setAddress(accounts[0] || null)
        }

        window.ethereum.on('accountsChanged', handleAccountsChanged)

        window.ethereum.request({ method: 'eth_accounts' })
            .then((accounts: string[]) => {
                if (accounts.length > 0) setAddress(accounts[0])
            })
            .finally(() => setIsLoading(false))

        return () => {
            window.ethereum.removeListener('accountsChanged', handleAccountsChanged)
        }
    }, [])

    useEffect(() => {
        async function detectRole(wallet: string) {
            try {
                const [doctorsRes, patientsRes] = await Promise.all([
                    api.get('/v1/doctors'),
                    api.get('/v1/patients')
                ])

                const isDoctor = doctorsRes.data.some((doc: any) => doc.wallet?.toLowerCase() === wallet.toLowerCase())
                const isPatient = patientsRes.data.some((pat: any) => pat.wallet?.toLowerCase() === wallet.toLowerCase())

                if (isDoctor) setRole('doctor')
                else if (isPatient) setRole('patient')
                else setRole('none')
            } catch (err) {
                console.error('Erro ao detectar papel do usuário:', err)
                setRole('none')
            }
        }

        if (address) {
            detectRole(address)
        }
    }, [address])

    return (
        <WalletContext.Provider value={{ address, isConnecting, isLoading, connect, disconnect, role }}>
            {children}
        </WalletContext.Provider>
    )
}

export function useWallet() {
    const context = useContext(WalletContext)
    if (!context) {
        throw new Error('useWallet must be used within a WalletProvider')
    }
    return context
}