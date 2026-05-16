// src/screens/WorkoutDetailScreen.tsx
import React, { useCallback, useEffect, useState } from 'react';
import {
  View, Text, ScrollView, Pressable,
  ActivityIndicator, Alert, TextInput, Modal, KeyboardAvoidingView, Platform
} from 'react-native';
import { useSafeAreaInsets } from 'react-native-safe-area-context';
import Icon from 'react-native-vector-icons/Feather';
import { Colors, Spacing } from '../theme/theme';
import { useTheme } from '../context/ThemeContext';
import { WorkoutSessionApiService, WorkoutSessionDto, WorkoutSetDto } from '../api/WorkoutSessionApiService';
import { WorkoutSetApiService } from '../api/WorkoutSetApiService';
import BackButton, { backButtonSpacing } from '../components/BackButton';
import { styles } from './WorkoutDetailScreen.styles';

const formatDuration = (startTime: string, endTime?: string) => {
  if (!endTime) return 'W trakcie';
  const diff = new Date(endTime).getTime() - new Date(startTime).getTime();
  const min = Math.round(diff / 60000);
  return `${min} min`;
};

const formatDate = (iso: string) =>
  new Date(iso).toLocaleDateString('pl-PL', { day: 'numeric', month: 'long', year: 'numeric' });

const groupSetsByExercise = (sets: WorkoutSetDto[]) => {
  const groups: Record<number, { name: string; sets: WorkoutSetDto[] }> = {};
  for (const set of sets) {
    if (!groups[set.exerciseId]) {
      groups[set.exerciseId] = { name: set.exercise?.name ?? `Ćwiczenie #${set.exerciseId}`, sets: [] };
    }
    groups[set.exerciseId].sets.push(set);
  }
  return Object.entries(groups);
};

interface AddSetForm {
  exerciseId: number;
  exerciseName: string;
  weight: string;
  reps: string;
  rpe: string;
  duration: string;
}

