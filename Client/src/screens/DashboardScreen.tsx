// src/screens/DashboardScreen.tsx
import React, { useCallback, useRef, useState } from 'react';
import { View, Text, Pressable, ScrollView, ActivityIndicator } from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';
import { useFocusEffect } from '@react-navigation/native';
import Icon from 'react-native-vector-icons/Feather';
import { useTheme } from '../context/ThemeContext';
import { getDashboardStyles } from './DashboardScreen.styles';

import { DashboardService, DashboardSummaryData } from '../api/DashboardService';

export default function DashboardScreen({ navigation }: any) {
  const { colors } = useTheme();
  const styles = getDashboardStyles(colors);

  const [summaryData, setSummaryData] = useState<DashboardSummaryData | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const hasLoadedSummary = useRef(false);

  const fetchSummary = useCallback(async () => {
    if (!hasLoadedSummary.current) {
      setIsLoading(true);
    }

    try {
      const response = await DashboardService.getSummary();
      if (response.isSuccess && response.value) {
        setSummaryData(response.value);
        hasLoadedSummary.current = true;
      }
    } finally {
      setIsLoading(false);
    }
  }, []);

  useFocusEffect(
    useCallback(() => {
      fetchSummary();
    }, [fetchSummary])
  );

  const getTodayDateString = () => {
    const options: Intl.DateTimeFormatOptions = { weekday: 'long', day: 'numeric', month: 'long', year: 'numeric' };
    return new Date().toLocaleDateString('pl-PL', options);
  };

  const weeklyActivity = summaryData?.weeklyActivity ?? [];
  const maxMinutes = Math.max(...weeklyActivity.map(day => day.minutes), 1);
  const caloriesProgress = summaryData?.caloriesGoal
    ? Math.min(100, Math.round((summaryData.caloriesConsumedToday / summaryData.caloriesGoal) * 100))
    : 0;

  if (isLoading) {
    return (
      <SafeAreaView style={[styles.container, { justifyContent: 'center', alignItems: 'center' }]}>
        <ActivityIndicator size="large" color={colors.primary} />
      </SafeAreaView>
    );
  }

  return (
    <SafeAreaView style={styles.container}>
      <ScrollView contentContainerStyle={styles.scrollContent}>
        <Text style={styles.welcome}>Witaj z powrotem, {summaryData?.firstName || 'Użytkowniku'}!</Text>
        <Text style={styles.date}>{getTodayDateString()}</Text>

        <View style={styles.sectionSeparator} />
        <View style={styles.heroCards}>
          <Pressable style={styles.heroCard} onPress={() => navigation.navigate('Training', { screen: 'Diet' })}>
            <View style={styles.heroHeader}>
              <View style={styles.heroIconWrapper}>
                <Icon name="coffee" size={22} color="#2563eb" />
              </View>
              <Text style={styles.heroTitle}>Dieta</Text>
            </View>
            <Text style={styles.heroValue}>{summaryData?.caloriesConsumedToday ?? 0}</Text>
            <Text style={styles.heroSubtitle}>z {summaryData?.caloriesGoal ?? 0} kcal dzisiaj</Text>
            <View style={styles.progressTrack}>
              <View style={[styles.progressFill, { width: `${caloriesProgress}%` }]} />
            </View>
            <Text style={styles.macroText}>
              B {summaryData?.proteinToday ?? 0}g · W {summaryData?.carbsToday ?? 0}g · T {summaryData?.fatToday ?? 0}g
            </Text>
          </Pressable>

          <Pressable style={styles.heroCard} onPress={() => navigation.navigate('Training', { screen: 'WorkoutLog' })}>
            <View style={styles.heroHeader}>
              <View style={styles.heroIconWrapper}>
                <Icon name="activity" size={22} color="#2563eb" />
              </View>
              <Text style={styles.heroTitle}>Treningi</Text>
            </View>
            <Text style={styles.heroValue}>{summaryData?.totalWorkouts ?? 0}</Text>
            <Text style={styles.heroSubtitle}>treningów w ostatnich 7 dniach</Text>
            <View style={styles.heroStatsRow}>
              <Text style={styles.heroStat}>{summaryData?.totalSets ?? 0} serii</Text>
              <Text style={styles.heroStat}>{summaryData?.workoutMinutesThisWeek ?? 0} min</Text>
            </View>
          </Pressable>
        </View>

        <View style={styles.sectionSeparator} />
        <View style={styles.statsRow}>
          <Pressable style={styles.statBox} onPress={() => navigation.navigate('Training', { screen: 'Diet' })}>
            <View style={styles.statIconWrapper}>
              <Icon name="zap" size={20} color="#2563eb" />
            </View>
            <Text style={styles.statValue}>{summaryData?.caloriesConsumedToday || 0}</Text>
            <Text style={styles.statLabel}>kcal dzisiaj</Text>
          </Pressable>
          <Pressable style={styles.statBox} onPress={() => navigation.navigate('Training', { screen: 'TrainingHub' })}>
            <View style={styles.statIconWrapper}>
              <Icon name="target" size={20} color="#2563eb" />
            </View>
            <Text style={styles.statValue}>{summaryData?.totalSets || 0}</Text>
            <Text style={styles.statLabel}>Serie</Text>
          </Pressable>
          <Pressable style={styles.statBox} onPress={() => navigation.navigate('Training', { screen: 'WorkoutLog' })}>
            <View style={styles.statIconWrapper}>
              <Icon name="activity" size={20} color="#2563eb" />
            </View>
            <Text style={styles.statValue}>{summaryData?.totalWorkouts || 0}</Text>
            <Text style={styles.statLabel}>Treningi</Text>
          </Pressable>
        </View>

        <View style={styles.sectionSeparator} />
        <Text style={styles.sectionHeader}>Aktywność w tym tygodniu</Text>
        <View style={styles.chartCard}>
          <View style={styles.chartBars}>
            {weeklyActivity.map(day => (
              <View key={day.day} style={styles.chartColumn}>
                <View style={styles.chartBarTrack}>
                  <View style={[styles.chartBar, { height: `${Math.max(8, (day.minutes / maxMinutes) * 100)}%` }]} />
                </View>
                <Text style={styles.chartLabel}>{day.day}</Text>
                <Text style={styles.chartValue}>{day.minutes}</Text>
              </View>
            ))}
          </View>
        </View>

        <View style={styles.sectionSeparator} />
        <Text style={styles.sectionHeader}>Ostatnie treningi</Text>
        {summaryData?.recentWorkouts?.length ? summaryData.recentWorkouts.map(w => (
          <Pressable
            key={w.id}
            style={styles.workoutItem}
            onPress={() => navigation.navigate('Training', { screen: 'WorkoutDetail', params: { id: w.id } })}
          >
            <View>
              <Text style={styles.workoutName}>{w.name}</Text>
              <Text style={styles.workoutDetails}>
                {w.exercisesCount} ćwiczeń · {w.duration}
              </Text>
            </View>
            <View style={styles.workoutDate}>
              <Text style={styles.workoutDateText}>{w.date}</Text>
            </View>
          </Pressable>
        )) : (
          <Text style={styles.emptyText}>Brak ostatnich treningów.</Text>
        )}
      </ScrollView>
    </SafeAreaView>
  );
}
