import { BaseApiService } from './BaseApiService';

export interface AuthResponse {
    isSuccess: boolean;
    value: {
        accessToken: string;
        refreshToken: string;
        accessTokenExpiration: string;
    };
    errorMessage?: string;
}

export class UserService extends BaseApiService {
    public static async login(data: any): Promise<AuthResponse> {
        return this.postAsync<AuthResponse>('/user/login', data);
    }

    public static async register(data: any): Promise<AuthResponse> {
        return this.postAsync<AuthResponse>('/user/register', data);
    }

    public static async getMe(): Promise<any> {
        return this.getAsync<any>('/user/me');
    }
}
