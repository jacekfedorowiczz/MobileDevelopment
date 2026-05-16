// src/screens/DietScreen.tsx
import React, { useEffect, useState } from 'react';
import { View, Text, ScrollView, Pressable, ActivityIndicator, Modal, TextInput, Alert, KeyboardAvoidingView, Platform } from 'react-native';
import { useSafeAreaInsets } from 'react-native-safe-area-context';
import Icon from 'react-native-vector-icons/Feather';
import { Colors, Spacing } from '../theme/theme';
import { useTheme } from '../context/ThemeContext';
import { useDietStore } from '../store/useDietStore';
import BackButton, { backButtonSpacing } from '../components/BackButton';
import { ProfileData, ProfileService } from '../api/ProfileService';
import { styles } from './DietScreen.styles';

import { useFocusEffect } from '@react-navigation/native';

export default function DietScreen({ navigation }: any) {
  const { activeDiet, activeDay, isLoading, error, fetchActiveDiet, selectDay, createDay, addMeal, removeMeal } = useDietStore();
  const { colors } = useTheme();
  const insets = useSafeAreaInsets();
  const [selectedDate, setSelectedDate] = useState<string>(new Date().toISOString().split('T')[0]);
  const [weekAnchorDate, setWeekAnchorDate] = useState<Date>(new Date());
  const [profile, setProfile] = useState<ProfileData | null>(null);
  const [dayModalVisible, setDayModalVisible] = useState(false);
  const [newDayDate, setNewDayDate] = useState<string>(new Date().toISOString().split('T')[0]);
  const [newDayNotes, setNewDayNotes] = useState('');
  const [mealModalVisible, setMealModalVisible] = useState(false);
  const [mealName, setMealName] = useState('');
  const [mealCalories, setMealCalories] = useState('');
  const [mealProtein, setMealProtein] = useState('');
  const [mealCarbs, setMealCarbs] = useState('');
  const [mealFats, setMealFats] = useState('');

  useFocusEffect(
    React.useCallback(() => {
      fetchActiveDiet();
      ProfileService.getProfile().then(response => {
        if (response.isSuccess && response.value) {
          setProfile(response.value);
        }
      });
    }, [fetchActiveDiet])
  );

  useEffect(() => {
    if (error) {
      import('react-native').then(rn => {
        rn.Alert.alert('Błąd API (Restart Backend Required?)', error);
      });
    }
  }, [error]);

  useEffect(() => {
    if (activeDay?.date) {
      const dateStr = activeDay.date.split('T')[0];
      setSelectedDate(dateStr);
      setWeekAnchorDate(new Date(dateStr));
    }
  }, [activeDay]);

  const handleSelectDay = (dateStr: string) => {
    setSelectedDate(dateStr);
    setWeekAnchorDate(new Date(dateStr));
    selectDay(dateStr);
  };

  const getDateKey = (date: Date) => date.toISOString().split('T')[0];

  const getWeekStart = (date: Date) => {
    const result = new Date(date);
    const day = result.getDay();
    const diff = day === 0 ? -6 : 1 - day;
    result.setDate(result.getDate() + diff);
    return result;
  };

  const getWeekDays = () => {
    const weekStart = getWeekStart(weekAnchorDate);
    const days = [];
    for (let i = 0; i < 7; i++) {
      const d = new Date(weekStart);
      d.setDate(weekStart.getDate() + i);
      days.push({
        day: d.toLocaleDateString('pl-PL', { weekday: 'short' }).toUpperCase().replace('.', ''),
        dateStr: getDateKey(d),
        dayNum: d.getDate().toString()
      });
    }
    return days;
  };

  const changeWeek = (direction: -1 | 1) => {
    const nextDate = new Date(selectedDate);
    nextDate.setDate(nextDate.getDate() + direction * 7);
    handleSelectDay(getDateKey(nextDate));
  };

  const changeDay = (direction: -1 | 1) => {
    const nextDate = new Date(selectedDate);
    nextDate.setDate(nextDate.getDate() + direction);
    handleSelectDay(getDateKey(nextDate));
  };

  const openDayModal = () => {
    setNewDayDate(selectedDate);
    setNewDayNotes('');
    setDayModalVisible(true);
  };

  const handleCreateDay = async () => {
    if (!/^\d{4}-\d{2}-\d{2}$/.test(newDayDate)) {
      Alert.alert('Błąd', 'Podaj datę w formacie RRRR-MM-DD.');
      return;
    }

    await createDay(newDayDate, newDayNotes);
    setSelectedDate(newDayDate);
    setWeekAnchorDate(new Date(newDayDate));
    setDayModalVisible(false);
  };

  const resetMealForm = () => {
    setMealName('');
    setMealCalories('');
    setMealProtein('');
    setMealCarbs('');
    setMealFats('');
  };

  const handleAddMeal = async () => {
    if (!activeDay?.id) {
      Alert.alert('Brak dnia', 'Najpierw wybierz lub dodaj dzień diety.');
      return;
    }

    if (!mealName.trim()) {
      Alert.alert('Błąd', 'Podaj nazwę posiłku.');
      return;
    }

    const totalCalories = Number(mealCalories.replace(',', '.'));
    const protein = Number(mealProtein.replace(',', '.')) || 0;
    const carbs = Number(mealCarbs.replace(',', '.')) || 0;
    const fats = Number(mealFats.replace(',', '.')) || 0;

    if (!Number.isFinite(totalCalories) || totalCalories <= 0) {
      Alert.alert('Błąd', 'Podaj poprawną kaloryczność posiłku.');
      return;
    }

    await addMeal({
      dietDayId: activeDay.id,
      name: mealName.trim(),
      totalCalories,
      protein,
      carbs,
      fats,
      time: new Date().toISOString()
    });

    setMealModalVisible(false);
    resetMealForm();
  };

  const getTargetMacros = (targetCalories: number) => {
    const proteinPct = profile?.proteinPercentage ?? 30;
    const carbsPct = profile?.carbsPercentage ?? 40;
    const fatPct = profile?.fatPercentage ?? 30;

    return {
      protein: Math.round((targetCalories * proteinPct / 100) / 4),
      carbs: Math.round((targetCalories * carbsPct / 100) / 4),
      fats: Math.round((targetCalories * fatPct / 100) / 9),
    };
  };

  const getComparison = (current: number, target: number, unit: string) => {
    const diff = Math.round(current - target);
    const absDiff = Math.abs(diff);
    const status = target === 0 ? 'neutral' : diff > target * 0.05 ? 'over' : diff < -target * 0.05 ? 'under' : 'ok';

    if (status === 'over') return { status, text: `Przekroczono o ${absDiff}${unit}` };
    if (status === 'under') return { status, text: `Brakuje ${absDiff}${unit}` };
    return { status, text: 'Cel spełniony' };
  };

  const getStatusStyle = (status: string) => {
    switch (status) {
      case 'over': return styles.status_over;
      case 'under': return styles.status_under;
      case 'ok': return styles.status_ok;
      default: return styles.status_neutral;
    }
  };

  const weekDays = getWeekDays();

  const renderContent = () => {
    if (isLoading && !activeDiet) {
      return (
        <View style={styles.centerContainer}>
          <ActivityIndicator size="large" color={Colors.primary} />
        </View>
      );
    }

    if (!activeDiet) {
      return (
        <View style={styles.centerContainer}>
          <Text style={styles.emptyText}>Brak aktywnej diety</Text>
          <Pressable
            style={styles.primaryButton}
            onPress={() => navigation.navigate('DietAssumptions')}
          >
            <Text style={styles.primaryButtonText}>Dodaj dietę</Text>
          </Pressable>
        </View>
      );
    }

    const consumedCalories = activeDay?.meals?.reduce((sum, m) => sum + m.totalCalories, 0) || 0;
    const consumedProtein = activeDay?.meals?.reduce((sum, m) => sum + m.protein, 0) || 0;
    const consumedCarbs = activeDay?.meals?.reduce((sum, m) => sum + m.carbs, 0) || 0;
    const consumedFats = activeDay?.meals?.reduce((sum, m) => sum + m.fats, 0) || 0;
    const targetCalories = profile?.dailyCaloriesGoal ?? parseInt(activeDiet.description?.replace(/[^0-9]/g, '') || '2500', 10);
    const targetMacros = getTargetMacros(targetCalories);
    const remainingCalories = Math.max(0, targetCalories - consumedCalories);
    const calorieComparison = getComparison(consumedCalories, targetCalories, ' kcal');
    const proteinComparison = getComparison(consumedProtein, targetMacros.protein, 'g');
    const carbsComparison = getComparison(consumedCarbs, targetMacros.carbs, 'g');
    const fatsComparison = getComparison(consumedFats, targetMacros.fats, 'g');
    const selectedDateLabel = new Date(selectedDate).toLocaleDateString('pl-PL', {
      weekday: 'long',
      day: 'numeric',
      month: 'long'
    });

    return (
      <>
        <View style={styles.calendarRow}>
          <Pressable style={[styles.navButton, { backgroundColor: colors.card, borderColor: colors.border }]} onPress={() => changeWeek(-1)}>
            <Icon name="chevron-left" size={20} color={colors.mutedForeground} />
          </Pressable>
          <View style={styles.daysRow}>
            {weekDays.map((item, index) => {
              const isActive = item.dateStr === selectedDate;
              return (
                <Pressable
                  key={index}
                  onPress={() => handleSelectDay(item.dateStr)}
                  style={[
                    styles.dayItem,
                    { backgroundColor: colors.card, borderColor: colors.border },
                    isActive && styles.dayActive
                  ]}
                >
                  <Text style={[styles.dayName, { color: colors.mutedForeground }, isActive && styles.dayNameActive]}>{item.day}</Text>
                  <Text style={[styles.dayNumber, { color: colors.foreground }, isActive && styles.dayNumberActive]}>{item.dayNum}</Text>
                </Pressable>
              );
            })}
          </View>
          <Pressable style={[styles.navButton, { backgroundColor: colors.card, borderColor: colors.border }]} onPress={() => changeWeek(1)}>
            <Icon name="chevron-right" size={20} color={colors.mutedForeground} />
          </Pressable>
        </View>

        <ScrollView style={styles.content} contentContainerStyle={styles.scrollContent}>
          <View style={[styles.selectedDayCard, { backgroundColor: colors.card, borderColor: colors.border }]}>
            <Pressable style={styles.dayChangeButton} onPress={() => changeDay(-1)}>
              <Icon name="chevron-left" size={18} color={colors.mutedForeground} />
            </Pressable>
            <View>
              <Text style={[styles.selectedDayLabel, { color: colors.mutedForeground }]}>Wybrany dzień</Text>
              <Text style={[styles.selectedDayTitle, { color: colors.foreground }]}>{selectedDateLabel}</Text>
            </View>
            <View style={{ alignItems: 'flex-end' }}>
              <Text style={[styles.selectedDayMeals, { color: colors.mutedForeground }]}>
                {activeDay?.meals?.length ?? 0} posiłków
              </Text>
              <Pressable style={styles.addDayInlineButton} onPress={openDayModal}>
                <Icon name="plus" size={12} color="#2563eb" />
                <Text style={styles.addDayInlineText}>Dodaj dzień</Text>
              </Pressable>
            </View>
            <Pressable style={styles.dayChangeButton} onPress={() => changeDay(1)}>
              <Icon name="chevron-right" size={18} color={colors.mutedForeground} />
            </Pressable>
          </View>

          <View style={styles.statsRow}>
            <View style={[styles.statBox, styles.statBoxPrimary]}>
              <View style={styles.statHeader}>
                <Icon name="target" size={16} color="#fff" />
                <Text style={styles.statLabelPrimary}>Spożyte</Text>
              </View>
              <Text style={styles.statValuePrimary}>{consumedCalories}</Text>
              <Text style={styles.statUnitPrimary}>kcal</Text>
            </View>
            <View style={[styles.statBox, styles.statBoxSecondary]}>
              <View style={styles.statHeader}>
                <Icon name="pie-chart" size={16} color={Colors.primary} />
                <Text style={styles.statLabelSecondary}>Pozostało</Text>
              </View>
              <Text style={styles.statValueSecondary}>{remainingCalories}</Text>
              <Text style={styles.statUnitSecondary}>kcal z {targetCalories}</Text>
            </View>
          </View>

          <View style={[styles.macroSummaryCard, { backgroundColor: colors.card, borderColor: colors.border }]}>
            <View style={styles.macroItem}>
              <Text style={[styles.macroLabel, { color: colors.mutedForeground }]}>Białko</Text>
              <Text style={[styles.macroValue, { color: colors.foreground }]}>{consumedProtein}g / {targetMacros.protein}g</Text>
            </View>
            <View style={styles.macroItem}>
              <Text style={[styles.macroLabel, { color: colors.mutedForeground }]}>Węglowodany</Text>
              <Text style={[styles.macroValue, { color: colors.foreground }]}>{consumedCarbs}g / {targetMacros.carbs}g</Text>
            </View>
            <View style={styles.macroItem}>
              <Text style={[styles.macroLabel, { color: colors.mutedForeground }]}>Tłuszcze</Text>
              <Text style={[styles.macroValue, { color: colors.foreground }]}>{consumedFats}g / {targetMacros.fats}g</Text>
            </View>
          </View>

          <View style={[styles.comparisonCard, { backgroundColor: colors.card, borderColor: colors.border }]}>
            <Text style={[styles.comparisonTitle, { color: colors.foreground }]}>Porównanie z założeniami</Text>
            {[
              { label: 'Kalorie', value: calorieComparison },
              { label: 'Białko', value: proteinComparison },
              { label: 'Węglowodany', value: carbsComparison },
              { label: 'Tłuszcze', value: fatsComparison },
            ].map(item => (
              <View key={item.label} style={styles.comparisonRow}>
                <Text style={[styles.comparisonLabel, { color: colors.mutedForeground }]}>{item.label}</Text>
                <View style={[styles.statusBadge, getStatusStyle(item.value.status)]}>
                  <Text style={styles.statusText}>{item.value.text}</Text>
                </View>
              </View>
            ))}
          </View>

          <View style={styles.sectionHeaderRow}>
            <Text style={[styles.sectionTitle, { color: colors.foreground }]}>Posiłki</Text>
            <View style={styles.sectionActions}>
              <Pressable
                style={[styles.actionButton, styles.actionButtonAdd]}
                onPress={() => setMealModalVisible(true)}
              >
                <Icon name="plus" size={20} color="#fff" />
              </Pressable>
            </View>
          </View>

          <View style={styles.mealsList}>
            {activeDay?.meals?.length ? (
              activeDay.meals.map(meal => (
                <View key={meal.id} style={[styles.mealCard, { backgroundColor: colors.card, borderColor: colors.border }]}>
                  <View style={styles.mealIconWrapper}>
                    <Icon name="coffee" size={20} color={Colors.primary} />
                  </View>
                  <View style={styles.mealInfo}>
                    <Text style={[styles.mealName, { color: colors.foreground }]}>{meal.name}</Text>
                    <Text style={[styles.mealDesc, { color: colors.mutedForeground }]}>B: {meal.protein}g | W: {meal.carbs}g | T: {meal.fats}g</Text>
                  </View>
                  <Text style={[styles.mealCalories, { color: colors.foreground }]}>{meal.totalCalories} kcal</Text>
                  <Pressable onPress={() => removeMeal(meal.id)} style={{ marginLeft: 12 }}>
                    <Icon name="trash-2" size={18} color="#ef4444" />
                  </Pressable>
                </View>
              ))
            ) : (
              <Text style={{ color: colors.mutedForeground, textAlign: 'center', marginTop: 20 }}>
                Brak posiłków w tym dniu. Dodaj pierwszy!
              </Text>
            )}
          </View>
        </ScrollView>
      </>
    );
  };

  return (
    <View style={[styles.container, { backgroundColor: colors.background }]}>
      <View style={[styles.header, { backgroundColor: colors.background, paddingTop: insets.top + Spacing.sm }]}>
        <BackButton onPress={() => navigation.navigate('TrainingHub')} style={backButtonSpacing} />
        <View style={styles.headerTop}>
          <Text style={[styles.title, { color: colors.foreground }]}>Dieta</Text>
          <Pressable style={[styles.editButton, { backgroundColor: colors.card, borderColor: colors.border }]} onPress={() => navigation.navigate('DietAssumptions')}>
            <Icon name="edit-2" size={14} color={colors.foreground} />
            <Text style={[styles.editButtonText, { color: colors.foreground }]}>Założenia</Text>
          </Pressable>
        </View>
        <Text style={[styles.subtitle, { color: colors.mutedForeground }]}>
          {new Date(selectedDate).toLocaleDateString('pl-PL', { month: 'long', year: 'numeric' })}
        </Text>
      </View>
      {renderContent()}

      <Modal visible={dayModalVisible} transparent animationType="slide" onRequestClose={() => setDayModalVisible(false)}>
        <KeyboardAvoidingView behavior={Platform.OS === 'ios' ? 'padding' : 'height'} style={styles.modalOverlay}>
          <Pressable style={styles.modalDismiss} onPress={() => setDayModalVisible(false)} />
          <View style={[styles.modalSheet, { backgroundColor: colors.card, paddingBottom: insets.bottom + Spacing.lg }]}>
            <Text style={[styles.modalTitle, { color: colors.foreground }]}>Dodaj dzień diety</Text>
            <Text style={[styles.inputLabel, { color: colors.mutedForeground }]}>Data (RRRR-MM-DD)</Text>
            <TextInput
              style={[styles.modalInput, { color: colors.foreground, backgroundColor: colors.background, borderColor: colors.border }]}
              value={newDayDate}
              onChangeText={setNewDayDate}
              placeholder="2026-05-16"
              placeholderTextColor={colors.mutedForeground}
            />
            <Text style={[styles.inputLabel, { color: colors.mutedForeground }]}>Notatki (opcjonalnie)</Text>
            <TextInput
              style={[styles.modalInput, styles.modalTextArea, { color: colors.foreground, backgroundColor: colors.background, borderColor: colors.border }]}
              value={newDayNotes}
              onChangeText={setNewDayNotes}
              placeholder="np. dzień treningowy"
              placeholderTextColor={colors.mutedForeground}
              multiline
            />
            <Pressable style={styles.modalPrimaryButton} onPress={handleCreateDay}>
              <Text style={styles.modalPrimaryText}>Zapisz dzień</Text>
            </Pressable>
          </View>
        </KeyboardAvoidingView>
      </Modal>

      <Modal visible={mealModalVisible} transparent animationType="slide" onRequestClose={() => setMealModalVisible(false)}>
        <KeyboardAvoidingView behavior={Platform.OS === 'ios' ? 'padding' : 'height'} style={styles.modalOverlay}>
          <Pressable style={styles.modalDismiss} onPress={() => setMealModalVisible(false)} />
          <View style={[styles.modalSheet, { backgroundColor: colors.card, paddingBottom: insets.bottom + Spacing.lg }]}>
            <Text style={[styles.modalTitle, { color: colors.foreground }]}>Dodaj posiłek</Text>
            <Text style={[styles.inputLabel, { color: colors.mutedForeground }]}>Nazwa</Text>
            <TextInput style={[styles.modalInput, { color: colors.foreground, backgroundColor: colors.background, borderColor: colors.border }]} value={mealName} onChangeText={setMealName} placeholder="np. Śniadanie" placeholderTextColor={colors.mutedForeground} />
            <View style={styles.modalGrid}>
              <View style={styles.modalGridItem}>
                <Text style={[styles.inputLabel, { color: colors.mutedForeground }]}>Kalorie</Text>
                <TextInput style={[styles.modalInput, { color: colors.foreground, backgroundColor: colors.background, borderColor: colors.border }]} value={mealCalories} onChangeText={setMealCalories} keyboardType="numeric" placeholder="400" placeholderTextColor={colors.mutedForeground} />
              </View>
              <View style={styles.modalGridItem}>
                <Text style={[styles.inputLabel, { color: colors.mutedForeground }]}>Białko</Text>
                <TextInput style={[styles.modalInput, { color: colors.foreground, backgroundColor: colors.background, borderColor: colors.border }]} value={mealProtein} onChangeText={setMealProtein} keyboardType="numeric" placeholder="20" placeholderTextColor={colors.mutedForeground} />
              </View>
            </View>
            <View style={styles.modalGrid}>
              <View style={styles.modalGridItem}>
                <Text style={[styles.inputLabel, { color: colors.mutedForeground }]}>Węglowodany</Text>
                <TextInput style={[styles.modalInput, { color: colors.foreground, backgroundColor: colors.background, borderColor: colors.border }]} value={mealCarbs} onChangeText={setMealCarbs} keyboardType="numeric" placeholder="40" placeholderTextColor={colors.mutedForeground} />
              </View>
              <View style={styles.modalGridItem}>
                <Text style={[styles.inputLabel, { color: colors.mutedForeground }]}>Tłuszcze</Text>
                <TextInput style={[styles.modalInput, { color: colors.foreground, backgroundColor: colors.background, borderColor: colors.border }]} value={mealFats} onChangeText={setMealFats} keyboardType="numeric" placeholder="15" placeholderTextColor={colors.mutedForeground} />
              </View>
            </View>
            <Pressable style={styles.modalPrimaryButton} onPress={handleAddMeal}>
              <Text style={styles.modalPrimaryText}>Dodaj posiłek</Text>
            </Pressable>
          </View>
        </KeyboardAvoidingView>
      </Modal>
    </View>
  );
}

