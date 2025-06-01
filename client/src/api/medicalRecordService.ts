// src/api/medicalRecordService.ts
import api from './axios'
import type {MedicalRecord} from '../types/medicalRecord'

export async function getMedicalRecordsByWallet(wallet: string): Promise<MedicalRecord[]> {
    const response = await api.get<MedicalRecord[]>(`/v1/records/${wallet}`)
    return response.data
}