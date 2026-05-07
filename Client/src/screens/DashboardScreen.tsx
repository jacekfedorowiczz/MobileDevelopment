// src/screens/DashboardScreen.tsx
import React from 'react';
import { View, Text, StyleSheet, Pressable, ScrollView } from 'react-native';
import Icon from 'react-native-vector-icons/Feather';
import { Colors, Font, Spacing } from '../theme/theme';

const weekData = [
  { day: 'Pon', minutes: 90 },
  { day: 'Wt', minutes: 0 },
  { day: 'Śr', minutes: 120 },
  { day: 'Czw', minutes: 60 },
  { day: 'Pt', minutes: 90 },
  { day: 'Sob', minutes: 0 },
  { day: 'Ndz', minutes: 150 },
];

const recentWorkouts = [
  { name: 'Push (Klatka)', exercises: 8, duration: '45 min', date: '12 kwi' },
  { name: 'Pull (Plecy)', exercises: 7, duration: '52 min', date: '10 kwi' },
  { name: 'FBW', exercises: 9, duration: '58 min', date: '8 kwi' },
];

const quickActions = [
  { title: 'Dieta', description: 'Zaplanuj posiłki', icon: 'apple', path: 'Diet' },
  { title: 'Plan treningowy', description: 'Dzisiejszy trening', icon: 'activity', path: 'Training' },
];

export default function DashboardScreen({ navigation }: any) {
  return (
    <ScrollView style={styles.container} contentContainerStyle={{ padding: Spacing.md }}>
      <Text style={styles.welcome}>Witaj z powrotem, Jan!</Text>
      <Text style={styles.date}>Niedziela, 12 kwietnia 2026</Text>

      <View style={styles.actionsRow}>
        {quickActions.map((action, idx) => (
          <Pressable
            key={idx}
            style={styles.actionButton}
            onPress={() => navigation.navigate(action.path)}
          >
            <View style={styles.actionIconWrapper}>
              <Icon name={action.icon} size={24} color={Colors.primary} />
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
        <View style={styles.statBox}>
          <Icon name="fire" size={20} color={Colors.primary} />
          <Text style={styles.statValue}>2,340</Text>
          <Text style={styles.statLabel}>Kalorie</Text>
        </View>
        <View style={styles.statBox}>
          <Icon name="target" size={20} color={Colors.primary} />
          <Text style={styles.statValue}>12</Text>
          <Text style={styles.statLabel}>Seria</Text>
        </View>
        <View style={styles.statBox}>
          <Icon name="dumbbell" size={20} color={Colors.primary} />
          <Text style={styles.statValue}>24</Text>
          <Text style={styles.statLabel}>Treningi</Text>
        </View>
      </View>

      <Text style={styles.sectionHeader}>Aktywność w tym tygodniu</Text>
      <View style={styles.placeholderBox}>
        <Text style={styles.placeholderText}>Bar chart placeholder</Text>
      </View>

      <Text style={styles.sectionHeader}>Ostatnie treningi</Text>
      {recentWorkouts.map((w, i) => (
        <View key={i} style={styles.workoutItem}>
          <View>
            <Text style={styles.workoutName}>{w.name}</Text>
            <Text style={styles.workoutDetails}>
              {w.exercises} ćwiczeń · {w.duration}
            </Text>
          </View>
          <View style={styles.workoutDate}>
            <Text style={styles.workoutDateText}>{w.date}</Text>
          </View>
        </View>
      ))}
    </ScrollView>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1, backgroundColor: Colors.background },
  welcome: { fontSize: 24, fontWeight: '600', color: Colors.foreground },
  date: { fontSize: 14, color: Colors.mutedForeground, marginBottom: Spacing.md },
  actionsRow: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    marginBottom: Spacing.lg,
  },
  actionButton: {
    flex: 1,
    backgroundColor: Colors.secondary,
    borderRadius: 12,
    padding: Spacing.sm,
    marginHorizontal: Spacing.xs,
    alignItems: 'center',
  },
  actionIconWrapper: {
    marginBottom: Spacing.xs,
  },
  actionTitle: { fontSize: 16, fontWeight: '500', color: Colors.foreground },
  actionDesc: { fontSize: 12, color: Colors.mutedForeground },
  placeholderBox: {
    height: 120,
    backgroundColor: Colors.secondary,
    borderRadius: 12,
    justifyContent: 'center',
    alignItems: 'center',
    marginBottom: Spacing.lg,
  },
  placeholderText: { color: Colors.mutedForeground },
  statsRow: { flexDirection: 'row', justifyContent: 'space-between', marginBottom: Spacing.lg },
  statBox: { alignItems: 'center', flex: 1 },
  statValue: { fontSize: 20, fontWeight: '600', color: Colors.foreground },
  statLabel: { fontSize: 12, color: Colors.mutedForeground },
  sectionHeader: { fontSize: 18, fontWeight: '600', marginBottom: Spacing.sm, color: Colors.foreground },
  workoutItem: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    paddingVertical: Spacing.sm,
    borderBottomWidth: 1,
    borderColor: Colors.border,
  },
  workoutName: { fontSize: 16, fontWeight: '500', color: Colors.foreground },
  workoutDetails: { fontSize: 12, color: Colors.mutedForeground },
  workoutDate: {
    backgroundColor: Colors.background,
    borderRadius: 6,
    borderWidth: 1,
    borderColor: Colors.border,
    paddingHorizontal: Spacing.xs,
    paddingVertical: 2,
  },
  workoutDateText: { fontSize: 12, color: Colors.mutedForeground },
});
