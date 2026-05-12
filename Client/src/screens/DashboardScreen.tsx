// src/screens/DashboardScreen.tsx
import React, { useState, useEffect } from 'react';
import { View, Text, Pressable, ScrollView, ActivityIndicator } from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';
import Icon from 'react-native-vector-icons/Feather';
import { useTheme } from '../context/ThemeContext';
import { getDashboardStyles } from './DashboardScreen.styles';

import { DashboardService, DashboardSummaryData } from '../api/DashboardService';

const quickActions = [
  { title: 'Dieta', description: 'Zaplanuj posiłki', icon: 'apple', stack: 'Training', screen: 'Diet' },
  { title: 'Plan treningowy', description: 'Dzisiejszy trening', icon: 'activity', stack: 'Training', screen: 'TrainingHub' },
];

export default function DashboardScreen({ navigation }: any) {
  const { colors } = useTheme();
  const styles = getDashboardStyles(colors);

  const [summaryData, setSummaryData] = useState<DashboardSummaryData | null>(null);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    fetchSummary();
  }, []);

  const fetchSummary = async () => {
    setIsLoading(true);
    const response = await DashboardService.getSummary();
    if (response.isSuccess && response.value) {
      setSummaryData(response.value);
    }
    setIsLoading(false);
  };

  const getTodayDateString = () => {
    const options: Intl.DateTimeFormatOptions = { weekday: 'long', day: 'numeric', month: 'long', year: 'numeric' };
    return new Date().toLocaleDateString('pl-PL', options);
  };

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

        <View style={styles.actionsRow}>
          {quickActions.map((action, idx) => (
            <Pressable
              key={idx}
              style={styles.actionButton}
              onPress={() => navigation.navigate(action.stack, { screen: action.screen })}
            >
              <View style={styles.actionIconWrapper}>
                <Icon name={action.icon} size={24} color="#2563eb" />
              </View>
              <Text style={styles.actionTitle}>{action.title}</Text>
              <Text style={styles.actionDesc}>{action.description}</Text>
            </Pressable>
          ))}
        </View>

        {/* Placeholder sections for progress, calories, series, trainings */}
        <View style={styles.placeholderBox}>
          <Text style={styles.placeholderText}>Progress</Text>
        </View>

        <View style={styles.statsRow}>
          <Pressable style={styles.statBox} onPress={() => navigation.navigate('Training', { screen: 'Diet' })}>
            <View style={styles.statIconWrapper}>
              <Icon name="fire" size={20} color="#2563eb" />
            </View>
            <Text style={styles.statValue}>{summaryData?.caloriesBurned || 0}</Text>
            <Text style={styles.statLabel}>Kalorie</Text>
          </Pressable>
          <Pressable style={styles.statBox} onPress={() => navigation.navigate('Training', { screen: 'TrainingHub' })}>
            <View style={styles.statIconWrapper}>
              <Icon name="target" size={20} color="#2563eb" />
            </View>
            <Text style={styles.statValue}>{summaryData?.totalSets || 0}</Text>
            <Text style={styles.statLabel}>Seria</Text>
          </Pressable>
          <Pressable style={styles.statBox} onPress={() => navigation.navigate('Training', { screen: 'WorkoutLog' })}>
            <View style={styles.statIconWrapper}>
              <Icon name="dumbbell" size={20} color="#2563eb" />
            </View>
            <Text style={styles.statValue}>{summaryData?.totalWorkouts || 0}</Text>
            <Text style={styles.statLabel}>Treningi</Text>
          </Pressable>
        </View>

        <Text style={styles.sectionHeader}>Aktywność w tym tygodniu</Text>
        <View style={styles.placeholderBox}>
          <Text style={styles.placeholderText}>Bar chart placeholder</Text>
        </View>

        <Text style={styles.sectionHeader}>Ostatnie treningi</Text>
        {summaryData?.recentWorkouts?.map((w, i) => (
          <Pressable 
            key={i} 
            style={styles.workoutItem}
            onPress={() => navigation.navigate('Training', { screen: 'WorkoutDetail', params: { id: (i+1).toString() } })}
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
        ))}
      </ScrollView>
    </SafeAreaView>
  );
}
