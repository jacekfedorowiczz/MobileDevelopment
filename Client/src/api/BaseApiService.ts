import AsyncStorage from '@react-native-async-storage/async-storage';
import { API_BASE_URL } from './api-config';
import { useAuthStore } from '../store/useAuthStore';

export class BaseApiService {
    private static isRefreshing = false;
    private static refreshSubscribers: ((token: string) => void)[] = [];

    private static onTokenRefreshed(token: string) {
        this.refreshSubscribers.map(cb => cb(token));
        this.refreshSubscribers = [];
    }

    private static addRefreshSubscriber(cb: (token: string) => void) {
        this.refreshSubscribers.push(cb);
    }

    private static async getHeaders(): Promise<HeadersInit> {
        const headers: HeadersInit = {
            'Content-Type': 'application/json',
            'Accept': 'application/json',
        };

        const token = useAuthStore.getState().userToken;
        if (token) {
            headers['Authorization'] = `Bearer ${token}`;
        }

        return headers;
    }

    protected static async getAsync<T>(endpoint: string): Promise<T> {
        return this.requestAsync<T>(endpoint, { method: 'GET' });
    }

    protected static async postAsync<T, U = any>(endpoint: string, data: U): Promise<T> {
        return this.requestAsync<T>(endpoint, {
            method: 'POST',
            body: JSON.stringify(data),
        });
    }

    protected static async putAsync<T, U = any>(endpoint: string, data: U): Promise<T> {
        return this.requestAsync<T>(endpoint, {
            method: 'PUT',
            body: JSON.stringify(data),
        });
    }

    protected static async deleteAsync<T>(endpoint: string): Promise<T> {
        return this.requestAsync<T>(endpoint, { method: 'DELETE' });
    }

    private static async requestAsync<T>(endpoint: string, options: RequestInit): Promise<T> {
        const headers = await this.getHeaders();
        const config = { ...options, headers };

        let response = await fetch(`${API_BASE_URL}${endpoint}`, config);

        if (response.status === 401) {
            // Próba odświeżenia tokenu
            try {
                const newToken = await this.handleTokenRefresh();
                
                // Ponów żądanie z nowym tokenem
                const newHeaders = await this.getHeaders();
                const retryResponse = await fetch(`${API_BASE_URL}${endpoint}`, {
                    ...options,
                    headers: newHeaders
                });
                
                await this.handleErrors(retryResponse);
                if (retryResponse.status === 204) return {} as T;
                return retryResponse.json();
            } catch (error) {
                // Jeśli odświeżanie się nie uda, wyloguj użytkownika
                useAuthStore.getState().logout();
                throw new Error("Sesja wygasła. Zaloguj się ponownie.");
            }
        }

        await this.handleErrors(response);
        if (response.status === 204) return {} as T;
        return response.json();
    }

    private static async handleTokenRefresh(): Promise<string> {
        if (this.isRefreshing) {
            return new Promise(resolve => {
                this.addRefreshSubscriber(token => resolve(token));
            });
        }

        this.isRefreshing = true;

        const refreshToken = useAuthStore.getState().refreshToken;
        if (!refreshToken) {
            this.isRefreshing = false;
            throw new Error("Brak refresh tokena.");
        }

        try {
            const response = await fetch(`${API_BASE_URL}/user/refresh`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ refreshToken })
            });

            if (!response.ok) throw new Error("Failed to refresh token");

            const result = await response.json();
            
            if (!result.isSuccess || !result.value) {
                throw new Error(result.errorMessage || "Failed to refresh token");
            }

            const newToken = result.value.accessToken;
            const newRefreshToken = result.value.refreshToken;

            useAuthStore.getState().setAuth(newToken, newRefreshToken);

            this.onTokenRefreshed(newToken);
            return newToken;
        } finally {
            this.isRefreshing = false;
        }
    }

    private static async handleErrors(response: Response): Promise<void> {
        if (!response.ok) {
            let errorMessage = `API Error: ${response.status} ${response.statusText}`;
            try {
                const errorData = await response.json();
                if (errorData && errorData.messages && errorData.messages.length > 0) {
                    errorMessage = errorData.messages.join(', ');
                } else if (errorData && errorData.message) {
                    errorMessage = errorData.message;
                }
            } catch (e) {}
            throw new Error(errorMessage);
        }
    }
}
