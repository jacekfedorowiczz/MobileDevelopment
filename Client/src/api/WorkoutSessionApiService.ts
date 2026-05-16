import { BaseApiService } from './BaseApiService';
import type { ExerciseDto } from './ExerciseApiService';
import type { PagedResult } from './ExerciseApiService';

// ─── Types ────────────────────────────────────────────────────────────────────

export interface WorkoutSetDto {
    id: number;
    workoutSessionId: number;
    exerciseId: number;
    setNumber: number;
    weight: number;
    reps: number;
    rpe?: number;
    durationSeconds?: number;
    exercise?: ExerciseDto;
}

export interface WorkoutSessionDto {
    id: number;
    userId: number;
    name: string;
    description?: string;
    startTime: string;   // ISO string
    endTime?: string;
    globalSessionRpe?: number;
    sets?: WorkoutSetDto[];
}

export interface WorkoutSessionSummaryDto {
    id: number;
    userId: number;
    name: string;
    description?: string;
    startTime: string;
    endTime?: string;
    globalSessionRpe?: number;
    exerciseCount: number;
    setCount: number;
}

export interface CreateEditWorkoutSessionDto {
    name: string;
    description?: string;
    startTime: string;
    endTime?: string;
}

// ─── Service ──────────────────────────────────────────────────────────────────

export class WorkoutSessionApiService extends BaseApiService {
    /** GET /workout-sessions?pageNumber=1&pageSize=20 */
    static async getPagedSessions(
        pageNumber = 1,
        pageSize = 20,
    ): Promise<PagedResult<WorkoutSessionSummaryDto>> {
        const res = await this.getAsync<{ isSuccess: boolean; value: PagedResult<WorkoutSessionSummaryDto> }>(
            `/workout-sessions?pageNumber=${pageNumber}&pageSize=${pageSize}`,
        );
        return res.value;
    }

    /** GET /workout-sessions/{id} — includes sets + exercises */
    static async getSessionById(id: number): Promise<WorkoutSessionDto> {
        const res = await this.getAsync<{ isSuccess: boolean; value: WorkoutSessionDto }>(
            `/workout-sessions/${id}`,
        );
        return res.value;
    }

    /** POST /workout-sessions */
    static async createSession(dto: CreateEditWorkoutSessionDto): Promise<WorkoutSessionDto> {
        const res = await this.postAsync<
            { isSuccess: boolean; value: WorkoutSessionDto },
            CreateEditWorkoutSessionDto
        >('/workout-sessions', dto);
        return res.value;
    }

    /** PUT /workout-sessions/{id} */
    static async updateSession(
        id: number,
        dto: CreateEditWorkoutSessionDto,
    ): Promise<WorkoutSessionDto> {
        const res = await this.putAsync<
            { isSuccess: boolean; value: WorkoutSessionDto },
            CreateEditWorkoutSessionDto
        >(`/workout-sessions/${id}`, dto);
        return res.value;
    }

    /** DELETE /workout-sessions/{id} */
    static async deleteSession(id: number): Promise<void> {
        await this.deleteAsync<void>(`/workout-sessions/${id}`);
    }

    /** DELETE /workout-sessions/range */
    static async deleteSessions(ids: number[]): Promise<void> {
        await this.deleteAsync<void, number[]>('/workout-sessions/range', ids);
    }
}
