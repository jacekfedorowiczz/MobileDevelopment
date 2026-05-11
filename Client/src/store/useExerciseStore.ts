// src/store/useExerciseStore.ts
import { create } from 'zustand';
import { persist, createJSONStorage } from 'zustand/middleware';
import AsyncStorage from '@react-native-async-storage/async-storage';

export interface Exercise {
  id: string;
  name: string;
  muscleGroup: string;
  equipment?: string;
  description?: string;
  imageUrl?: string;
  isCustom?: boolean;
}

interface ExerciseState {
  exercises: Exercise[]; // Cached from API
  favorites: string[]; // IDs of favorite exercises
  customExercises: Exercise[];
  
  // Actions
  setExercises: (exercises: Exercise[]) => void;
  toggleFavorite: (exerciseId: string) => void;
  addCustomExercise: (exercise: Omit<Exercise, 'id' | 'isCustom'>) => void;
  removeCustomExercise: (exerciseId: string) => void;
}

export const useExerciseStore = create<ExerciseState>()(
  persist(
    (set) => ({
      exercises: [],
      favorites: [],
      customExercises: [],

      setExercises: (exercises) => set({ exercises }),

      toggleFavorite: (exerciseId) => set((state) => ({
        favorites: state.favorites.includes(exerciseId)
          ? state.favorites.filter(id => id !== exerciseId)
          : [...state.favorites, exerciseId]
      })),

      addCustomExercise: (exercise) => set((state) => {
        const newExercise: Exercise = {
          ...exercise,
          id: `custom-${Math.random().toString(36).substr(2, 9)}`,
          isCustom: true
        };
        return {
          customExercises: [...state.customExercises, newExercise]
        };
      }),

      removeCustomExercise: (exerciseId) => set((state) => ({
        customExercises: state.customExercises.filter(e => e.id !== exerciseId)
      })),
    }),
    {
      name: 'exercise-storage',
      storage: createJSONStorage(() => AsyncStorage),
    }
  )
);
