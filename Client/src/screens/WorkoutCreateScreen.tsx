// src/screens/WorkoutCreateScreen.tsx
import React, { useState } from 'react';
import {
  View, Text, StyleSheet, ScrollView, TextInput, Pressable, Alert, ActivityIndicator,
} from 'react-native';
import DateTimePicker from '@react-native-community/datetimepicker';
import Icon from 'react-native-vector-icons/Feather';
import { Spacing } from '../theme/theme';
import { useTheme } from '../context/ThemeContext';
import { useSafeAreaInsets } from 'react-native-safe-area-context';
import { WorkoutSessionApiService } from '../api/WorkoutSessionApiService';
import BackButton from '../components/BackButton';

export default function WorkoutCreateScreen({ navigation }: any) {
  const { colors } = useTheme();
  const insets = useSafeAreaInsets();
  const [name, setName] = useState('');
  const [description, setDescription] = useState('');
  const [workoutDate, setWorkoutDate] = useState(new Date());
  const [showDatePicker, setShowDatePicker] = useState(false);
  const [isLoading, setIsLoading] = useState(false);

  const getStartTime = () => {
    const now = new Date();
    const selected = new Date(workoutDate);
    selected.setHours(now.getHours(), now.getMinutes(), now.getSeconds(), now.getMilliseconds());
    return selected.toISOString();
  };

  const handleDateChange = (_event: any, selectedDate?: Date) => {
    setShowDatePicker(false);
    if (!selectedDate) return;

    const today = new Date();
    today.setHours(23, 59, 59, 999);
    if (selectedDate > today) {
      Alert.alert('Błąd', 'Data treningu nie może być późniejsza niż dzisiaj.');
      return;
    }

    setWorkoutDate(selectedDate);
  };

  const handleCreate = async () => {
    if (!name.trim()) {
      Alert.alert('Błąd', 'Nazwa treningu jest wymagana');
      return;
    }

    setIsLoading(true);
    try {
      const session = await WorkoutSessionApiService.createSession({
        name: name.trim(),
        description: description.trim() || undefined,
        startTime: getStartTime(),
      });
      navigation.replace('WorkoutDetail', { id: session.id });
    } catch (e: any) {
      Alert.alert('Błąd', e.message ?? 'Nie udało się utworzyć treningu');
      setIsLoading(false);
    }
  };

  return (
    <View style={[styles.container, { backgroundColor: colors.background }]}>
      {/* Header */}
      <View style={[styles.header, { backgroundColor: colors.background, paddingTop: insets.top + Spacing.sm }]}>
        <BackButton onPress={() => navigation.canGoBack() ? navigation.goBack() : navigation.navigate('WorkoutLog')} />
        <Text style={[styles.title, { color: colors.foreground }]}>Nowy trening</Text>
      </View>

      <ScrollView style={styles.content} contentContainerStyle={styles.scrollContent} keyboardShouldPersistTaps="handled">
        {/* Card */}
        <View style={[styles.card, { backgroundColor: colors.card, borderColor: colors.border }]}>
          <Text style={[styles.cardTitle, { color: colors.foreground }]}>Informacje</Text>

          {/* Name */}
          <View style={styles.inputGroup}>
            <Text style={[styles.label, { color: colors.foreground }]}>Nazwa treningu <Text style={{ color: '#dc2626' }}>*</Text></Text>
            <TextInput
              style={[styles.input, { backgroundColor: colors.background, borderColor: colors.border, color: colors.foreground }]}
              value={name}
              onChangeText={setName}
              placeholder="np. Push A — Klatka i Triceps"
              placeholderTextColor={colors.mutedForeground}
              maxLength={100}
            />
          </View>

          {/* Description */}
          <View style={styles.inputGroup}>
            <Text style={[styles.label, { color: colors.foreground }]}>
              Notatki <Text style={[styles.optional, { color: colors.mutedForeground }]}>(opcjonalnie)</Text>
            </Text>
            <TextInput
              style={[styles.input, styles.textArea, { backgroundColor: colors.background, borderColor: colors.border, color: colors.foreground }]}
              value={description}
              onChangeText={setDescription}
              placeholder="Dodaj notatki do treningu..."
              placeholderTextColor={colors.mutedForeground}
              multiline
              maxLength={300}
              textAlignVertical="top"
            />
            <Text style={[styles.charCount, { color: colors.mutedForeground }]}>{description.length} / 300</Text>
          </View>

          <View style={styles.inputGroup}>
            <Text style={[styles.label, { color: colors.foreground }]}>Data treningu</Text>
            <Pressable
              style={[styles.dateButton, { backgroundColor: colors.background, borderColor: colors.border }]}
              onPress={() => setShowDatePicker(true)}
            >
              <Icon name="calendar" size={18} color="#3b82f6" />
              <Text style={[styles.dateButtonText, { color: colors.foreground }]}>
                {workoutDate.toLocaleDateString('pl-PL', { day: '2-digit', month: 'long', year: 'numeric' })}
              </Text>
            </Pressable>
            {showDatePicker && (
              <DateTimePicker
                value={workoutDate}
                mode="date"
                display="default"
                maximumDate={new Date()}
                onChange={handleDateChange}
              />
            )}
          </View>

        </View>

        {/* Start info */}
        <View style={[styles.infoBox, { backgroundColor: 'rgba(59, 130, 246, 0.08)', borderColor: 'rgba(59, 130, 246, 0.25)' }]}>
          <Icon name="info" size={16} color="#3b82f6" />
          <Text style={[styles.infoText, { color: colors.mutedForeground }]}>
            Trening zostanie zapisany z wybraną datą. RPE sesji wyliczymy automatycznie ze średniej RPE dodanych ćwiczeń.
          </Text>
        </View>

        {/* Buttons */}
        <View style={styles.buttons}>
          <Pressable
            style={[styles.cancelBtn, { backgroundColor: colors.card, borderColor: colors.border }]}
            onPress={() => navigation.canGoBack() ? navigation.goBack() : navigation.navigate('WorkoutLog')}
          >
            <Text style={[styles.cancelText, { color: colors.foreground }]}>Anuluj</Text>
          </Pressable>
          <Pressable
            style={[styles.startBtn, isLoading && styles.startBtnDisabled]}
            onPress={handleCreate}
            disabled={isLoading}
          >
            {isLoading ? (
              <ActivityIndicator color="#fff" size="small" />
            ) : (
              <>
                <Text style={styles.startText}>Zacznij trening</Text>
              </>
            )}
          </Pressable>
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
  card: { borderRadius: 20, padding: Spacing.lg, borderWidth: 1, marginBottom: Spacing.md },
  cardTitle: { fontSize: 18, fontWeight: '600', marginBottom: Spacing.lg },
  inputGroup: { marginBottom: Spacing.md },
  label: { fontSize: 14, fontWeight: '500', marginBottom: 8 },
  optional: { fontWeight: 'normal', fontSize: 13 },
  input: { height: 48, borderRadius: 12, paddingHorizontal: Spacing.md, borderWidth: 1, fontSize: 14 },
  textArea: { height: 96, paddingTop: Spacing.sm },
  charCount: { fontSize: 12, textAlign: 'right', marginTop: 4 },
  dateButton: { height: 48, borderRadius: 12, paddingHorizontal: Spacing.md, borderWidth: 1, flexDirection: 'row', alignItems: 'center' },
  dateButtonText: { fontSize: 14, fontWeight: '500', marginLeft: Spacing.sm, textTransform: 'capitalize' },
  infoBox: { flexDirection: 'row', alignItems: 'flex-start', gap: 10, padding: Spacing.md, borderRadius: 12, borderWidth: 1, marginBottom: Spacing.lg },
  infoText: { flex: 1, fontSize: 13, lineHeight: 20 },
  buttons: { flexDirection: 'row', gap: Spacing.sm },
  cancelBtn: { flex: 1, height: 52, borderRadius: 14, alignItems: 'center', justifyContent: 'center', borderWidth: 1 },
  cancelText: { fontSize: 15, fontWeight: '500' },
  startBtn: { flex: 2, height: 52, borderRadius: 14, backgroundColor: '#3b82f6', alignItems: 'center', justifyContent: 'center' },
  startBtnDisabled: { backgroundColor: '#93c5fd' },
  startText: { fontSize: 15, fontWeight: '600', color: '#fff' },
});
