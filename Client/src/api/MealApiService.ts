import { BaseApiService } from './BaseApiService';
import type { PagedResult } from './ExerciseApiService';

export interface MealDto {
    id: number;
    dietDayId: number;
    name: string;
    time?: string;
    totalCalories: number;
    protein: number;
    carbs: number;
    fats: number;
}

export interface CreateEditMealDto {
    dietDayId: number;
    name: string;
    time?: string;
    totalCalories: number;
    protein: number;
    carbs: number;
    fats: number;
}

export class MealApiService extends BaseApiService {
    static async getAllMeals(dietDayId: number): Promise<MealDto[]> {
        const res = await this.getAsync<{ isSuccess: boolean; value: MealDto[] }>(`/meals/by-diet-day/${dietDayId}`);
        return res.value;
    }

    static async getPagedMeals(dietDayId: number, pageNumber = 1, pageSize = 10): Promise<PagedResult<MealDto>> {
        const res = await this.getAsync<{ isSuccess: boolean; value: PagedResult<MealDto> }>(
            `/meals/by-diet-day/${dietDayId}/paged?pageNumber=${pageNumber}&pageSize=${pageSize}`
        );
        return res.value;
    }

    static async getMealById(id: number): Promise<MealDto> {
        const res = await this.getAsync<{ isSuccess: boolean; value: MealDto }>(`/meals/${id}`);
        return res.value;
    }

    static async createMeal(dto: CreateEditMealDto): Promise<MealDto> {
        const res = await this.postAsync<{ isSuccess: boolean; value: MealDto }, CreateEditMealDto>('/meals', dto);
        return res.value;
    }

    static async updateMeal(id: number, dto: CreateEditMealDto): Promise<MealDto> {
        const res = await this.putAsync<{ isSuccess: boolean; value: MealDto }, CreateEditMealDto>(`/meals/${id}`, dto);
        return res.value;
    }

    static async deleteMeal(id: number): Promise<void> {
        await this.deleteAsync<void>(`/meals/${id}`);
    }
}
