import { BaseApiService } from './BaseApiService';
import type { PagedResult } from './ExerciseApiService';

export interface GymDto {
    id: number;
    name: string;
    street: string;
    city: string;
    zipCode: string;
    latitude: number;
    longitude: number;
    rating: number;
    description?: string;
    isActive: boolean;
    createdByUserId?: number;
}

export interface CreateEditGymDto {
    name: string;
    street: string;
    city: string;
    zipCode: string;
    latitude: number;
    longitude: number;
    rating: number;
    description?: string;
}

export class GymApiService extends BaseApiService {
    static async getAllGyms(search?: string): Promise<GymDto[]> {
        const url = search ? `/gyms?search=${encodeURIComponent(search)}` : '/gyms';
        const res = await this.getAsync<{ isSuccess: boolean; value: GymDto[] }>(url);
        return res?.value ?? [];
    }

    static async getPagedGyms(pageNumber = 1, pageSize = 20): Promise<PagedResult<GymDto>> {
        const res = await this.getAsync<{ isSuccess: boolean; value: PagedResult<GymDto> }>(
            `/gyms/paged?pageNumber=${pageNumber}&pageSize=${pageSize}`
        );
        return res.value;
    }

    static async getGymById(id: number): Promise<GymDto> {
        const res = await this.getAsync<{ isSuccess: boolean; value: GymDto }>(`/gyms/${id}`);
        return res.value;
    }

    static async createGym(dto: CreateEditGymDto): Promise<GymDto> {
        const res = await this.postAsync<{ isSuccess?: boolean; value?: GymDto } | GymDto, CreateEditGymDto>('/gyms', dto);
        return 'value' in res && res.value ? res.value : res as GymDto;
    }

    static async updateGym(id: number, dto: CreateEditGymDto): Promise<GymDto> {
        const res = await this.putAsync<{ isSuccess: boolean; value: GymDto }, CreateEditGymDto>(`/gyms/${id}`, dto);
        return res.value;
    }

    static async deleteGym(id: number): Promise<void> {
        await this.deleteAsync<void>(`/gyms/${id}`);
    }
}
