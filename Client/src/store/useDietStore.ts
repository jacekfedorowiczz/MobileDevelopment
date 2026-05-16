import { create } from 'zustand';
import { DietApiService, DietDto } from '../api/DietApiService';
import { DietDayApiService, DietDayDto } from '../api/DietDayApiService';
import { MealApiService, CreateEditMealDto } from '../api/MealApiService';

interface DietState {
  activeDiet: DietDto | null;
  activeDay: DietDayDto | null;
  isLoading: boolean;
  error: string | null;

  // Actions
  fetchActiveDiet: () => Promise<void>;
  selectDay: (dateStr: string) => Promise<void>;
  createDay: (dateStr: string, notes?: string) => Promise<void>;
  addMeal: (meal: CreateEditMealDto) => Promise<void>;
  removeMeal: (mealId: number) => Promise<void>;
  createDiet: (name: string, calories: number) => Promise<void>;
}

export const useDietStore = create<DietState>((set, get) => ({
  activeDiet: null,
  activeDay: null,
  isLoading: false,
  error: null,

  fetchActiveDiet: async () => {
    try {
      set({ isLoading: true, error: null });
      const diets = await DietApiService.getAllDiets();
      const dietList = Array.isArray(diets) ? diets : [];
      
      // Get the first active diet, or the most recent one
      const currentDiet = dietList.find(d => d.isActive) || dietList[0];
      
      if (currentDiet) {
        // Fetch full details with days and meals
        const fullDiet = await DietApiService.getDietById(currentDiet.id);
        
        // Find today's date
        const todayStr = new Date().toISOString().split('T')[0];
        let todayDay = fullDiet.dietDays?.find(d => d.date.startsWith(todayStr));
        
        // Auto-create day if not exists
        if (!todayDay) {
          todayDay = await DietDayApiService.createDietDay({
            dietId: fullDiet.id,
            date: new Date().toISOString()
          });
          if (!fullDiet.dietDays) fullDiet.dietDays = [];
          fullDiet.dietDays.push(todayDay);
        }

        set({ activeDiet: fullDiet, activeDay: todayDay, isLoading: false });
      } else {
        set({ activeDiet: null, activeDay: null, isLoading: false });
      }
    } catch (err: any) {
      set({ error: err.message || 'Wystąpił błąd podczas ładowania diety', isLoading: false });
    }
  },

  selectDay: async (dateStr: string) => {
    const { activeDiet } = get();
    if (!activeDiet) return;

    let day = activeDiet.dietDays?.find(d => d.date.startsWith(dateStr));
    
    if (!day) {
      try {
        set({ isLoading: true });
        day = await DietDayApiService.createDietDay({
          dietId: activeDiet.id,
          date: new Date(dateStr).toISOString()
        });
        
        // Update diet in state
        const updatedDiet = { ...activeDiet };
        if (!updatedDiet.dietDays) updatedDiet.dietDays = [];
        updatedDiet.dietDays.push(day);
        
        set({ activeDiet: updatedDiet, activeDay: day, isLoading: false });
      } catch (err: any) {
        set({ error: err.message, isLoading: false });
      }
    } else {
      set({ activeDay: day });
    }
  },

  createDay: async (dateStr: string, notes?: string) => {
    const { activeDiet } = get();
    if (!activeDiet) return;

    try {
      set({ isLoading: true });
      const existingDay = activeDiet.dietDays?.find(d => d.date.startsWith(dateStr));
      const day = existingDay ?? await DietDayApiService.createDietDay({
        dietId: activeDiet.id,
        date: new Date(dateStr).toISOString(),
        notes: notes?.trim() || undefined,
      });

      const updatedDiet = { ...activeDiet, dietDays: [...(activeDiet.dietDays || [])] };
      if (!existingDay) {
        updatedDiet.dietDays!.push(day);
      }

      set({ activeDiet: updatedDiet, activeDay: day, isLoading: false });
    } catch (err: any) {
      set({ error: err.message, isLoading: false });
    }
  },

  addMeal: async (meal: CreateEditMealDto) => {
    try {
      set({ isLoading: true });
      const newMeal = await MealApiService.createMeal(meal);
      
      const { activeDay, activeDiet } = get();
      if (activeDay && activeDiet) {
        const updatedDay = { ...activeDay, meals: [...(activeDay.meals || []), newMeal] };
        
        const updatedDiet = { ...activeDiet };
        if (updatedDiet.dietDays) {
          const dayIndex = updatedDiet.dietDays.findIndex(d => d.id === activeDay.id);
          if (dayIndex !== -1) {
            updatedDiet.dietDays[dayIndex] = updatedDay;
          }
        }
        
        set({ activeDay: updatedDay, activeDiet: updatedDiet, isLoading: false });
      }
    } catch (err: any) {
      set({ error: err.message, isLoading: false });
    }
  },

  removeMeal: async (mealId: number) => {
    try {
      set({ isLoading: true });
      await MealApiService.deleteMeal(mealId);
      
      const { activeDay, activeDiet } = get();
      if (activeDay && activeDiet) {
        const updatedDay = { ...activeDay, meals: activeDay.meals?.filter(m => m.id !== mealId) || [] };
        
        const updatedDiet = { ...activeDiet };
        if (updatedDiet.dietDays) {
          const dayIndex = updatedDiet.dietDays.findIndex(d => d.id === activeDay.id);
          if (dayIndex !== -1) {
            updatedDiet.dietDays[dayIndex] = updatedDay;
          }
        }
        
        set({ activeDay: updatedDay, activeDiet: updatedDiet, isLoading: false });
      }
    } catch (err: any) {
      set({ error: err.message, isLoading: false });
    }
  },

  createDiet: async (name: string, calories: number) => {
    try {
      set({ isLoading: true });
      const { activeDiet } = get();
      const dto = {
        userId: 0, // Ignorowane przez backend dla current user
        name,
        description: `Target: ${calories} kcal`,
        startDate: activeDiet?.startDate ?? new Date().toISOString(),
        endDate: activeDiet?.endDate,
      };

      if (activeDiet) {
        await DietApiService.updateDiet(activeDiet.id, dto);
      } else {
        await DietApiService.createDiet(dto);
      }

      await get().fetchActiveDiet();
    } catch (err: any) {
      set({ error: err.message, isLoading: false });
    }
  }
}));
