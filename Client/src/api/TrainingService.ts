import { BaseApiService } from './BaseApiService';

export interface Exercise {
    id: number;
    name: string;
    description: string;
    imageUrl?: string;
}

export interface WorkoutSession {
    id: number;
    name: string;
    date: string;
    duration: string;
}

export class TrainingService extends BaseApiService {
    public static async getExercises(): Promise<Exercise[]> {
        const response = await this.getAsync<any>('/training/exercises');
        return response.value || [];
    }

    public static async getWorkoutSessions(): Promise<WorkoutSession[]> {
        const response = await this.getAsync<any>('/training/sessions');
        return response.value || [];
    }
}
