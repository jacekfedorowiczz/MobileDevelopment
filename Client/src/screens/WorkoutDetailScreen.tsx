// src/screens/WorkoutDetailScreen.tsx
import React from 'react';
import { View, Text, StyleSheet, ScrollView, Pressable } from 'react-native';
import Icon from 'react-native-vector-icons/Feather';
import { Colors, Spacing } from '../theme/theme';

const workoutDetails: Record<string, any> = {
  "1": {
    name: "Push (Klatka)", date: "12 kwietnia 2026", time: "10:30 - 11:15", duration: "45 min", intensity: "Wysoka", calories: 420, location: "Siłownia FitGym",
    exercises: [
      { name: "Wyciskanie sztangi na ławce płaskiej", sets: 4, reps: "8-10", weight: "80 kg" },
      { name: "Wyciskanie hantli na ławce skośnej", sets: 4, reps: "10-12", weight: "30 kg" },
      { name: "Rozpiętki na wyciągu górnym", sets: 3, reps: "12-15", weight: "15 kg" },
      { name: "Pompki diamentowe", sets: 3, reps: "max", weight: "Brak" },
    ],
  },
  "2": {
    name: "Pull (Plecy)", date: "10 kwietnia 2026", time: "14:15 - 15:07", duration: "52 min", intensity: "Średnia", calories: 380, location: "Calisthenics Park",
    exercises: [
      { name: "Podciąganie na drążku", sets: 4, reps: "6-8", weight: "Brak" },
      { name: "Wiosłowanie sztangą", sets: 4, reps: "8-10", weight: "70 kg" },
      { name: "Przyciąganie wyciągu górnego", sets: 3, reps: "10-12", weight: "55 kg" },
      { name: "Uginanie ramion z hantlami", sets: 3, reps: "10-12", weight: "15 kg" },
    ],
  },
  "3": {
    name: "FBW", date: "8 kwietnia 2026", time: "09:00 - 09:58", duration: "58 min", intensity: "Wysoka", calories: 480, location: "Dom",
    exercises: [
      { name: "Przysiady ze sztangą", sets: 4, reps: "8-10", weight: "100 kg" },
      { name: "Wyciskanie sztangi na ławce", sets: 4, reps: "8-10", weight: "80 kg" },
      { name: "Wiosłowanie hantlami", sets: 3, reps: "10-12", weight: "25 kg" },
      { name: "Martwy ciąg", sets: 3, reps: "6-8", weight: "120 kg" },
      { name: "Wyciskanie sztangi stojąc", sets: 3, reps: "8-10", weight: "45 kg" },
    ],
  },
};

const getIntensityColor = (intensity: string) => {
  switch (intensity) {
    case "Wysoka": return '#dc2626';
    case "Średnia": return '#f59e0b';
    case "Niska": return '#16a34a';
    default: return Colors.foreground;
  }
};

