import { BaseApiService } from './BaseApiService';

export interface LoginRequest {
    login: string;
    passwordHash: string; 
}

export interface AuthResponse {
    token: string;
    refreshToken: string;
}

export interface Result<T> {
    succeeded: boolean;
    data: T;
    messages: string[];
}

export class AuthService extends BaseApiService {
    public static async loginAsync(request: LoginRequest): Promise<Result<AuthResponse>> {
        return this.postAsync<Result<AuthResponse>>('/auth/login', request);
    }
}
