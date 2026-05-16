import { BaseApiService } from './BaseApiService';

export interface ProfileData {
  firstName: string;
  lastName: string;
  email: string;
  profileImageUrl?: string;
  workoutsThisMonth: number;
  averageWorkoutTime: number;
  achievementsCount: number;
  weight?: number;
  height?: number;
  preferredWeightUnit?: number;
  currentGoal?: number;
  dietType?: string;
  dailyCaloriesGoal?: number;
  proteinPercentage?: number;
  carbsPercentage?: number;
  fatPercentage?: number;
}

interface ApiResult<T> {
  isSuccess: boolean;
  value?: T;
  errorMessage?: string;
}

export interface UpdateProfileData {
  firstName: string;
  lastName: string;
  email: string;
  weight?: number;
  height?: number;
  preferredWeightUnit?: number;
  currentGoal?: number;
}

export interface UpdateDietAssumptionsData {
  dietType: string;
  dailyCaloriesGoal: number;
  proteinPercentage: number;
  carbsPercentage: number;
  fatPercentage: number;
}

export class ProfileService extends BaseApiService {
  public static async getProfile(): Promise<{ isSuccess: boolean; value?: ProfileData; errorMessage?: string }> {
    try {
      const response = await this.getAsync<ApiResult<ProfileData> | ProfileData>('/profile/me');
      if ('isSuccess' in response) {
        return {
          isSuccess: response.isSuccess,
          value: response.value,
          errorMessage: response.errorMessage,
        };
      }

      return { isSuccess: true, value: response };
    } catch (error: any) {
      return { isSuccess: false, errorMessage: error.message || 'Wystąpił błąd podczas pobierania profilu.' };
    }
  }

  public static async updateProfile(data: UpdateProfileData): Promise<{ isSuccess: boolean; errorMessage?: string }> {
    try {
      await this.putAsync<any>('/profile/me', data);
      return { isSuccess: true };
    } catch (error: any) {
      return { isSuccess: false, errorMessage: error.message || 'Wystąpił błąd podczas aktualizacji profilu.' };
    }
  }

  public static async updateDietAssumptions(data: UpdateDietAssumptionsData): Promise<{ isSuccess: boolean; errorMessage?: string }> {
    try {
      await this.putAsync<any, UpdateDietAssumptionsData>('/profile/me/diet-assumptions', data);
      return { isSuccess: true };
    } catch (error: any) {
      return { isSuccess: false, errorMessage: error.message || 'Wystąpił błąd podczas zapisywania założeń diety.' };
    }
  }

  public static async uploadAvatar(imageUri: string, fileName: string, type: string): Promise<{ isSuccess: boolean; value?: string; errorMessage?: string }> {
    try {
      const formData = new FormData();
      formData.append('file', {
        uri: imageUri,
        name: fileName,
        type: type,
      } as any);

      const data = await this.postFormAsync<any>('/profile/me/avatar', formData);
      return { isSuccess: true, value: data?.value ?? data };
    } catch (error: any) {
      return { isSuccess: false, errorMessage: error.message || 'Wystąpił błąd podczas wgrywania zdjęcia.' };
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
