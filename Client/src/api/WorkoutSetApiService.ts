import { BaseApiService } from './BaseApiService';
import type { WorkoutSetDto } from './WorkoutSessionApiService';

export interface CreateEditWorkoutSetDto {
    workoutSessionId: number;
    exerciseId: number;
    setNumber: number;
    weight: number;
    reps: number;
    rpe?: number;
    durationSeconds?: number;
}

export class WorkoutSetApiService extends BaseApiService {
    static async getSetsBySession(sessionId: number): Promise<WorkoutSetDto[]> {
        const res = await this.getAsync<{ isSuccess: boolean; value: WorkoutSetDto[] }>(
            `/workout-sets/by-session/${sessionId}`,
        );
        return res.value ?? [];
    }

    static async getSetById(id: number): Promise<WorkoutSetDto> {
        const res = await this.getAsync<{ isSuccess: boolean; value: WorkoutSetDto }>(
            `/workout-sets/${id}`,
        );
        return res.value;
    }

    static async createSet(dto: CreateEditWorkoutSetDto): Promise<WorkoutSetDto> {
        const res = await this.postAsync<
            { isSuccess: boolean; value: WorkoutSetDto },
            CreateEditWorkoutSetDto
        >('/workout-sets', dto);
        return res.value;
    }

    static async updateSet(id: number, dto: CreateEditWorkoutSetDto): Promise<WorkoutSetDto> {
        const res = await this.putAsync<
            { isSuccess: boolean; value: WorkoutSetDto },
            CreateEditWorkoutSetDto
        >(`/workout-sets/${id}`, dto);
        return res.value;
    }

    static async deleteSet(id: number): Promise<void> {
        await this.deleteAsync<void>(`/workout-sets/${id}`);
    }

    static async deleteSets(ids: number[]): Promise<void> {
        await this.deleteAsync<void, number[]>('/workout-sets/range', ids);
    }
}
