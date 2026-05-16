// src/screens/ExerciseDetailScreen.tsx
import React, { useCallback, useEffect, useState } from 'react';
import { View, Text, StyleSheet, ScrollView, Image, Pressable, ActivityIndicator, Alert } from 'react-native';
import { useSafeAreaInsets } from 'react-native-safe-area-context';
import Icon from 'react-native-vector-icons/Feather';
import { Spacing } from '../theme/theme';
import { useTheme } from '../context/ThemeContext';
import { ExerciseApiService, ExerciseDifficulty, ExerciseDto } from '../api/ExerciseApiService';
import BackButton from '../components/BackButton';

const getDifficultyLabel = (difficulty?: ExerciseDifficulty) => {
  switch (difficulty) {
    case ExerciseDifficulty.Beginner: return 'Początkujący';
    case ExerciseDifficulty.Intermediate: return 'Średniozaawansowany';
    case ExerciseDifficulty.Advanced: return 'Zaawansowany';
    case ExerciseDifficulty.Elite: return 'Elita';
    default: return undefined;
  }
};

export default function ExerciseDetailScreen({ route, navigation }: any) {
  const { colors } = useTheme();
  const insets = useSafeAreaInsets();
  const exerciseId = route.params?.id;
  const [exercise, setExercise] = useState<ExerciseDto | null>(null);
  const [isLoading, setIsLoading] = useState(true);

  const loadExercise = useCallback(async () => {
    try {
      setIsLoading(true);
      const data = await ExerciseApiService.getExerciseById(exerciseId);
      setExercise(data);
    } catch (e: any) {
      Alert.alert('Błąd', e.message ?? 'Nie udało się pobrać danych ćwiczenia');
      navigation.goBack();
    } finally {
      setIsLoading(false);
    }
  }, [exerciseId, navigation]);

  useEffect(() => {
    loadExercise();
  }, [loadExercise]);

  if (isLoading) {
    return (
      <View style={[styles.container, styles.centered, { backgroundColor: colors.background }]}>
        <ActivityIndicator size="large" color="#3b82f6" />
      </View>
    );
  }

  if (!exercise) return null;

  const muscles = exercise.targetedMuscles?.map(m => m.name).join(', ') ?? 'Brak danych';
  const difficultyLabel = getDifficultyLabel(exercise.difficulty);

  return (
    <View style={[styles.container, { backgroundColor: colors.background }]}>
      <ScrollView contentContainerStyle={styles.scrollContent} bounces={false}>
        <View style={[styles.heroContainer, { backgroundColor: colors.card }]}>
          <Image
            source={{ uri: exercise.imageUrl || "https://images.unsplash.com/photo-1585484764802-387ea30e8432?crop=entropy&cs=tinysrgb&fit=max&fm=jpg&q=80&w=800" }}
            style={styles.heroImage}
          />
          <View style={styles.overlay} />
          
          <BackButton
            onPress={() => navigation.canGoBack() ? navigation.goBack() : navigation.navigate('Exercises')}
            style={[styles.backButton, { top: insets.top + 12 }]}
          />
          
          {!exercise.isSystem && (
            <Pressable style={[styles.editButton, { top: insets.top + 12 }]} onPress={() => Alert.alert('Edycja', 'Funkcja edycji ćwiczenia będzie dostępna wkrótce.')}>
              <Icon name="edit-2" size={20} color="#fff" />
            </Pressable>
          )}

          <View style={styles.playButtonContainer}>
            <Pressable style={styles.playButton}>
              <Icon name="play-circle" size={32} color="#fff" />
            </Pressable>
          </View>
        </View>

        <View style={[styles.content, { backgroundColor: colors.background, borderTopLeftRadius: 24, borderTopRightRadius: 24, marginTop: -24 }]}>
          <View style={styles.titleSection}>
            <Text style={[styles.title, { color: colors.foreground }]}>{exercise.name}</Text>
            <View style={styles.subtitleRow}>
              <Icon name="clipboard" size={16} color="#3b82f6" />
              <Text style={[styles.subtitle, { color: colors.mutedForeground }]}>Partie: {muscles}</Text>
            </View>
          </View>

          <View style={styles.tagsContainer}>
            {difficultyLabel && (
              <View style={[styles.tag, { backgroundColor: colors.card, borderColor: colors.border }]}>
                <Icon name="bar-chart-2" size={12} color="#3b82f6" style={styles.tagIcon} />
                <Text style={[styles.tagText, { color: colors.foreground }]}>{difficultyLabel}</Text>
              </View>
            )}
            <View style={[styles.tag, { backgroundColor: colors.card, borderColor: colors.border }]}>
              <Icon name="zap" size={12} color="#3b82f6" style={styles.tagIcon} />
              <Text style={[styles.tagText, { color: colors.foreground }]}>{exercise.isCompound ? 'Wielostawowe' : 'Izolowane'}</Text>
            </View>
            <View style={[styles.tag, { backgroundColor: colors.card, borderColor: colors.border }]}>
              <Icon name="shield" size={12} color="#3b82f6" style={styles.tagIcon} />
              <Text style={[styles.tagText, { color: colors.foreground }]}>{exercise.isSystem ? 'Systemowe' : 'Własne'}</Text>
            </View>
          </View>

          <View style={[styles.descriptionCard, { backgroundColor: colors.card, borderColor: colors.border }]}>
            <View style={styles.descriptionHeader}>
              <View style={styles.infoIconWrapper}>
                <Icon name="info" size={16} color="#3b82f6" />
              </View>
              <Text style={[styles.descriptionTitle, { color: colors.foreground }]}>Opis i technika</Text>
            </View>
            <Text style={[styles.descriptionText, { color: colors.foreground }]}>
              {exercise.description || 'Brak opisu dla tego ćwiczenia. Pamiętaj o zachowaniu poprawnej techniki i oddychaniu podczas wykonywania ruchu.'}
            </Text>
          </View>

          {/* Additional Info / History Placeholder */}
          <View style={[styles.statsSection, { marginTop: Spacing.xl }]}>
            <Text style={[styles.sectionTitle, { color: colors.foreground }]}>Twoje statystyki</Text>
            <View style={[styles.statsGrid]}>
              <View style={[styles.statItem, { backgroundColor: colors.card, borderColor: colors.border }]}>
                <Text style={[styles.statLabel, { color: colors.mutedForeground }]}>Max Ciężar</Text>
                <Text style={[styles.statValue, { color: colors.foreground }]}>—</Text>
              </View>
              <View style={[styles.statItem, { backgroundColor: colors.card, borderColor: colors.border }]}>
                <Text style={[styles.statLabel, { color: colors.mutedForeground }]}>Max Reps</Text>
                <Text style={[styles.statValue, { color: colors.foreground }]}>—</Text>
              </View>
            </View>
          </View>
        </View>
      </ScrollView>
    </View>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1 },
  centered: { justifyContent: 'center', alignItems: 'center' },
  scrollContent: { paddingBottom: Spacing.xl },
  heroContainer: { width: '100%', height: 300, position: 'relative' },
  heroImage: { width: '100%', height: '100%', resizeMode: 'cover' },
  overlay: { ...StyleSheet.absoluteFillObject, backgroundColor: 'rgba(0,0,0,0.3)' },
  backButton: { position: 'absolute', left: 20 },
  editButton: { position: 'absolute', right: 20, width: 44, height: 44, borderRadius: 12, backgroundColor: 'rgba(0,0,0,0.5)', alignItems: 'center', justifyContent: 'center' },
  playButtonContainer: { ...StyleSheet.absoluteFillObject, alignItems: 'center', justifyContent: 'center' },
  playButton: { width: 64, height: 64, borderRadius: 32, backgroundColor: '#3b82f6', alignItems: 'center', justifyContent: 'center', shadowColor: '#3b82f6', shadowOpacity: 0.4, shadowRadius: 10, elevation: 5 },
  content: { paddingHorizontal: Spacing.lg, paddingTop: Spacing.xl },
  titleSection: { marginBottom: Spacing.lg },
  title: { fontSize: 28, fontWeight: 'bold', marginBottom: 8 },
  subtitleRow: { flexDirection: 'row', alignItems: 'center' },
  subtitle: { fontSize: 14, fontWeight: '500', marginLeft: 6 },
  tagsContainer: { flexDirection: 'row', flexWrap: 'wrap', gap: 8, marginBottom: Spacing.xl },
  tag: { flexDirection: 'row', alignItems: 'center', paddingHorizontal: 12, paddingVertical: 8, borderRadius: 10, borderWidth: 1 },
  tagIcon: { marginRight: 6 },
  tagText: { fontSize: 12, fontWeight: '500' },
  descriptionCard: { borderRadius: 16, padding: Spacing.lg, borderWidth: 1 },
  descriptionHeader: { flexDirection: 'row', alignItems: 'center', marginBottom: Spacing.md },
  infoIconWrapper: { width: 32, height: 32, borderRadius: 16, backgroundColor: 'rgba(59, 130, 246, 0.1)', alignItems: 'center', justifyContent: 'center', marginRight: 8 },
  descriptionTitle: { fontSize: 18, fontWeight: '600' },
  descriptionText: { fontSize: 14, opacity: 0.8, lineHeight: 22 },
  sectionTitle: { fontSize: 18, fontWeight: 'bold', marginBottom: Spacing.md },
  statsGrid: { flexDirection: 'row', gap: Spacing.md },
  statItem: { flex: 1, padding: Spacing.md, borderRadius: 16, borderWidth: 1, alignItems: 'center' },
  statLabel: { fontSize: 12, marginBottom: 4 },
  statValue: { fontSize: 20, fontWeight: 'bold' },
});
