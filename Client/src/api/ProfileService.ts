import { BaseApiService } from './BaseApiService';

export interface ProfileData {
  firstName: string;
  lastName: string;
  email: string;
  profileImageUrl?: string;
  workoutsThisMonth: number;
  averageWorkoutTime: number;
  achievementsCount: number;
}

export class ProfileService extends BaseApiService {
  public static async getProfile(): Promise<{ isSuccess: boolean; value?: ProfileData; errorMessage?: string }> {
    try {
      const response = await this.getAsync<ProfileData>('/profile/me');
      return { isSuccess: true, value: response };
    } catch (error: any) {
      return { isSuccess: false, errorMessage: error.message || 'Wystąpił błąd podczas pobierania profilu.' };
    }
  }

  public static async logout(): Promise<{ isSuccess: boolean; errorMessage?: string }> {
    try {
      await this.postAsync<any>('/user/logout', {});
      return { isSuccess: true };
    } catch (error: any) {
      return { isSuccess: false, errorMessage: error.message || 'Błąd podczas wylogowywania.' };
    }
  }
}
