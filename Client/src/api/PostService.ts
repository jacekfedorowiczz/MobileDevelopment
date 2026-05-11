import { BaseApiService, Result } from './BaseApiService';

export interface Post {
    id: number;
    author: string;
    avatarUrl: string;
    time: string;
    content: string;
    imageUrl?: string | null;
    likes: number;
    comments: number;
    category: string;
}

export interface PagedResponse<T> {
    items: T[];
    pageNumber: number;
    totalPages: number;
    totalCount: number;
    hasPreviousPage: boolean;
    hasNextPage: boolean;
}

export class PostService extends BaseApiService {
    public static async getPostsAsync(pageNumber: number, pageSize: number): Promise<Result<PagedResponse<Post>>> {
        // Zmień endpoint na /posts lub inny wg. Twojej konfiguracji
        // Symulacja parametrów: ?pageNumber=1&pageSize=10
        return this.getAsync<Result<PagedResponse<Post>>>(`/posts?pageNumber=${pageNumber}&pageSize=${pageSize}`);
    }
}