export default function WorkoutDetailScreen({ route, navigation }: any) {
  const { colors } = useTheme();
  const insets = useSafeAreaInsets();
  const sessionId: number = route.params?.id;
  const [session, setSession] = useState<WorkoutSessionDto | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  // Add Set Modal state
  const [showAddModal, setShowAddModal] = useState(false);
  const [selectedExerciseId, setSelectedExerciseId] = useState<number | null>(null);
  const [selectedExerciseName, setSelectedExerciseName] = useState('');
  const [form, setForm] = useState<Omit<AddSetForm, 'exerciseId' | 'exerciseName'>>({ weight: '', reps: '', rpe: '', duration: '' });
  const [isSaving, setIsSaving] = useState(false);

  // Edit Session Modal state
  const [showEditModal, setShowEditModal] = useState(false);
  const [editName, setEditName] = useState('');
  const [editDescription, setEditDescription] = useState('');
  const [editDurationMin, setEditDurationMin] = useState('');
  const [isSavingEdit, setIsSavingEdit] = useState(false);

  const loadSession = useCallback(async () => {
    try {
      const data = await WorkoutSessionApiService.getSessionById(sessionId);
      setSession(data);
      setError(null);
    } catch (e: any) {
      setError(e.message ?? 'Błąd pobierania treningu');
    } finally {
      setIsLoading(false);
    }
  }, [sessionId]);

  useEffect(() => { loadSession(); }, [loadSession]);

  // Reload when returning from ExercisePicker
  useEffect(() => {
    const unsubscribe = navigation.addListener('focus', () => {
      if (!isLoading) loadSession();
    });
    return unsubscribe;
  }, [navigation, isLoading, loadSession]);

  const handleDeleteSet = (setId: number) => {
    Alert.alert('Usuń serię', 'Czy na pewno chcesz usunąć tę serię?', [
      { text: 'Anuluj', style: 'cancel' },
      {
        text: 'Usuń', style: 'destructive',
        onPress: async () => {
          try {
            await WorkoutSetApiService.deleteSet(setId);
            await loadSession();
          } catch (e: any) {
            Alert.alert('Błąd', e.message);
          }
        },
      },
    ]);
  };

  const openAddSetModal = (exerciseId: number, exerciseName: string) => {
    setSelectedExerciseId(exerciseId);
    setSelectedExerciseName(exerciseName);
    setForm({ weight: '', reps: '', rpe: '', duration: '' });
    setShowAddModal(true);
  };

  const handleAddSetFromPicker = (exerciseId: number, name: string) => {
    openAddSetModal(exerciseId, name);
  };

  const handleSaveSet = async () => {
    if (!selectedExerciseId || !form.reps || !form.weight) {
      Alert.alert('Błąd', 'Wypełnij wagę i liczbę powtórzeń.');
      return;
    }
    const existingSets = session?.sets?.filter(s => s.exerciseId === selectedExerciseId) ?? [];
    const nextSetNumber = existingSets.length + 1;
    const parsedRpe = form.rpe ? parseInt(form.rpe, 10) : undefined;

    if (parsedRpe !== undefined && (parsedRpe < 1 || parsedRpe > 10)) {
      Alert.alert('Błąd', 'RPE serii musi być w zakresie 1-10.');
      return;
    }

    setIsSaving(true);
    try {
      await WorkoutSetApiService.createSet({
        workoutSessionId: sessionId,
        exerciseId: selectedExerciseId,
        setNumber: nextSetNumber,
        weight: parseFloat(form.weight) || 0,
        reps: parseInt(form.reps, 10) || 0,
        rpe: parsedRpe,
        durationSeconds: form.duration ? parseInt(form.duration, 10) : undefined,
      });
      setShowAddModal(false);
      await loadSession();
    } catch (e: any) {
      Alert.alert('Błąd', e.message ?? 'Nie udało się dodać serii');
    } finally {
      setIsSaving(false);
    }
  };

  const openEditModal = () => {
    if (!session) return;
    setEditName(session.name);
    setEditDescription(session.description ?? '');
    const diffMin = session.endTime
      ? Math.round((new Date(session.endTime).getTime() - new Date(session.startTime).getTime()) / 60000)
      : 0;
    setEditDurationMin(String(diffMin));
    setShowEditModal(true);
  };

  const handleSaveEdit = async () => {
    if (!session) return;
    setIsSavingEdit(true);
    try {
      const durationMin = parseInt(editDurationMin, 10) || 0;
      const endTime = new Date(new Date(session.startTime).getTime() + durationMin * 60000).toISOString();
      await WorkoutSessionApiService.updateSession(session.id, {
        name: editName,
        description: editDescription || undefined,
        startTime: session.startTime,
        endTime,
      });
      setShowEditModal(false);
      await loadSession();
    } catch (e: any) {
      Alert.alert('Błąd', e.message ?? 'Nie udało się zaktualizować treningu');
    } finally {
      setIsSavingEdit(false);
    }
  };

  const goBack = () => navigation.canGoBack() ? navigation.goBack() : navigation.navigate('TrainingHub');

  if (isLoading) {
    return (
      <View style={[styles.container, styles.centered, { backgroundColor: colors.background, paddingTop: insets.top }]}>
        <ActivityIndicator size="large" color="#3b82f6" />
      </View>
    );
  }

  if (error || !session) {
    return (
      <View style={[styles.container, styles.centered, { backgroundColor: colors.background, paddingTop: insets.top }]}>
        <Icon name="alert-circle" size={40} color={Colors.mutedForeground} />
        <Text style={[styles.errorText, { color: colors.mutedForeground }]}>{error ?? 'Nie znaleziono treningu'}</Text>
        <Pressable style={styles.retryBtn} onPress={loadSession}>
          <Text style={styles.retryText}>Spróbuj ponownie</Text>
        </Pressable>
        <BackButton onPress={goBack} style={{ marginTop: 12 }} />
      </View>
    );
  }

  const sets = session.sets ?? [];
  const exerciseGroups = groupSetsByExercise(sets);
  const duration = formatDuration(session.startTime, session.endTime ?? undefined);

  return (
    <View style={[styles.container, { backgroundColor: colors.background }]}>
      {/* Header with safe area */}
      <View style={[styles.header, { backgroundColor: colors.background, paddingTop: insets.top + Spacing.sm }]}>
        <BackButton onPress={goBack} style={backButtonSpacing} />
        <View style={styles.titleRow}>
          <View style={{ flex: 1 }}>
            <Text style={[styles.title, { color: colors.foreground }]} numberOfLines={2}>{session.name}</Text>
            <Text style={[styles.subtitle, { color: colors.mutedForeground }]}>{formatDate(session.startTime)}</Text>
          </View>
          <Pressable style={[styles.editBtn, { backgroundColor: colors.card, borderColor: colors.border }]} onPress={openEditModal}>
            <Icon name="edit-2" size={18} color={colors.foreground} />
          </Pressable>
        </View>
      </View>

      <ScrollView style={styles.content} contentContainerStyle={[styles.scrollContent, { paddingBottom: insets.bottom + Spacing.xl }]}>
        {/* Stats */}
        <View style={styles.statsGrid}>
          {[
            { icon: 'clock', color: '#3b82f6', label: 'Czas', value: duration },
            { icon: 'activity', color: '#8b5cf6', label: 'Ćwiczenia', value: String(exerciseGroups.length) },
            { icon: 'layers', color: '#f59e0b', label: 'Serie', value: String(sets.length) },
            {
              icon: 'trending-up',
              color: session.globalSessionRpe && session.globalSessionRpe >= 8 ? '#dc2626' : '#16a34a',
              label: 'RPE',
              value: session.globalSessionRpe ? `${session.globalSessionRpe}/10` : '—'
            },
          ].map(stat => (
            <View key={stat.label} style={[styles.statBox, { backgroundColor: colors.card, borderColor: colors.border }]}>
              <View style={styles.statHeader}>
                <Icon name={stat.icon as any} size={16} color={stat.color} />
                <Text style={[styles.statLabel, { color: colors.mutedForeground }]}>{stat.label}</Text>
              </View>
              <Text style={[styles.statValue, { color: colors.foreground }]}>{stat.value}</Text>
            </View>
          ))}
        </View>

        {/* Notes */}
        {!!session.description && (
          <View style={[styles.notesBox, { backgroundColor: colors.card, borderColor: colors.border, marginBottom: Spacing.lg, marginTop: 0 }]}>
            <Text style={[styles.notesTitle, { color: colors.foreground }]}>Notatki</Text>
            <Text style={[styles.notesText, { color: colors.mutedForeground }]}>{session.description}</Text>
          </View>
        )}

        {/* Exercises & Sets */}
        <View style={styles.sectionRow}>
          <Text style={[styles.sectionTitle, { color: colors.foreground }]}>Ćwiczenia i serie</Text>
          <Pressable
            style={styles.addBtn}
            onPress={() => navigation.navigate('Exercises', {
              sessionId: session.id,
              mode: 'pick',
              onSelect: (id: number, name: string) => handleAddSetFromPicker(id, name),
            })}
          >
            <Icon name="plus" size={18} color="#fff" />
            <Text style={styles.addBtnText}>Dodaj</Text>
          </Pressable>
        </View>

        {exerciseGroups.length === 0 ? (
          <View style={[styles.emptyExercises, { borderColor: colors.border }]}>
            <Icon name="plus-circle" size={32} color="#3b82f6" />
            <Text style={[styles.emptyExTitle, { color: colors.foreground }]}>Brak ćwiczeń</Text>
            <Text style={[styles.emptyExDesc, { color: colors.mutedForeground }]}>Dodaj pierwsze ćwiczenie do tego treningu</Text>
          </View>
        ) : (
          exerciseGroups.map(([exerciseId, group]) => (
            <View key={exerciseId} style={[styles.exerciseCard, { backgroundColor: colors.card, borderColor: colors.border }]}>
              <View style={styles.exerciseHeader}>
                <Text style={[styles.exerciseName, { color: colors.foreground }]}>{group.name}</Text>
                <Pressable
                  style={styles.addSetBtn}
                  onPress={() => openAddSetModal(Number(exerciseId), group.name)}
                >
                  <Icon name="plus" size={14} color="#3b82f6" />
                  <Text style={styles.addSetBtnText}>Seria</Text>
                </Pressable>
              </View>
              {/* Sets header */}
              <View style={styles.setsHeader}>
                <Text style={[styles.setCol, styles.setHeaderText, { color: colors.mutedForeground }]}>Seria</Text>
                <Text style={[styles.setCol, styles.setHeaderText, { color: colors.mutedForeground }]}>Kg</Text>
                <Text style={[styles.setCol, styles.setHeaderText, { color: colors.mutedForeground }]}>Pow.</Text>
                <Text style={[styles.setCol, styles.setHeaderText, { color: colors.mutedForeground }]}>RPE</Text>
                <Text style={[styles.setCol, styles.setHeaderText, { color: colors.mutedForeground }]}>Czas</Text>
                <View style={{ width: 32 }} />
              </View>
              {group.sets.map(set => (
                <View key={set.id} style={[styles.setRow, { borderTopColor: colors.border }]}>
                  <Text style={[styles.setCol, { color: colors.foreground }]}>{set.setNumber}</Text>
                  <Text style={[styles.setCol, { color: colors.foreground }]}>{set.weight}</Text>
                  <Text style={[styles.setCol, { color: colors.foreground }]}>{set.reps}</Text>
                  <Text style={[styles.setCol, { color: colors.mutedForeground }]}>{set.rpe ?? '—'}</Text>
                  <Text style={[styles.setCol, { color: colors.mutedForeground }]}>{set.durationSeconds ? `${set.durationSeconds}s` : '—'}</Text>
                  <Pressable style={styles.deleteSet} onPress={() => handleDeleteSet(set.id)}>
                    <Icon name="x" size={16} color="#ef4444" />
                  </Pressable>
                </View>
              ))}
            </View>
          ))
        )}
      </ScrollView>

      {/* Add Set Modal */}
      <Modal visible={showAddModal} transparent animationType="slide" onRequestClose={() => setShowAddModal(false)}>
        <KeyboardAvoidingView behavior={Platform.OS === 'ios' ? 'padding' : 'height'} style={styles.modalOverlay}>
          <Pressable style={styles.modalDismiss} onPress={() => setShowAddModal(false)} />
          <View style={[styles.modalSheet, { backgroundColor: colors.card, paddingBottom: insets.bottom + Spacing.lg }]}>
            <View style={styles.modalHandle} />
            <Text style={[styles.modalTitle, { color: colors.foreground }]}>Dodaj serię</Text>
            <Text style={[styles.modalSubtitle, { color: colors.mutedForeground }]}>{selectedExerciseName}</Text>

            <View style={styles.modalFields}>
              <View style={styles.modalField}>
                <Text style={[styles.modalLabel, { color: colors.mutedForeground }]}>Ciężar (kg)</Text>
                <TextInput
                  style={[styles.modalInput, { backgroundColor: colors.background, borderColor: colors.border, color: colors.foreground }]}
                  value={form.weight}
                  onChangeText={v => setForm(f => ({ ...f, weight: v }))}
                  keyboardType="decimal-pad"
                  placeholder="0"
                  placeholderTextColor={colors.mutedForeground}
                />
              </View>
              <View style={styles.modalField}>
                <Text style={[styles.modalLabel, { color: colors.mutedForeground }]}>Powt.</Text>
                <TextInput
                  style={[styles.modalInput, { backgroundColor: colors.background, borderColor: colors.border, color: colors.foreground }]}
                  value={form.reps}
                  onChangeText={v => setForm(f => ({ ...f, reps: v }))}
                  keyboardType="number-pad"
                  placeholder="0"
                  placeholderTextColor={colors.mutedForeground}
                />
              </View>
            </View>
            <View style={styles.modalFields}>
              <View style={styles.modalField}>
                <Text style={[styles.modalLabel, { color: colors.mutedForeground }]}>RPE</Text>
                <TextInput
                  style={[styles.modalInput, { backgroundColor: colors.background, borderColor: colors.border, color: colors.foreground }]}
                  value={form.rpe}
                  onChangeText={v => setForm(f => ({ ...f, rpe: v }))}
                  keyboardType="number-pad"
                  placeholder="1–10"
                  placeholderTextColor={colors.mutedForeground}
                />
              </View>
              <View style={styles.modalField}>
                <Text style={[styles.modalLabel, { color: colors.mutedForeground }]}>Czas (s)</Text>
                <TextInput
                  style={[styles.modalInput, { backgroundColor: colors.background, borderColor: colors.border, color: colors.foreground }]}
                  value={form.duration}
                  onChangeText={v => setForm(f => ({ ...f, duration: v }))}
                  keyboardType="number-pad"
                  placeholder="0"
                  placeholderTextColor={colors.mutedForeground}
                />
              </View>
            </View>

            <Pressable
              style={[styles.modalSaveBtn, { opacity: isSaving ? 0.6 : 1 }]}
              onPress={handleSaveSet}
              disabled={isSaving}
            >
              {isSaving ? <ActivityIndicator color="#fff" size="small" /> : <Text style={styles.modalSaveBtnText}>Dodaj serię</Text>}
            </Pressable>
          </View>
        </KeyboardAvoidingView>
      </Modal>

      {/* Edit Session Modal */}
      <Modal visible={showEditModal} transparent animationType="slide" onRequestClose={() => setShowEditModal(false)}>
        <KeyboardAvoidingView behavior={Platform.OS === 'ios' ? 'padding' : 'height'} style={styles.modalOverlay}>
          <Pressable style={styles.modalDismiss} onPress={() => setShowEditModal(false)} />
          <View style={[styles.modalSheet, { backgroundColor: colors.card, paddingBottom: insets.bottom + Spacing.lg }]}>
            <View style={styles.modalHandle} />
            <Text style={[styles.modalTitle, { color: colors.foreground }]}>Edytuj trening</Text>

            <View style={styles.modalFields}>
              <View style={{ flex: 2 }}>
                <Text style={[styles.modalLabel, { color: colors.mutedForeground }]}>Nazwa treningu</Text>
                <TextInput
                  style={[styles.modalInput, { backgroundColor: colors.background, borderColor: colors.border, color: colors.foreground }]}
                  value={editName}
                  onChangeText={setEditName}
                  placeholder="np. Push Day"
                  placeholderTextColor={colors.mutedForeground}
                />
              </View>
            </View>

            <View style={styles.modalFields}>
              <View style={{ flex: 1 }}>
                <Text style={[styles.modalLabel, { color: colors.mutedForeground }]}>Notatki (opcjonalnie)</Text>
                <TextInput
                  style={[styles.modalInput, { backgroundColor: colors.background, borderColor: colors.border, color: colors.foreground, height: 80, paddingTop: 12 }]}
                  value={editDescription}
                  onChangeText={setEditDescription}
                  placeholder="Brak notatek"
                  placeholderTextColor={colors.mutedForeground}
                  multiline
                  textAlignVertical="top"
                />
              </View>
            </View>

            <View style={styles.modalFields}>
              <View style={{ flex: 1 }}>
                <Text style={[styles.modalLabel, { color: colors.mutedForeground }]}>Czas (minuty)</Text>
                <TextInput
                  style={[styles.modalInput, { backgroundColor: colors.background, borderColor: colors.border, color: colors.foreground }]}
                  value={editDurationMin}
                  onChangeText={setEditDurationMin}
                  keyboardType="number-pad"
                  placeholder="60"
                  placeholderTextColor={colors.mutedForeground}
                />
              </View>
            </View>

            <Pressable
              style={[styles.modalSaveBtn, { opacity: isSavingEdit ? 0.6 : 1 }]}
              onPress={handleSaveEdit}
              disabled={isSavingEdit}
            >
              {isSavingEdit ? <ActivityIndicator color="#fff" size="small" /> : <Text style={styles.modalSaveBtnText}>Zapisz zmiany</Text>}
            </Pressable>
          </View>
        </KeyboardAvoidingView>
      </Modal>
    </View>
  );
}

