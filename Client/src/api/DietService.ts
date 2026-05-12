import { BaseApiService } from './BaseApiService';

export interface DietSummary {
    caloriesConsumed: number;
    caloriesGoal: number;
    protein: number;
    carbs: number;
    fat: number;
}

export class DietService extends BaseApiService {
    public static async getDietSummary(): Promise<DietSummary> {
        return await this.getAsync<DietSummary>('/diet/summary');
    }
}
