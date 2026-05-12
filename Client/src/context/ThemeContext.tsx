// src/context/ThemeContext.tsx
import React, { useEffect } from 'react';
import AsyncStorage from '@react-native-async-storage/async-storage';
import { useColorScheme } from 'react-native';
import { useThemeStore } from '../store/useThemeStore';
import { LightColors, DarkColors } from '../theme/theme';

export const ThemeProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const systemColorScheme = useColorScheme();
  const setTheme = useThemeStore(state => state.setTheme);

  useEffect(() => {
    const loadTheme = async () => {
      try {
        const savedTheme = await AsyncStorage.getItem('userTheme');
        if (savedTheme === 'light' || savedTheme === 'dark') {
          setTheme(savedTheme === 'dark');
        } else {
          setTheme(systemColorScheme === 'dark');
        }
      } catch (e) {
        console.error('Failed to load theme', e);
      }
    };
    loadTheme();
  }, [systemColorScheme, setTheme]);

  // Sync theme changes to AsyncStorage
  const isDark = useThemeStore(state => state.isDark);
  useEffect(() => {
    AsyncStorage.setItem('userTheme', isDark ? 'dark' : 'light').catch(e => console.error(e));
  }, [isDark]);

  return <>{children}</>;
};

export const useTheme = () => {
  const { isDark, toggleTheme, setTheme } = useThemeStore();
  const theme = isDark ? 'dark' : 'light';
  const colors = isDark ? DarkColors : LightColors;
  
  return {
    theme,
    colors,
    isDark,
    toggleTheme,
    setTheme: (t: 'light' | 'dark') => setTheme(t === 'dark')
  };
};
