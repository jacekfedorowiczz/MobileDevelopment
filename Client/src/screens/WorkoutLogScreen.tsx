// src/screens/WorkoutLogScreen.tsx
import React from 'react';
import { View, Text, StyleSheet, ScrollView, Pressable } from 'react-native';
import Icon from 'react-native-vector-icons/Feather';
import { Colors, Spacing } from '../theme/theme';

const workoutHistory = [
  {
    id: "1", date: "12 kwietnia 2026",
    workouts: [{ name: "Push (Klatka)", exercises: 8, duration: "45 min", time: "10:30 - 11:15", intensity: "Wysoka" }],
  },
  {
    id: "2", date: "10 kwietnia 2026",
    workouts: [{ name: "Pull (Plecy)", exercises: 7, duration: "52 min", time: "14:15 - 15:07", intensity: "Średnia" }],
  },
  {
    id: "3", date: "8 kwietnia 2026",
    workouts: [{ name: "FBW", exercises: 9, duration: "58 min", time: "09:00 - 09:58", intensity: "Wysoka" }],
  },
];

const getIntensityColor = (intensity: string) => {
  switch (intensity) {
    case "Wysoka": return '#dc2626';
    case "Średnia": return '#f59e0b';
    case "Niska": return '#16a34a';
    default: return Colors.foreground;
  }
};

export default function WorkoutLogScreen({ navigation }: any) {
  return (
    <View style={styles.container}>
      <View style={styles.header}>
        <Pressable onPress={() => navigation.goBack()} style={styles.backButton}>
          <Icon name="arrow-left" size={20} color={Colors.foreground} />
        </Pressable>
        <View style={styles.headerTop}>
          <Text style={styles.title}>Dziennik</Text>
          <View style={styles.headerActions}>
            <Pressable style={styles.iconButton}>
              <Icon name="trash-2" size={20} color={Colors.foreground} />
            </Pressable>
            <Pressable style={[styles.iconButton, styles.iconButtonPrimary]}>
              <Icon name="plus" size={20} color="#fff" />
            </Pressable>
          </View>
        </View>
      </View>

      <ScrollView style={styles.content} contentContainerStyle={styles.scrollContent}>
        {workoutHistory.map((day, dayIndex) => (
          <View key={dayIndex} style={styles.dayGroup}>
            <Text style={styles.dayDate}>{day.date}</Text>
            {day.workouts.map((workout, workoutIndex) => (
              <Pressable
                key={workoutIndex}
                style={styles.workoutCard}
                onPress={() => navigation.navigate('WorkoutDetail', { id: day.id })}
              >
                <View style={styles.workoutHeader}>
                  <View>
                    <Text style={styles.workoutName}>{workout.name}</Text>
                    <Text style={styles.workoutTime}>{workout.time}</Text>
                  </View>
                  <View style={styles.chevronWrapper}>
                    <Icon name="chevron-right" size={20} color="#3b82f6" />
                  </View>
                </View>

                <View style={styles.statsRow}>
                  <View style={styles.statBox}>
                    <Text style={styles.statLabel}>Ćwiczenia</Text>
                    <Text style={styles.statValue}>{workout.exercises}</Text>
                  </View>
                  <View style={styles.statBox}>
                    <Text style={styles.statLabel}>Czas</Text>
                    <Text style={styles.statValue}>{workout.duration}</Text>
                  </View>
                  <View style={styles.statBox}>
                    <Text style={styles.statLabel}>Intensywność</Text>
                    <Text style={[styles.statValue, { color: getIntensityColor(workout.intensity) }]}>{workout.intensity}</Text>
                  </View>
                </View>
              </Pressable>
            ))}
          </View>
        ))}
      </ScrollView>
    </View>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1, backgroundColor: Colors.background },
  header: { paddingHorizontal: Spacing.lg, paddingTop: Spacing.lg, paddingBottom: Spacing.md },
  backButton: { marginBottom: Spacing.md },
  headerTop: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center' },
  title: { fontSize: 28, fontWeight: 'bold', color: Colors.foreground },
  headerActions: { flexDirection: 'row', gap: Spacing.xs },
  iconButton: { width: 44, height: 44, borderRadius: 12, backgroundColor: Colors.secondary, alignItems: 'center', justifyContent: 'center', borderWidth: 1, borderColor: Colors.border, marginLeft: Spacing.xs },
  iconButtonPrimary: { backgroundColor: '#3b82f6', borderColor: '#3b82f6' },
  content: { flex: 1, paddingHorizontal: Spacing.lg },
  scrollContent: { paddingBottom: Spacing.xl },
  dayGroup: { marginBottom: Spacing.lg },
  dayDate: { fontSize: 14, fontWeight: '500', color: Colors.mutedForeground, marginBottom: Spacing.sm, paddingHorizontal: 4 },
  workoutCard: { backgroundColor: Colors.secondary, borderRadius: 16, padding: Spacing.md, marginBottom: Spacing.sm, borderWidth: 1, borderColor: Colors.border },
  workoutHeader: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'flex-start', marginBottom: Spacing.md },
  workoutName: { fontSize: 18, fontWeight: '600', color: Colors.foreground, marginBottom: 4 },
  workoutTime: { fontSize: 14, color: Colors.mutedForeground },
  chevronWrapper: { width: 32, height: 32, borderRadius: 8, backgroundColor: 'rgba(59, 130, 246, 0.1)', alignItems: 'center', justifyContent: 'center' },
  statsRow: { flexDirection: 'row', gap: Spacing.sm },
  statBox: { flex: 1, backgroundColor: Colors.background, borderRadius: 12, padding: Spacing.sm, borderWidth: 1, borderColor: Colors.border },
  statLabel: { fontSize: 12, color: Colors.mutedForeground, marginBottom: 6 },
  statValue: { fontSize: 16, fontWeight: '600', color: Colors.foreground },
});
