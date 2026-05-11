import { BaseApiService } from './BaseApiService';

export interface LoginRequest {
    login: string;
    passwordHash: string; // Zakładam, że Twoje DTO tak się nazywa, w razie czego zmień na `password`
}

export interface AuthResponse {
    token: string;
    refreshToken: string;
    // Prawdopodobnie Twoje Result<T> zwraca coś w stylu { data: { token: ... }, messages: [] }
    // Jeśli używasz Wrappera Result<T>, musisz to tutaj uwzględnić. Zakładam prosty zwrot.
}

// Interfejs zgodny ze standardowym wzorcem CQRS w C# (Result<T>)
export interface Result<T> {
    succeeded: boolean;
    data: T;
    messages: string[];
}

export class AuthService extends BaseApiService {
    public static async loginAsync(request: LoginRequest): Promise<Result<AuthResponse>> {
        // Zmień endpoint na prawidłowy wg. Twojego WebAPI (np. /auth/login, /users/login)
        return this.postAsync<Result<AuthResponse>>('/auth/login', request);
    }
}
