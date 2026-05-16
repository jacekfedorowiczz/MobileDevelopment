// src/screens/ExerciseCreateScreen.tsx
import React, { useEffect, useState } from 'react';
import { View, Text, StyleSheet, ScrollView, TextInput, Pressable, Alert, ActivityIndicator, Switch } from 'react-native';
import Icon from 'react-native-vector-icons/Feather';
import { Spacing } from '../theme/theme';
import { useTheme } from '../context/ThemeContext';
import { useSafeAreaInsets } from 'react-native-safe-area-context';
import { ExerciseApiService, ExerciseDifficulty, MuscleGroupDto } from '../api/ExerciseApiService';
import BackButton from '../components/BackButton';

const DIFFICULTY_OPTIONS = [
  { value: ExerciseDifficulty.Beginner, label: 'Początkujący' },
  { value: ExerciseDifficulty.Intermediate, label: 'Średniozaawansowany' },
  { value: ExerciseDifficulty.Advanced, label: 'Zaawansowany' },
  { value: ExerciseDifficulty.Elite, label: 'Elita' },
];

export default function ExerciseCreateScreen({ navigation }: any) {
  const { colors } = useTheme();
  const insets = useSafeAreaInsets();
  const [name, setName] = useState('');
  const [description, setDescription] = useState('');
  const [imageUrl, setImageUrl] = useState('');
  const [isCompound, setIsCompound] = useState(false);
  const [difficulty, setDifficulty] = useState<ExerciseDifficulty | undefined>(undefined);
  const [selectedMuscleGroupIds, setSelectedMuscleGroupIds] = useState<number[]>([]);
  const [muscleGroups, setMuscleGroups] = useState<MuscleGroupDto[]>([]);
  const [isLoadingGroups, setIsLoadingGroups] = useState(true);
  const [isSaving, setIsSaving] = useState(false);

  useEffect(() => {
    ExerciseApiService.getMuscleGroups()
      .then(setMuscleGroups)
      .catch(() => Alert.alert('Błąd', 'Nie udało się pobrać grup mięśniowych'))
      .finally(() => setIsLoadingGroups(false));
  }, []);

  const toggleMuscle = (id: number) => {
    setSelectedMuscleGroupIds(prev =>
      prev.includes(id) ? prev.filter(x => x !== id) : [...prev, id],
    );
  };

  const handleCreate = async () => {
    if (!name.trim()) { Alert.alert('Błąd', 'Nazwa ćwiczenia jest wymagana'); return; }
    if (selectedMuscleGroupIds.length === 0) { Alert.alert('Błąd', 'Wybierz przynajmniej jedną partię mięśniową'); return; }

    setIsSaving(true);
    try {
      await ExerciseApiService.createExercise({
        name: name.trim(),
        description: description.trim() || undefined,
        imageUrl: imageUrl.trim() || undefined,
        isCompound,
        muscleGroupIds: selectedMuscleGroupIds,
        difficulty,
      });
      Alert.alert('Sukces', 'Dodano nowe ćwiczenie!', [{ text: 'OK', onPress: () => navigation.goBack() }]);
    } catch (e: any) {
      Alert.alert('Błąd', e.message ?? 'Nie udało się dodać ćwiczenia');
    } finally {
      setIsSaving(false);
    }
  };

  return (
    <View style={[styles.container, { backgroundColor: colors.background }]}>
      <View style={[styles.header, { backgroundColor: colors.background, paddingTop: insets.top + Spacing.sm }]}>
        <BackButton onPress={() => navigation.canGoBack() ? navigation.goBack() : navigation.navigate('Exercises')} />
        <Text style={[styles.title, { color: colors.foreground }]}>Nowe ćwiczenie</Text>
      </View>

      <ScrollView style={styles.content} contentContainerStyle={styles.scrollContent} keyboardShouldPersistTaps="handled">
        <View style={[styles.card, { backgroundColor: colors.card, borderColor: colors.border }]}>
          {/* Name */}
          <View style={styles.inputGroup}>
            <Text style={[styles.label, { color: colors.foreground }]}>Nazwa <Text style={{ color: '#dc2626' }}>*</Text></Text>
            <TextInput style={[styles.input, { backgroundColor: colors.background, borderColor: colors.border, color: colors.foreground }]} value={name} onChangeText={setName} placeholder="np. Wyciskanie hantli" placeholderTextColor={colors.mutedForeground} />
          </View>

          {/* Description */}
          <View style={styles.inputGroup}>
            <Text style={[styles.label, { color: colors.foreground }]}>Opis <Text style={[styles.opt, { color: colors.mutedForeground }]}>(opcjonalnie)</Text></Text>
            <TextInput style={[styles.input, styles.textArea, { backgroundColor: colors.background, borderColor: colors.border, color: colors.foreground }]} value={description} onChangeText={setDescription} placeholder="Wskazówki wykonania..." placeholderTextColor={colors.mutedForeground} multiline maxLength={300} textAlignVertical="top" />
            <Text style={[styles.char, { color: colors.mutedForeground }]}>{description.length}/300</Text>
          </View>

          {/* Image URL */}
          <View style={styles.inputGroup}>
            <Text style={[styles.label, { color: colors.foreground }]}>URL zdjęcia <Text style={[styles.opt, { color: colors.mutedForeground }]}>(opcjonalnie)</Text></Text>
            <TextInput style={[styles.input, { backgroundColor: colors.background, borderColor: colors.border, color: colors.foreground }]} value={imageUrl} onChangeText={setImageUrl} placeholder="https://..." placeholderTextColor={colors.mutedForeground} autoCapitalize="none" keyboardType="url" />
          </View>

          {/* IsCompound toggle */}
          <View style={[styles.switchRow, { borderColor: colors.border }]}>
            <View>
              <Text style={[styles.label, { color: colors.foreground, marginBottom: 2 }]}>Ćwiczenie złożone</Text>
              <Text style={[styles.opt, { color: colors.mutedForeground }]}>Angażuje wiele grup mięśniowych</Text>
            </View>
            <Switch value={isCompound} onValueChange={setIsCompound} trackColor={{ false: colors.border, true: '#3b82f6' }} thumbColor="#fff" />
          </View>

          {/* Difficulty */}
          <View style={styles.inputGroup}>
            <Text style={[styles.label, { color: colors.foreground }]}>Poziom trudności <Text style={[styles.opt, { color: colors.mutedForeground }]}>(opcjonalnie)</Text></Text>
            <View style={styles.pillRow}>
              {DIFFICULTY_OPTIONS.map(option => (
                <Pressable
                  key={option.value}
                  style={[styles.pill, { borderColor: difficulty === option.value ? '#3b82f6' : colors.border, backgroundColor: difficulty === option.value ? '#3b82f6' : colors.background }]}
                  onPress={() => setDifficulty(prev => prev === option.value ? undefined : option.value)}
                >
                  <Text style={[styles.pillText, { color: difficulty === option.value ? '#fff' : colors.foreground }]}>{option.label}</Text>
                </Pressable>
              ))}
            </View>
          </View>

          {/* Muscle groups */}
          <View style={styles.inputGroup}>
            <Text style={[styles.label, { color: colors.foreground }]}>Partie mięśniowe <Text style={{ color: '#dc2626' }}>*</Text></Text>
            {isLoadingGroups ? (
              <ActivityIndicator color="#3b82f6" style={{ marginTop: 8 }} />
            ) : (
              <View style={styles.muscleGrid}>
                {muscleGroups.map(mg => {
                  const selected = selectedMuscleGroupIds.includes(mg.id);
                  return (
                    <Pressable
                      key={mg.id}
                      style={[styles.muscleChip, { borderColor: selected ? '#3b82f6' : colors.border, backgroundColor: selected ? 'rgba(59,130,246,0.1)' : colors.background }]}
                      onPress={() => toggleMuscle(mg.id)}
                    >
                      {selected && <Icon name="check" size={12} color="#3b82f6" style={{ marginRight: 4 }} />}
                      <Text style={[styles.muscleText, { color: selected ? '#3b82f6' : colors.foreground }]}>{mg.name}</Text>
                    </Pressable>
                  );
                })}
              </View>
            )}
          </View>

          {/* Buttons */}
          <View style={styles.buttons}>
            <Pressable style={[styles.cancelBtn, { backgroundColor: colors.background, borderColor: colors.border }]} onPress={() => navigation.canGoBack() ? navigation.goBack() : navigation.navigate('Exercises')}>
              <Text style={[styles.cancelText, { color: colors.foreground }]}>Anuluj</Text>
            </Pressable>
            <Pressable style={[styles.saveBtn, isSaving && { opacity: 0.6 }]} onPress={handleCreate} disabled={isSaving}>
              {isSaving ? <ActivityIndicator color="#fff" size="small" /> : <Text style={styles.saveText}>Dodaj ćwiczenie</Text>}
            </Pressable>
          </View>
        </View>
      </ScrollView>
    </View>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1 },
  header: { flexDirection: 'row', alignItems: 'center', paddingHorizontal: Spacing.lg, paddingTop: Spacing.xl, paddingBottom: Spacing.md, gap: Spacing.md },
  title: { fontSize: 24, fontWeight: 'bold' },
  content: { flex: 1, paddingHorizontal: Spacing.lg },
  scrollContent: { paddingBottom: Spacing.xl * 2 },
  card: { borderRadius: 20, padding: Spacing.lg, borderWidth: 1 },
  inputGroup: { marginBottom: Spacing.md },
  label: { fontSize: 14, fontWeight: '500', marginBottom: 8 },
  opt: { fontWeight: 'normal', fontSize: 13 },
  input: { height: 48, borderRadius: 12, paddingHorizontal: Spacing.md, borderWidth: 1, fontSize: 14 },
  textArea: { height: 88, paddingTop: Spacing.sm },
  char: { fontSize: 12, textAlign: 'right', marginTop: 4 },
  switchRow: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center', paddingVertical: Spacing.sm, borderBottomWidth: 1, marginBottom: Spacing.md },
  pillRow: { flexDirection: 'row', flexWrap: 'wrap', gap: 8 },
  pill: { paddingHorizontal: 12, paddingVertical: 8, borderRadius: 10, borderWidth: 1.5 },
  pillText: { fontSize: 13, fontWeight: '500' },
  muscleGrid: { flexDirection: 'row', flexWrap: 'wrap', gap: 8 },
  muscleChip: { flexDirection: 'row', alignItems: 'center', paddingHorizontal: 12, paddingVertical: 8, borderRadius: 20, borderWidth: 1.5 },
  muscleText: { fontSize: 13, fontWeight: '500' },
  buttons: { flexDirection: 'row', gap: Spacing.sm, marginTop: Spacing.md },
  cancelBtn: { flex: 1, height: 48, borderRadius: 12, alignItems: 'center', justifyContent: 'center', borderWidth: 1 },
  cancelText: { fontSize: 14, fontWeight: '500' },
  saveBtn: { flex: 1, height: 48, borderRadius: 12, backgroundColor: '#3b82f6', alignItems: 'center', justifyContent: 'center' },
  saveText: { fontSize: 14, fontWeight: '600', color: '#fff' },
});

