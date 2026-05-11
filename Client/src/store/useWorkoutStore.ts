// src/store/useWorkoutStore.ts
import { create } from 'zustand';
import { persist, createJSONStorage } from 'zustand/middleware';
import AsyncStorage from '@react-native-async-storage/async-storage';

export interface WorkoutSet {
  id: string;
  exerciseId: string;
  exerciseName: string;
  weight: number;
  reps: number;
  rpe?: number;
}

export interface WorkoutSession {
  id?: string;
  name: string;
  startTime: string;
  endTime?: string;
  sets: WorkoutSet[];
}

interface WorkoutState {
  activeSession: WorkoutSession | null;
  isTraining: boolean;
  
  // Actions
  startWorkout: (name: string) => void;
  endWorkout: () => void;
  addSet: (exerciseId: string, exerciseName: string) => void;
  updateSet: (setId: string, data: Partial<WorkoutSet>) => void;
  removeSet: (setId: string) => void;
  clearSession: () => void;
}

export const useWorkoutStore = create<WorkoutState>()(
  persist(
    (set) => ({
      activeSession: null,
      isTraining: false,

      startWorkout: (name) => set({
        isTraining: true,
        activeSession: {
          name,
          startTime: new Date().toISOString(),
          sets: [],
        }
      }),

      endWorkout: () => set((state) => ({
        isTraining: false,
        activeSession: state.activeSession 
          ? { ...state.activeSession, endTime: new Date().toISOString() } 
          : null
      })),

      addSet: (exerciseId, exerciseName) => set((state) => {
        if (!state.activeSession) return state;
        
        const newSet: WorkoutSet = {
          id: Math.random().toString(36).substr(2, 9),
          exerciseId,
          exerciseName,
          weight: 0,
          reps: 0,
        };

        return {
          activeSession: {
            ...state.activeSession,
            sets: [...state.activeSession.sets, newSet]
          }
        };
      }),

      updateSet: (setId, data) => set((state) => {
        if (!state.activeSession) return state;

        return {
          activeSession: {
            ...state.activeSession,
            sets: state.activeSession.sets.map(s => 
              s.id === setId ? { ...s, ...data } : s
            )
          }
        };
      }),

      removeSet: (setId) => set((state) => {
        if (!state.activeSession) return state;

        return {
          activeSession: {
            ...state.activeSession,
            sets: state.activeSession.sets.filter(s => s.id !== setId)
          }
        };
      }),

      clearSession: () => set({ activeSession: null, isTraining: false }),
    }),
    {
      name: 'workout-storage',
      storage: createJSONStorage(() => AsyncStorage),
    }
  )
);
