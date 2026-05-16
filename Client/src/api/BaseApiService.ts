import axios, { type AxiosError, type AxiosRequestConfig, type Method } from 'axios';
import { API_BASE_URL } from './api-config';
import { useAuthStore } from '../store/useAuthStore';

export class BaseApiService {
    private static refreshPromise: Promise<string> | null = null;
    private static readonly api = axios.create({
        baseURL: API_BASE_URL,
        headers: {
            'Accept': 'application/json',
        },
    });

    private static getHeaders(includeContentType = true): Record<string, string> {
        const headers: Record<string, string> = {
            'Accept': 'application/json',
        };
        if (includeContentType) {
            headers['Content-Type'] = 'application/json';
        }

        const token = useAuthStore.getState().userToken;
        if (token) {
            headers.Authorization = `Bearer ${token}`;
        }

        return headers;
    }

    protected static async getAsync<T>(endpoint: string): Promise<T> {
        return this.requestAsync<T>(endpoint, { method: 'GET' });
    }

    protected static async postAsync<T, U = any>(endpoint: string, data: U): Promise<T> {
        return this.requestAsync<T>(endpoint, { method: 'POST', data });
    }

    protected static async putAsync<T, U = any>(endpoint: string, data: U): Promise<T> {
        return this.requestAsync<T>(endpoint, { method: 'PUT', data });
    }

    protected static async deleteAsync<T, U = undefined>(endpoint: string, data?: U): Promise<T> {
        return this.requestAsync<T>(endpoint, { method: 'DELETE', data });
    }

    protected static async postFormAsync<T>(endpoint: string, formData: FormData): Promise<T> {
        return this.requestAsync<T>(endpoint, {
            method: 'POST',
            data: formData,
        });
    }

    private static async requestAsync<T>(endpoint: string, config: AxiosRequestConfig): Promise<T> {
        const isFormData = typeof FormData !== 'undefined' && config.data instanceof FormData;
        try {
            const response = await this.api.request<T>({
                ...config,
                url: endpoint,
                method: config.method as Method,
                headers: {
                    ...this.getHeaders(!isFormData),
                    ...config.headers,
                },
            });

            return (response.status === 204 ? {} : response.data) as T;
        } catch (error) {
            if (!axios.isAxiosError(error) || error.response?.status !== 401) {
                throw this.toApiError(error);
            }

            try {
                await this.handleTokenRefresh();

                const retryResponse = await this.api.request<T>({
                    ...config,
                    url: endpoint,
                    method: config.method as Method,
                    headers: {
                        ...this.getHeaders(!isFormData),
                        ...config.headers,
                    },
                });

                return (retryResponse.status === 204 ? {} : retryResponse.data) as T;
            } catch {
                useAuthStore.getState().logout();
                throw new Error("Sesja wygasła. Zaloguj się ponownie.");
            }
        }
    }

    private static async handleTokenRefresh(): Promise<string> {
        if (this.refreshPromise) {
            return this.refreshPromise;
        }

        this.refreshPromise = (async () => {
            const refreshToken = useAuthStore.getState().refreshToken;
            if (!refreshToken) {
                throw new Error("Brak refresh tokena.");
            }

            try {
                const response = await this.api.post('/user/refresh', { refreshToken });
                const result = response.data;

                if (!result.isSuccess || !result.value) {
                    throw new Error(result.errorMessage || "Failed to refresh token");
                }

                const newToken = result.value.accessToken;
                const newRefreshToken = result.value.refreshToken;

                useAuthStore.getState().setAuth(newToken, newRefreshToken);
                return newToken;
            } catch (error) {
                // Jeśli w trakcie odświeżania wystąpi błąd (np. zła sieć, wygasły refresh),
                // wylogowujemy profilaktycznie już tutaj.
                useAuthStore.getState().logout();
                throw error;
            } finally {
                this.refreshPromise = null;
            }
        })();

        return this.refreshPromise;
    }

    private static toApiError(error: unknown): Error {
        if (!axios.isAxiosError(error)) {
            return error instanceof Error ? error : new Error('Nieznany błąd API.');
        }

        const axiosError = error as AxiosError<any>;
        const status = axiosError.response?.status;
        const statusText = axiosError.response?.statusText;
        const data = axiosError.response?.data;

        let errorMessage = status ? `API Error: ${status} ${statusText ?? ''}`.trim() : axiosError.message;
        if (data?.messages?.length) {
            errorMessage = data.messages.join(', ');
        } else if (data?.message) {
            errorMessage = data.message;
        } else if (data?.errorMessage) {
            errorMessage = data.errorMessage;
        } else if (typeof data === 'string') {
            errorMessage = data;
        }

        return new Error(errorMessage);
    }
}