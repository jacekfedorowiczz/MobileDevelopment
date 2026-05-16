import { BaseApiService } from './BaseApiService';
import type { PagedResult } from './ExerciseApiService';
import type { DietDayDto, CreateDietDayWithMealsDto } from './DietDayApiService';

export interface DietSummary {
    caloriesConsumed: number;
    caloriesGoal: number;
    protein: number;
    carbs: number;
    fat: number;
}

export interface DietDto {
    id: number;
    userId: number;
    name: string;
    description?: string;
    startDate: string;
    endDate?: string;
    isActive: boolean;
    dietDays?: DietDayDto[];
}

export interface CreateEditDietDto {
    userId: number;
    name: string;
    description?: string;
    startDate: string;
    endDate?: string;
}

export interface CreateDietWithDaysDto {
    userId: number;
    name: string;
    description?: string;
    startDate: string;
    endDate?: string;
    dietDays?: CreateDietDayWithMealsDto[];
}

export class DietApiService extends BaseApiService {
    static async getDietSummary(): Promise<DietSummary> {
        const res = await this.getAsync<{ isSuccess?: boolean; value?: DietSummary } | DietSummary>('/diets/summary');
        return 'value' in res && res.value ? res.value : res as DietSummary;
    }

    static async getAllDiets(): Promise<DietDto[]> {
        const res = await this.getAsync<{ isSuccess?: boolean; value?: DietDto[] } | DietDto[]>('/diets');
        return Array.isArray(res) ? res : res?.value ?? [];
    }

    static async getPagedDiets(pageNumber = 1, pageSize = 10): Promise<PagedResult<DietDto>> {
        const res = await this.getAsync<{ isSuccess?: boolean; value?: PagedResult<DietDto> } | PagedResult<DietDto>>(
            `/diets/paged?pageNumber=${pageNumber}&pageSize=${pageSize}`
        );
        return 'value' in res && res.value ? res.value : res as PagedResult<DietDto>;
    }

    static async getDietById(id: number): Promise<DietDto> {
        const res = await this.getAsync<{ isSuccess?: boolean; value?: DietDto } | DietDto>(`/diets/${id}`);
        return 'value' in res && res.value ? res.value : res as DietDto;
    }

    static async createDiet(dto: CreateEditDietDto): Promise<DietDto> {
        const res = await this.postAsync<{ isSuccess?: boolean; value?: DietDto } | DietDto, CreateEditDietDto>('/diets', dto);
        return 'value' in res && res.value ? res.value : res as DietDto;
    }

    static async updateDiet(id: number, dto: CreateEditDietDto): Promise<DietDto> {
        const res = await this.putAsync<{ isSuccess?: boolean; value?: DietDto } | DietDto, CreateEditDietDto>(`/diets/${id}`, dto);
        return 'value' in res && res.value ? res.value : res as DietDto;
    }

    static async deleteDiet(id: number): Promise<void> {
        await this.deleteAsync<void>(`/diets/${id}`);
    }
}