export default function WorkoutDetailScreen({ route, navigation }: any) {
  const { id } = route.params || { id: "1" };
  const workout = workoutDetails[id];

  if (!workout) {
    return (
      <View style={[styles.container, { justifyContent: 'center', alignItems: 'center' }]}>
        <Text style={{ color: Colors.mutedForeground, marginBottom: Spacing.md }}>Nie znaleziono treningu</Text>
        <Pressable onPress={() => navigation.goBack()}>
          <Text style={{ color: '#2563eb', fontWeight: '500' }}>← Powrót</Text>
        </Pressable>
      </View>
    );
  }

  return (
    <View style={styles.container}>
      <View style={styles.header}>
        <Pressable onPress={() => navigation.goBack()} style={styles.backButtonTextWrap}>
          <Icon name="chevron-left" size={20} color="#2563eb" />
          <Text style={styles.backButtonText}>Powrót do dziennika</Text>
        </Pressable>

        <View style={styles.titleRow}>
          <View>
            <Text style={styles.title}>{workout.name}</Text>
            <View style={styles.subtitleRow}>
              <Text style={styles.subtitle}>{workout.date}</Text>
              <Text style={styles.dot}>•</Text>
              <Text style={styles.subtitle}>{workout.time}</Text>
            </View>
            <Text style={styles.locationText}><Text style={styles.locationLabel}>Miejsce:</Text> {workout.location}</Text>
          </View>
          <Pressable style={styles.editButton}>
            <Icon name="edit-2" size={20} color={Colors.mutedForeground} />
          </Pressable>
        </View>
      </View>

      <ScrollView style={styles.content} contentContainerStyle={styles.scrollContent}>
        <View style={styles.statsGrid}>
          <View style={styles.statBox}>
            <View style={styles.statHeader}>
              <Icon name="clock" size={16} color="#2563eb" />
              <Text style={styles.statLabel}>Czas</Text>
            </View>
            <Text style={styles.statValue}>{workout.duration}</Text>
          </View>
          <View style={styles.statBox}>
            <View style={styles.statHeader}>
              <Icon name="zap" size={16} color="#f97316" />
              <Text style={styles.statLabel}>Kalorie</Text>
            </View>
            <Text style={styles.statValue}>{workout.calories}</Text>
          </View>
          <View style={styles.statBox}>
            <View style={styles.statHeader}>
              <Icon name="trending-up" size={16} color="#2563eb" />
              <Text style={styles.statLabel}>Intensywność</Text>
            </View>
            <Text style={[styles.statValue, { color: getIntensityColor(workout.intensity) }]}>{workout.intensity}</Text>
          </View>
          <View style={styles.statBox}>
            <View style={styles.statHeader}>
              <Text style={styles.statLabel}>Ćwiczenia</Text>
            </View>
            <Text style={styles.statValue}>{workout.exercises.length}</Text>
          </View>
        </View>

        <View style={styles.sectionHeaderRow}>
          <Text style={styles.sectionTitle}>Ćwiczenia</Text>
          <View style={styles.sectionActions}>
            <Pressable style={[styles.actionButton, styles.actionButtonDelete]}>
              <Icon name="trash-2" size={20} color="#ef4444" />
            </Pressable>
            <Pressable style={[styles.actionButton, styles.actionButtonAdd]}>
              <Icon name="plus" size={20} color="#fff" />
            </Pressable>
          </View>
        </View>

        <View style={styles.exercisesList}>
          {workout.exercises.map((exercise: any, index: number) => (
            <View key={index} style={styles.exerciseCard}>
              <View style={styles.exerciseHeaderRow}>
                <View style={styles.exerciseNumberBadge}>
                  <Text style={styles.exerciseNumber}>{index + 1}</Text>
                </View>
                <Text style={styles.exerciseName} numberOfLines={1}>{exercise.name}</Text>
              </View>
              <View style={styles.exerciseDetailsRow}>
                <View style={styles.detailItem}>
                  <Text style={styles.detailLabel}>Serie: </Text>
                  <Text style={styles.detailValue}>{exercise.sets}</Text>
                </View>
                <View style={styles.detailItem}>
                  <Text style={styles.detailLabel}>Powtórzenia: </Text>
                  <Text style={styles.detailValue}>{exercise.reps}</Text>
                </View>
                <View style={styles.detailItem}>
                  <Text style={styles.detailLabel}>Obciążenie: </Text>
                  <Text style={styles.detailValue}>{exercise.weight}</Text>
                </View>
              </View>
            </View>
          ))}
        </View>

        <View style={styles.analysisBox}>
          <Text style={styles.analysisTitle}>Analiza</Text>
          <Text style={styles.analysisText}>
            Świetny trening! Udało się wykonać wszystkie serie z planowanym obciążeniem. 
            Przy następnym treningu spróbuję zwiększyć ciężar o 2.5 kg.
          </Text>
        </View>
      </ScrollView>
    </View>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1, backgroundColor: Colors.background },
  header: { paddingHorizontal: Spacing.lg, paddingTop: Spacing.lg, paddingBottom: Spacing.md },
  backButtonTextWrap: { flexDirection: 'row', alignItems: 'center', marginBottom: Spacing.lg },
  backButtonText: { color: '#2563eb', fontWeight: '500', marginLeft: 4, fontSize: 14 },
  titleRow: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'flex-start' },
  title: { fontSize: 28, fontWeight: 'bold', color: Colors.foreground, marginBottom: 8 },
  subtitleRow: { flexDirection: 'row', alignItems: 'center', marginBottom: 4 },
  subtitle: { fontSize: 14, color: Colors.mutedForeground },
  dot: { marginHorizontal: 8, color: Colors.mutedForeground },
  locationText: { fontSize: 14, color: Colors.foreground },
  locationLabel: { fontWeight: '500' },
  editButton: { width: 40, height: 40, borderRadius: 12, backgroundColor: Colors.secondary, alignItems: 'center', justifyContent: 'center', borderWidth: 1, borderColor: Colors.border },
  content: { flex: 1, paddingHorizontal: Spacing.lg },
  scrollContent: { paddingBottom: Spacing.xl },
  statsGrid: { flexDirection: 'row', flexWrap: 'wrap', justifyContent: 'space-between', marginBottom: Spacing.lg },
  statBox: { width: '48%', backgroundColor: Colors.secondary, padding: Spacing.md, borderRadius: 16, marginBottom: Spacing.sm, borderWidth: 1, borderColor: Colors.border },
  statHeader: { flexDirection: 'row', alignItems: 'center', marginBottom: 8 },
  statLabel: { fontSize: 14, color: Colors.mutedForeground, marginLeft: 6 },
  statValue: { fontSize: 24, fontWeight: 'bold', color: Colors.foreground },
  sectionHeaderRow: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center', marginBottom: Spacing.md },
  sectionTitle: { fontSize: 18, fontWeight: '600', color: Colors.foreground },
  sectionActions: { flexDirection: 'row', alignItems: 'center' },
  actionButton: { width: 40, height: 40, borderRadius: 12, alignItems: 'center', justifyContent: 'center', marginLeft: Spacing.sm },
  actionButtonDelete: { backgroundColor: Colors.secondary, borderWidth: 1, borderColor: Colors.border },
  actionButtonAdd: { backgroundColor: '#3b82f6' },
  exercisesList: { gap: Spacing.sm, marginBottom: Spacing.lg },
  exerciseCard: { backgroundColor: Colors.secondary, padding: Spacing.md, borderRadius: 16, borderWidth: 1, borderColor: Colors.border },
  exerciseHeaderRow: { flexDirection: 'row', alignItems: 'center', marginBottom: 12 },
  exerciseNumberBadge: { width: 24, height: 24, borderRadius: 12, backgroundColor: 'rgba(37, 99, 235, 0.1)', alignItems: 'center', justifyContent: 'center', marginRight: 8 },
  exerciseNumber: { fontSize: 12, fontWeight: 'bold', color: '#2563eb' },
  exerciseName: { fontSize: 14, fontWeight: '600', color: Colors.foreground, flex: 1 },
  exerciseDetailsRow: { flexDirection: 'row', justifyContent: 'space-between' },
  detailItem: { flexDirection: 'row' },
  detailLabel: { fontSize: 12, color: Colors.mutedForeground },
  detailValue: { fontSize: 12, fontWeight: '500', color: Colors.foreground },
  analysisBox: { backgroundColor: Colors.secondary, padding: Spacing.md, borderRadius: 16, borderWidth: 1, borderColor: Colors.border },
  analysisTitle: { fontSize: 16, fontWeight: '600', color: Colors.foreground, marginBottom: 8 },
  analysisText: { fontSize: 14, color: Colors.mutedForeground, lineHeight: 20 },
});
