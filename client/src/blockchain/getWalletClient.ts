import { createWalletClient, custom } from 'viem'

export function getWalletClient() {
    if (!window.ethereum) {
        throw new Error('Carteira não encontrada. Verifique se o MetaMask está instalado.')
    }

    return createWalletClient({
        transport: custom(window.ethereum)
        // ⛔️ não force o chain aqui, deixe o MetaMask indicar qual rede está conectada
    })
}