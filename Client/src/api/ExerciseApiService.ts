import { BaseApiService } from './BaseApiService';

// ─── Types ────────────────────────────────────────────────────────────────────

export interface MuscleGroupDto {
    id: number;
    name: string;
}

export enum ExerciseDifficulty {
    Beginner = 1,
    Intermediate = 2,
    Advanced = 3,
    Elite = 4,
}

export interface ExerciseDto {
    id: number;
    name: string;
    description?: string;
    isCompound: boolean;
    imageUrl?: string;
    difficulty?: ExerciseDifficulty;
    isSystem: boolean;
    createdByUserId?: number;
    targetedMuscles?: MuscleGroupDto[];
}

export interface CreateEditExerciseDto {
    name: string;
    description?: string;
    isCompound: boolean;
    muscleGroupIds: number[];
    imageUrl?: string;
    difficulty?: ExerciseDifficulty;
}

export interface PagedResult<T> {
    items: T[];
    pageIndex: number;
    totalPages: number;
    totalCount: number;
    hasPreviousPage: boolean;
    hasNextPage: boolean;
}

// ─── Service ──────────────────────────────────────────────────────────────────

export class ExerciseApiService extends BaseApiService {
    /** GET /exercises?pageNumber=1&pageSize=20&search=squat&muscleGroupIds=1,2 */
    static async getPagedExercises(
        pageNumber = 1,
        pageSize = 20,
        search?: string,
        muscleGroupIds?: number[],
    ): Promise<PagedResult<ExerciseDto>> {
        const params = new URLSearchParams();
        params.append('pageNumber', String(pageNumber));
        params.append('pageSize', String(pageSize));
        if (search) params.append('search', search);
        if (muscleGroupIds?.length) {
            muscleGroupIds.forEach(id => params.append('muscleGroupIds', String(id)));
        }
        const res = await this.getAsync<{ isSuccess: boolean; value: PagedResult<ExerciseDto> }>(
            `/exercises?${params.toString()}`,
        );
        return res.value;
    }

    /** GET /exercises/{id} */
    static async getExerciseById(id: number): Promise<ExerciseDto> {
        const res = await this.getAsync<{ isSuccess: boolean; value: ExerciseDto }>(`/exercises/${id}`);
        return res.value;
    }

    /** GET /exercises/muscle-groups */
    static async getMuscleGroups(): Promise<MuscleGroupDto[]> {
        const res = await this.getAsync<{ isSuccess: boolean; value: MuscleGroupDto[] }>('/exercises/muscle-groups');
        return res.value ?? [];
    }

    /** POST /exercises */
    static async createExercise(dto: CreateEditExerciseDto): Promise<ExerciseDto> {
        const res = await this.postAsync<{ isSuccess: boolean; value: ExerciseDto }, CreateEditExerciseDto>(
            '/exercises',
            dto,
        );
        return res.value;
    }

    /** PUT /exercises/{id} */
    static async updateExercise(id: number, dto: CreateEditExerciseDto): Promise<ExerciseDto> {
        const res = await this.putAsync<{ isSuccess: boolean; value: ExerciseDto }, CreateEditExerciseDto>(
            `/exercises/${id}`,
            dto,
        );
        return res.value;
    }

    /** DELETE /exercises/{id} */
    static async deleteExercise(id: number): Promise<void> {
        await this.deleteAsync<void>(`/exercises/${id}`);
    }
}
