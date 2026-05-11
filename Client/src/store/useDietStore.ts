// src/store/useDietStore.ts
import { create } from 'zustand';
import { persist, createJSONStorage } from 'zustand/middleware';
import AsyncStorage from '@react-native-async-storage/async-storage';

export interface FoodItem {
  id: string;
  name: string;
  calories: number;
  protein: number;
  carbs: number;
  fat: number;
  amount: number; // in grams or units
}

export interface Meal {
  id: string;
  name: string;
  time: string;
  items: FoodItem[];
}

interface DietState {
  dailyMeals: Meal[];
  targetCalories: number;
  targetProtein: number;
  targetCarbs: number;
  targetFat: number;
  
  // Actions
  addMeal: (meal: Omit<Meal, 'id'>) => void;
  removeMeal: (mealId: string) => void;
  updateMeal: (mealId: string, data: Partial<Meal>) => void;
  setTargets: (targets: { calories?: number; protein?: number; carbs?: number; fat?: number }) => void;
  clearDailyMeals: () => void;
}

export const useDietStore = create<DietState>()(
  persist(
    (set) => ({
      dailyMeals: [],
      targetCalories: 2500,
      targetProtein: 150,
      targetCarbs: 250,
      targetFat: 70,

      addMeal: (meal) => set((state) => ({
        dailyMeals: [...state.dailyMeals, { ...meal, id: Math.random().toString(36).substr(2, 9) }]
      })),

      removeMeal: (mealId) => set((state) => ({
        dailyMeals: state.dailyMeals.filter(m => m.id !== mealId)
      })),

      updateMeal: (mealId, data) => set((state) => ({
        dailyMeals: state.dailyMeals.map(m => m.id === mealId ? { ...m, ...data } : m)
      })),

      setTargets: (targets) => set((state) => ({
        ...state,
        ...targets
      })),

      clearDailyMeals: () => set({ dailyMeals: [] }),
    }),
    {
      name: 'diet-storage',
      storage: createJSONStorage(() => AsyncStorage),
    }
  )
);
