// src/screens/DietAssumptionsScreen.tsx
import React, { useEffect, useState } from 'react';
import {
  View, Text, StyleSheet, Pressable, ScrollView, Alert,
  Modal, TouchableOpacity, TextInput
} from 'react-native';
import { useSafeAreaInsets } from 'react-native-safe-area-context';
import Icon from 'react-native-vector-icons/Feather';
import { Spacing } from '../theme/theme';
import { useTheme } from '../context/ThemeContext';
import { useDietStore } from '../store/useDietStore';
import BackButton, { backButtonSpacing } from '../components/BackButton';
import { ProfileService } from '../api/ProfileService';

const DIET_TYPES = [
  { key: 'balanced', label: 'Zbilansowana', desc: 'Równy rozkład makroskładników', icon: 'activity' },
  { key: 'low_carb', label: 'Low Carb', desc: 'Ograniczone węglowodany (<100g)', icon: 'trending-down' },
  { key: 'keto', label: 'Keto', desc: 'Dieta ketogeniczna (<50g węglowodanów)', icon: 'zap' },
  { key: 'high_protein', label: 'Wysokobiałkowa', desc: 'Maksymalna budowa masy mięśniowej', icon: 'award' },
  { key: 'vegan', label: 'Wegańska', desc: 'Bez produktów odzwierzęcych', icon: 'feather' },
  { key: 'paleo', label: 'Paleo', desc: 'Naturalne, nieprzetworzone produkty', icon: 'sun' },
];

