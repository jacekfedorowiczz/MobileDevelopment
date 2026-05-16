import { BaseApiService } from './BaseApiService';

export interface AchievementDto {
  id: number;
  name: string;
  description: string;
  iconCode: string;
  achievementType: string;
  targetValue: number;
}

export interface ProfileAchievementDto {
  id: number;
  achievement: AchievementDto;
  unlockedAt: string;
}

export class AchievementApiService extends BaseApiService {
  
  static async getAllAchievements(): Promise<AchievementDto[]> {
    const res = await this.getAsync<{ isSuccess: boolean; value: AchievementDto[] }>('/achievements');
    return res?.value ?? [];
  }

  static async getMyAchievements(): Promise<ProfileAchievementDto[]> {
    const res = await this.getAsync<{ isSuccess: boolean; value: ProfileAchievementDto[] }>('/achievements/my');
    return res?.value ?? [];
  }
}