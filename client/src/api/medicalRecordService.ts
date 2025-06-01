import api from './axios'
import type {MedicalRecord} from '../types/medicalRecord'

export async function getMedicalRecordsByWallet(wallet: string): Promise<MedicalRecord[]> {
    const response = await api.get<MedicalRecord[]>(`/v1/records/${wallet}`)
    return response.data
}

export async function getRecordsByWallet(wallet: string, status?: string): Promise<MedicalRecord[]> {
    const response = await api.get(`/v1/records/${wallet}`, {
        params: status ? { status } : {},
    })
    return response.data
}

export async function approveRecord(id: string): Promise<void> {
    await api.put(`/v1/records/${id}/status`, { status: 'confirmed' })
}