export default function DietAssumptionsScreen({ navigation }: any) {
  const { activeDiet, createDiet } = useDietStore();
  const { colors } = useTheme();
  const insets = useSafeAreaInsets();

  const defaultCalories = parseInt(activeDiet?.description?.replace(/[^0-9]/g, '') || '2500', 10);

  const [dietName, setDietName] = useState(activeDiet?.name || 'Moja Dieta');
  const [calories, setCalories] = useState(defaultCalories);
  const [dietType, setDietType] = useState('balanced');
  const [showTypePicker, setShowTypePicker] = useState(false);
  const [isSaving, setIsSaving] = useState(false);

  const selectedType = DIET_TYPES.find(d => d.key === dietType) ?? DIET_TYPES[0];

  useEffect(() => {
    ProfileService.getProfile().then(response => {
      if (!response.isSuccess || !response.value) return;

      setDietType(response.value.dietType || 'balanced');
      if (response.value.dailyCaloriesGoal) {
        setCalories(response.value.dailyCaloriesGoal);
      }
    });
  }, []);

  // Macro splits by diet type
  const getMacros = () => {
    switch (dietType) {
      case 'low_carb': return { protein: 30, carbs: 20, fat: 50 };
      case 'keto': return { protein: 25, carbs: 5, fat: 70 };
      case 'high_protein': return { protein: 40, carbs: 40, fat: 20 };
      case 'vegan': return { protein: 20, carbs: 55, fat: 25 };
      case 'paleo': return { protein: 35, carbs: 35, fat: 30 };
      default: return { protein: 30, carbs: 40, fat: 30 };
    }
  };

  const macros = getMacros();
  const proteinG = Math.round((calories * macros.protein / 100) / 4);
  const carbsG = Math.round((calories * macros.carbs / 100) / 4);
  const fatG = Math.round((calories * macros.fat / 100) / 9);

  const handleSave = async () => {
    setIsSaving(true);
    try {
      if (!dietName.trim()) {
        Alert.alert('Błąd', 'Podaj nazwę diety.');
        return;
      }

      const profileResult = await ProfileService.updateDietAssumptions({
        dietType,
        dailyCaloriesGoal: calories,
        proteinPercentage: macros.protein,
        carbsPercentage: macros.carbs,
        fatPercentage: macros.fat,
      });

      if (!profileResult.isSuccess) {
        Alert.alert('Błąd', profileResult.errorMessage || 'Nie udało się zapisać założeń diety.');
        return;
      }

      await createDiet(dietName, calories);
      Alert.alert('Zapisano', 'Założenia diety zostały zaktualizowane.', [
        { text: 'OK', onPress: () => navigation.goBack() }
      ]);
    } catch (e: any) {
      Alert.alert('Błąd', e.message ?? 'Nie udało się zapisać diety');
    } finally {
      setIsSaving(false);
    }
  };

  return (
    <View style={[styles.container, { backgroundColor: colors.background }]}>
      {/* Header */}
      <View style={[styles.header, { backgroundColor: colors.background, paddingTop: insets.top + Spacing.sm }]}>
        <BackButton onPress={() => navigation.canGoBack() ? navigation.goBack() : navigation.navigate('Diet')} style={backButtonSpacing} />
        <Text style={[styles.title, { color: colors.foreground }]}>Założenia diety</Text>
        <Text style={[styles.subtitle, { color: colors.mutedForeground }]}>Dostosuj cele i typ żywienia</Text>
      </View>

      <ScrollView style={styles.content} contentContainerStyle={[styles.scrollContent, { paddingBottom: insets.bottom + 120 }]} showsVerticalScrollIndicator={false}>
        <View style={[styles.card, { backgroundColor: colors.card, borderColor: colors.border }]}>
          <View style={styles.cardHeader}>
            <View style={styles.cardIcon}>
              <Icon name="edit-3" size={18} color="#3b82f6" />
            </View>
            <Text style={[styles.cardTitle, { color: colors.foreground }]}>Nazwa diety</Text>
          </View>
          <TextInput
            style={[styles.nameInput, { color: colors.foreground, backgroundColor: colors.background, borderColor: colors.border }]}
            value={dietName}
            onChangeText={setDietName}
            placeholder="np. Moja dieta"
            placeholderTextColor={colors.mutedForeground}
          />
        </View>

        {/* Calorie Goal Slider */}
        <View style={[styles.card, { backgroundColor: colors.card, borderColor: colors.border }]}>
          <View style={styles.cardHeader}>
            <View style={styles.cardIcon}>
              <Icon name="target" size={18} color="#3b82f6" />
            </View>
            <Text style={[styles.cardTitle, { color: colors.foreground }]}>Cel kaloryczny</Text>
          </View>

          <View style={styles.stepperRow}>
            <Pressable
              style={[styles.stepBtn, { backgroundColor: colors.background, borderColor: colors.border }]}
              onPress={() => setCalories(c => Math.max(1000, c - 50))}
              onLongPress={() => setCalories(c => Math.max(1000, c - 200))}
            >
              <Icon name="minus" size={22} color={colors.foreground} />
            </Pressable>

            <View style={styles.stepperCenter}>
              <TextInput
                style={[styles.calorieInput, { color: '#3b82f6', borderColor: colors.border }]}
                keyboardType="numeric"
                value={String(calories)}
                onChangeText={v => {
                  const n = parseInt(v.replace(/[^0-9]/g, ''), 10);
                  if (!isNaN(n)) setCalories(Math.min(5000, Math.max(1000, n)));
                }}
              />
              <Text style={[styles.calorieUnit, { color: colors.mutedForeground }]}>kcal / dzień</Text>
            </View>

            <Pressable
              style={[styles.stepBtn, { backgroundColor: colors.background, borderColor: colors.border }]}
              onPress={() => setCalories(c => Math.min(5000, c + 50))}
              onLongPress={() => setCalories(c => Math.min(5000, c + 200))}
            >
              <Icon name="plus" size={22} color={colors.foreground} />
            </Pressable>
          </View>

          <View style={styles.stepperHint}>
            <Text style={[styles.sliderLabel, { color: colors.mutedForeground }]}>min 1000</Text>
            <Text style={[styles.sliderLabel, { color: colors.mutedForeground }]}>Przytrzymaj aby zmienić o ±200</Text>
            <Text style={[styles.sliderLabel, { color: colors.mutedForeground }]}>max 5000</Text>
          </View>
        </View>

        {/* Diet Type Picker */}
        <View style={[styles.card, { backgroundColor: colors.card, borderColor: colors.border }]}>
          <View style={styles.cardHeader}>
            <View style={[styles.cardIcon, { backgroundColor: '#f59e0b20' }]}>
              <Icon name="layers" size={18} color="#f59e0b" />
            </View>
            <Text style={[styles.cardTitle, { color: colors.foreground }]}>Typ diety</Text>
          </View>

          <Pressable
            style={[styles.typePickerButton, { backgroundColor: colors.background, borderColor: colors.border }]}
            onPress={() => setShowTypePicker(true)}
          >
            <Icon name={selectedType.icon as any} size={20} color="#f59e0b" />
            <View style={{ flex: 1, marginLeft: Spacing.md }}>
              <Text style={[styles.typeLabel, { color: colors.foreground }]}>{selectedType.label}</Text>
              <Text style={[styles.typeDesc, { color: colors.mutedForeground }]}>{selectedType.desc}</Text>
            </View>
            <Icon name="chevron-down" size={18} color={colors.mutedForeground} />
          </Pressable>
        </View>

        {/* Macro Preview */}
        <View style={[styles.card, { backgroundColor: colors.card, borderColor: colors.border }]}>
          <View style={styles.cardHeader}>
            <View style={[styles.cardIcon, { backgroundColor: '#8b5cf620' }]}>
              <Icon name="pie-chart" size={18} color="#8b5cf6" />
            </View>
            <Text style={[styles.cardTitle, { color: colors.foreground }]}>Podział makroskładników</Text>
          </View>

          <View style={styles.macroBar}>
            <View style={[styles.macroSegment, { flex: macros.protein, backgroundColor: '#3b82f6' }]} />
            <View style={[styles.macroSegment, { flex: macros.carbs, backgroundColor: '#f59e0b' }]} />
            <View style={[styles.macroSegment, { flex: macros.fat, backgroundColor: '#ef4444' }]} />
          </View>

          <View style={styles.macroLegend}>
            {[
              { label: 'Białko', value: proteinG, pct: macros.protein, color: '#3b82f6' },
              { label: 'Węglowodany', value: carbsG, pct: macros.carbs, color: '#f59e0b' },
              { label: 'Tłuszcze', value: fatG, pct: macros.fat, color: '#ef4444' },
            ].map(m => (
              <View key={m.label} style={styles.macroItem}>
                <View style={[styles.macroDot, { backgroundColor: m.color }]} />
                <View>
                  <Text style={[styles.macroLabel, { color: colors.mutedForeground }]}>{m.label}</Text>
                  <Text style={[styles.macroVal, { color: colors.foreground }]}>{m.value}g <Text style={{ color: colors.mutedForeground, fontSize: 11 }}>({m.pct}%)</Text></Text>
                </View>
              </View>
            ))}
          </View>
        </View>

        {/* Quick Presets */}
        <Text style={[styles.presetsTitle, { color: colors.mutedForeground }]}>Szybkie szablony</Text>
        <View style={styles.presetsRow}>
          {[
            { label: 'Redukcja', kcal: 1800 },
            { label: 'Utrzymanie', kcal: 2400 },
            { label: 'Masa', kcal: 3200 },
          ].map(p => (
            <Pressable key={p.label} style={[styles.presetBtn, { backgroundColor: colors.card, borderColor: calories === p.kcal ? '#3b82f6' : colors.border }]} onPress={() => setCalories(p.kcal)}>
              <Text style={[styles.presetLabel, { color: calories === p.kcal ? '#3b82f6' : colors.foreground }]}>{p.label}</Text>
              <Text style={[styles.presetKcal, { color: colors.mutedForeground }]}>{p.kcal} kcal</Text>
            </Pressable>
          ))}
        </View>

      </ScrollView>

      {/* Footer */}
      <View style={[styles.footer, { backgroundColor: colors.card, borderTopColor: colors.border, paddingBottom: insets.bottom + Spacing.sm }]}>
        <Pressable style={[styles.saveBtn, { opacity: isSaving ? 0.6 : 1 }]} onPress={handleSave} disabled={isSaving}>
          <Icon name="check" size={20} color="#fff" />
          <Text style={styles.saveBtnText}>Zapisz założenia</Text>
        </Pressable>
      </View>

      {/* Diet Type Picker Modal */}
      <Modal visible={showTypePicker} transparent animationType="slide" onRequestClose={() => setShowTypePicker(false)}>
        <View style={styles.modalOverlay}>
          <Pressable style={styles.modalDismiss} onPress={() => setShowTypePicker(false)} />
          <View style={[styles.modalSheet, { backgroundColor: colors.card, paddingBottom: insets.bottom + Spacing.lg }]}>
            <View style={styles.modalHandle} />
            <Text style={[styles.modalTitle, { color: colors.foreground }]}>Wybierz typ diety</Text>
            {DIET_TYPES.map(dt => (
              <TouchableOpacity
                key={dt.key}
                style={[styles.typeOption, { borderColor: dietType === dt.key ? '#3b82f6' : colors.border, backgroundColor: dietType === dt.key ? '#3b82f620' : colors.background }]}
                onPress={() => { setDietType(dt.key); setShowTypePicker(false); }}
              >
                <View style={[styles.typeOptionIcon, { backgroundColor: '#f59e0b20' }]}>
                  <Icon name={dt.icon as any} size={18} color="#f59e0b" />
                </View>
                <View style={{ flex: 1 }}>
                  <Text style={[styles.typeOptionLabel, { color: colors.foreground }]}>{dt.label}</Text>
                  <Text style={[styles.typeOptionDesc, { color: colors.mutedForeground }]}>{dt.desc}</Text>
                </View>
                {dietType === dt.key && <Icon name="check" size={18} color="#3b82f6" />}
              </TouchableOpacity>
            ))}
          </View>
        </View>
      </Modal>
    </View>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1 },
  header: { paddingHorizontal: Spacing.lg, paddingBottom: Spacing.md },
  title: { fontSize: 28, fontWeight: 'bold' },
  subtitle: { fontSize: 14, marginTop: 4 },
  content: { flex: 1, paddingHorizontal: Spacing.lg },
  scrollContent: { paddingTop: Spacing.md },
  card: { borderRadius: 16, padding: Spacing.lg, borderWidth: 1, marginBottom: Spacing.md },
  cardHeader: { flexDirection: 'row', alignItems: 'center', marginBottom: Spacing.lg },
  cardIcon: { width: 36, height: 36, borderRadius: 10, backgroundColor: '#3b82f620', alignItems: 'center', justifyContent: 'center', marginRight: Spacing.md },
  cardTitle: { fontSize: 16, fontWeight: '600' },
  nameInput: { height: 48, borderRadius: 12, paddingHorizontal: Spacing.md, borderWidth: 1, fontSize: 14 },
  calorieInput: { fontSize: 48, fontWeight: 'bold', textAlign: 'center', minWidth: 140, borderBottomWidth: 2, paddingBottom: 4 },
  calorieUnit: { fontSize: 14, marginTop: 4, textAlign: 'center' },
  stepperRow: { flexDirection: 'row', alignItems: 'center', justifyContent: 'space-between', marginTop: Spacing.sm },
  stepperCenter: { alignItems: 'center', flex: 1 },
  stepBtn: { width: 52, height: 52, borderRadius: 14, borderWidth: 1, alignItems: 'center', justifyContent: 'center' },
  stepperHint: { flexDirection: 'row', justifyContent: 'space-between', marginTop: Spacing.sm },
  sliderLabels: { flexDirection: 'row', justifyContent: 'space-between', marginTop: -8 },
  sliderLabel: { fontSize: 11 },
  typePickerButton: { flexDirection: 'row', alignItems: 'center', borderRadius: 12, borderWidth: 1, padding: Spacing.md },
  typeLabel: { fontSize: 15, fontWeight: '600' },
  typeDesc: { fontSize: 12, marginTop: 2 },
  macroBar: { flexDirection: 'row', height: 12, borderRadius: 6, overflow: 'hidden', marginBottom: Spacing.lg, gap: 2 },
  macroSegment: { borderRadius: 6 },
  macroLegend: { flexDirection: 'row', justifyContent: 'space-between' },
  macroItem: { flexDirection: 'row', alignItems: 'center', gap: 8 },
  macroDot: { width: 10, height: 10, borderRadius: 5 },
  macroLabel: { fontSize: 11 },
  macroVal: { fontSize: 14, fontWeight: '600' },
  presetsTitle: { fontSize: 12, fontWeight: '600', textTransform: 'uppercase', letterSpacing: 0.5, marginBottom: Spacing.sm },
  presetsRow: { flexDirection: 'row', gap: Spacing.sm, marginBottom: Spacing.lg },
  presetBtn: { flex: 1, borderRadius: 12, borderWidth: 1.5, padding: Spacing.md, alignItems: 'center' },
  presetLabel: { fontSize: 13, fontWeight: '600' },
  presetKcal: { fontSize: 11, marginTop: 2 },
  footer: { padding: Spacing.lg, borderTopWidth: 1 },
  saveBtn: { flexDirection: 'row', alignItems: 'center', justifyContent: 'center', backgroundColor: '#3b82f6', height: 54, borderRadius: 14, gap: 8 },
  saveBtnText: { color: '#fff', fontSize: 16, fontWeight: '600' },
  modalOverlay: { flex: 1, justifyContent: 'flex-end' },
  modalDismiss: { flex: 1, backgroundColor: 'rgba(0,0,0,0.4)' },
  modalSheet: { borderTopLeftRadius: 24, borderTopRightRadius: 24, padding: Spacing.lg, paddingTop: Spacing.md },
  modalHandle: { width: 40, height: 4, borderRadius: 2, backgroundColor: '#94a3b8', alignSelf: 'center', marginBottom: Spacing.lg },
  modalTitle: { fontSize: 20, fontWeight: 'bold', marginBottom: Spacing.lg },
  typeOption: { flexDirection: 'row', alignItems: 'center', borderRadius: 12, borderWidth: 1.5, padding: Spacing.md, marginBottom: Spacing.sm, gap: Spacing.md },
  typeOptionIcon: { width: 36, height: 36, borderRadius: 10, alignItems: 'center', justifyContent: 'center' },
  typeOptionLabel: { fontSize: 15, fontWeight: '600' },
  typeOptionDesc: { fontSize: 12, marginTop: 2 },
});
