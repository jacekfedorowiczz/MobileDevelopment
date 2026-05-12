import { create } from 'zustand';
import { persist, createJSONStorage } from 'zustand/middleware';
import AsyncStorage from '@react-native-async-storage/async-storage';

interface AuthState {
    userToken: string | null;
    refreshToken: string | null;
    isAuthenticated: boolean;
    setAuth: (token: string, refreshToken: string) => void;
    logout: () => void;
}

export const useAuthStore = create<AuthState>()(
    persist(
        (set) => ({
            userToken: null,
            refreshToken: null,
            isAuthenticated: false,

            setAuth: (token, refreshToken) => set({
                userToken: token,
                refreshToken: refreshToken,
                isAuthenticated: true
            }),

            logout: () => {
                set({
                    userToken: null,
                    refreshToken: null,
                    isAuthenticated: false
                });
            }
        }),
        {
            name: 'auth-storage',
            storage: createJSONStorage(() => AsyncStorage),
        }
    )
);
