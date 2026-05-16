import { BaseApiService } from './BaseApiService';
import type { PagedResult } from './ExerciseApiService';
import type { MealDto, CreateEditMealDto } from './MealApiService';

export interface DietDayDto {
    id: number;
    dietId: number;
    date: string;
    notes?: string;
    meals?: MealDto[];
}

export interface CreateEditDietDayDto {
    dietId: number;
    date: string;
    notes?: string;
}

export interface CreateDietDayWithMealsDto {
    dietId: number;
    date: string;
    notes?: string;
    meals?: CreateEditMealDto[];
}

export class DietDayApiService extends BaseApiService {
    static async getAllDietDays(dietId: number): Promise<DietDayDto[]> {
        const res = await this.getAsync<{ isSuccess: boolean; value: DietDayDto[] }>(`/diet-days/by-diet/${dietId}`);
        return res.value;
    }

    static async getPagedDietDays(dietId: number, pageNumber = 1, pageSize = 10): Promise<PagedResult<DietDayDto>> {
        const res = await this.getAsync<{ isSuccess: boolean; value: PagedResult<DietDayDto> }>(
            `/diet-days/by-diet/${dietId}/paged?pageNumber=${pageNumber}&pageSize=${pageSize}`
        );
        return res.value;
    }

    static async getDietDayById(id: number): Promise<DietDayDto> {
        const res = await this.getAsync<{ isSuccess: boolean; value: DietDayDto }>(`/diet-days/${id}`);
        return res.value;
    }

    static async createDietDay(dto: CreateEditDietDayDto): Promise<DietDayDto> {
        const res = await this.postAsync<{ isSuccess: boolean; value: DietDayDto }, CreateEditDietDayDto>('/diet-days', dto);
        return res.value;
    }

    static async updateDietDay(id: number, dto: CreateEditDietDayDto): Promise<DietDayDto> {
        const res = await this.putAsync<{ isSuccess: boolean; value: DietDayDto }, CreateEditDietDayDto>(`/diet-days/${id}`, dto);
        return res.value;
    }

    static async deleteDietDay(id: number): Promise<void> {
        await this.deleteAsync<void>(`/diet-days/${id}`);
    }
}
