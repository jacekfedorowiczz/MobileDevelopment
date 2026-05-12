import { BaseApiService } from './BaseApiService';

export interface ActivityDayData {
    day: string;
    minutes: number;
}

export interface RecentWorkoutData {
    name: string;
    exercisesCount: number;
    duration: string;
    date: string;
}

export interface DashboardSummaryData {
    firstName: string;
    caloriesBurned: number;
    totalSets: number;
    totalWorkouts: number;
    weeklyActivity: ActivityDayData[];
    recentWorkouts: RecentWorkoutData[];
}

export class DashboardService extends BaseApiService {
    public static async getSummary(): Promise<{ isSuccess: boolean; value?: DashboardSummaryData; errorMessage?: string }> {
        try {
            const response = await this.getAsync<{ isSuccess: boolean, value: DashboardSummaryData }>('/dashboard/summary');
            return { isSuccess: true, value: response.value };
        } catch (error: any) {
            return { isSuccess: false, errorMessage: error.message || 'Wystąpił błąd podczas pobierania podsumowania dashboardu.' };
        }
    }
}
