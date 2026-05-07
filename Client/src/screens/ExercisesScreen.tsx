// src/screens/ExercisesScreen.tsx
import React, { useState } from 'react';
import { View, Text, StyleSheet, ScrollView, TextInput, Image, Pressable, Modal } from 'react-native';
import Icon from 'react-native-vector-icons/Feather';
import { Colors, Spacing } from '../theme/theme';

interface Exercise {
  id: string;
  name: string;
  category: string;
  muscleGroups: string[];
  difficulty: "Początkujący" | "Średniozaawansowany" | "Zaawansowany";
  image: string;
  isCustom: boolean;
}

const systemExercises: Exercise[] = [
  {
    id: "1", name: "Przysiady ze sztangą", category: "Nogi", muscleGroups: ["Czworogłowy", "Pośladki", "Łydki"], difficulty: "Średniozaawansowany",
    image: "https://images.unsplash.com/photo-1585484764802-387ea30e8432?crop=entropy&cs=tinysrgb&fit=max&fm=jpg&q=80&w=400", isCustom: false,
  },
  {
    id: "2", name: "Wyciskanie sztangi na ławce płaskiej", category: "Klatka piersiowa", muscleGroups: ["Klatka piersiowa", "Triceps", "Barki"], difficulty: "Średniozaawansowany",
    image: "https://images.unsplash.com/photo-1652363722833-509b3aac287b?crop=entropy&cs=tinysrgb&fit=max&fm=jpg&q=80&w=400", isCustom: false,
  },
  {
    id: "3", name: "Martwy ciąg", category: "Plecy", muscleGroups: ["Plecy", "Pośladki", "Nogi"], difficulty: "Zaawansowany",
    image: "https://images.unsplash.com/photo-1606657830879-00d70555b1b6?crop=entropy&cs=tinysrgb&fit=max&fm=jpg&q=80&w=400", isCustom: false,
  },
  {
    id: "4", name: "Podciąganie na drążku", category: "Plecy", muscleGroups: ["Plecy", "Biceps"], difficulty: "Średniozaawansowany",
    image: "https://images.unsplash.com/photo-1677165733273-dcc3724c00e8?crop=entropy&cs=tinysrgb&fit=max&fm=jpg&q=80&w=400", isCustom: false,
  },
  {
    id: "5", name: "Wyciskanie sztangi nad głowę", category: "Barki", muscleGroups: ["Barki", "Triceps"], difficulty: "Średniozaawansowany",
    image: "https://images.unsplash.com/photo-1694856677238-07fe09f6e31a?crop=entropy&cs=tinysrgb&fit=max&fm=jpg&q=80&w=400", isCustom: false,
  },
];

const getDifficultyStyle = (difficulty: string) => {
  switch (difficulty) {
    case "Początkujący": return { color: '#16a34a', backgroundColor: '#f0fdf4' };
    case "Średniozaawansowany": return { color: '#d97706', backgroundColor: '#fffbeb' };
    case "Zaawansowany": return { color: '#dc2626', backgroundColor: '#fef2f2' };
    default: return { color: Colors.foreground, backgroundColor: Colors.secondary };
  }
};

export default function ExercisesScreen({ navigation }: any) {
  const [searchQuery, setSearchQuery] = useState("");
  const [showFilterDialog, setShowFilterDialog] = useState(false);
  const [selectedGroups, setSelectedGroups] = useState<string[]>([]);
  const [selectedDifficulties, setSelectedDifficulties] = useState<string[]>([]);

  const toggleGroup = (m: string) => setSelectedGroups(prev => prev.includes(m) ? prev.filter(g => g !== m) : [...prev, m]);
  const toggleDifficulty = (d: string) => setSelectedDifficulties(prev => prev.includes(d) ? prev.filter(x => x !== d) : [...prev, d]);

  const filteredExercises = systemExercises.filter(ex => {
    const matchesSearch = ex.name.toLowerCase().includes(searchQuery.toLowerCase());
    const matchesGroups = selectedGroups.length === 0 || selectedGroups.some(g => ex.category === g || ex.muscleGroups.includes(g));
    const matchesDiff = selectedDifficulties.length === 0 || selectedDifficulties.includes(ex.difficulty);
    return matchesSearch && matchesGroups && matchesDiff;
  });

  return (
    <View style={styles.container}>
      <View style={styles.header}>
        <Pressable onPress={() => navigation.goBack()} style={styles.backButton}>
          <Icon name="arrow-left" size={20} color={Colors.foreground} />
        </Pressable>
        <View style={styles.headerTop}>
          <View>
            <Text style={styles.title}>Ćwiczenia</Text>
            <Text style={styles.subtitle}>{filteredExercises.length} dostępnych ćwiczeń</Text>
          </View>
          <View style={styles.headerActions}>
            <Pressable style={styles.iconButton} onPress={() => setShowFilterDialog(true)}>
              <Icon name="filter" size={20} color={Colors.foreground} />
            </Pressable>
            <Pressable style={[styles.iconButton, styles.iconButtonDestructive]}>
              <Icon name="trash-2" size={20} color="#ef4444" />
            </Pressable>
            <Pressable style={[styles.iconButton, styles.iconButtonPrimary]} onPress={() => navigation.navigate('ExerciseCreate')}>
              <Icon name="plus" size={20} color="#fff" />
            </Pressable>
          </View>
        </View>

        <View style={styles.searchContainer}>
          <Icon name="search" size={20} color={Colors.mutedForeground} style={styles.searchIcon} />
          <TextInput
            style={styles.searchInput}
            placeholder="Szukaj ćwiczenia..."
            value={searchQuery}
            onChangeText={setSearchQuery}
            placeholderTextColor={Colors.mutedForeground}
          />
        </View>
      </View>

      <ScrollView style={styles.content} contentContainerStyle={styles.scrollContent}>
        {filteredExercises.map(ex => {
          const diffStyle = getDifficultyStyle(ex.difficulty);
          return (
            <Pressable key={ex.id} style={styles.exerciseCard} onPress={() => navigation.navigate('ExerciseDetail', { id: ex.id })}>
              <Image source={{ uri: ex.image }} style={styles.exerciseImage} />
              <View style={styles.exerciseInfo}>
                <View style={styles.exerciseHeader}>
                  <Text style={styles.exerciseName} numberOfLines={1}>{ex.name}</Text>
                  {!ex.isCustom && <Icon name="star" size={14} color={Colors.mutedForeground} />}
                </View>
                <Text style={styles.exerciseCategory}>{ex.category}</Text>
                
                <View style={styles.tagsContainer}>
                  <Text style={[styles.tag, diffStyle]}>{ex.difficulty}</Text>
                  {ex.muscleGroups.slice(0, 2).map((m, i) => (
                    <Text key={i} style={styles.muscleTag}>{m}</Text>
                  ))}
                  {ex.muscleGroups.length > 2 && (
                    <Text style={styles.muscleMoreTag}>+{ex.muscleGroups.length - 2}</Text>
                  )}
                </View>
              </View>
            </Pressable>
          );
        })}

        {filteredExercises.length === 0 && (
          <View style={styles.emptyState}>
            <Text style={styles.emptyStateTitle}>Nie znaleziono ćwiczeń</Text>
            <Text style={styles.emptyStateDesc}>Spróbuj zmienić filtr lub dodaj własne ćwiczenie</Text>
          </View>
        )}
      </ScrollView>

      <Modal visible={showFilterDialog} transparent animationType="fade">
        <View style={styles.modalOverlay}>
          <View style={styles.modalContent}>
            <View style={styles.modalHeader}>
              <Text style={styles.modalTitle}>Filtruj wyniki</Text>
              <Pressable onPress={() => setShowFilterDialog(false)} style={styles.closeButton}>
                <Icon name="x" size={20} color={Colors.foreground} />
              </Pressable>
            </View>

            <Text style={styles.filterSectionTitle}>Grupa mięśniowa</Text>
            <View style={styles.filterChips}>
              {["Klatka piersiowa", "Plecy", "Nogi", "Barki", "Ramiona", "Biceps", "Triceps", "Brzuch"].map(m => {
                const isActive = selectedGroups.includes(m);
                return (
                  <Pressable key={m} style={[styles.chip, isActive && styles.chipActive]} onPress={() => toggleGroup(m)}>
                    <Text style={[styles.chipText, isActive && styles.chipTextActive]}>{m}</Text>
                  </Pressable>
                );
              })}
            </View>

            <Text style={styles.filterSectionTitle}>Poziom trudności</Text>
            <View style={styles.filterChips}>
              {["Początkujący", "Średniozaawansowany", "Zaawansowany"].map(lvl => {
                const isActive = selectedDifficulties.includes(lvl);
                return (
                  <Pressable key={lvl} style={[styles.chip, isActive && styles.chipActive]} onPress={() => toggleDifficulty(lvl)}>
                    <Text style={[styles.chipText, isActive && styles.chipTextActive]}>{lvl}</Text>
                  </Pressable>
                );
              })}
            </View>

            <Pressable style={styles.applyButton} onPress={() => setShowFilterDialog(false)}>
              <Text style={styles.applyButtonText}>Zastosuj filtry</Text>
            </Pressable>
          </View>
        </View>
      </Modal>
    </View>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1, backgroundColor: Colors.background },
  header: { paddingHorizontal: Spacing.lg, paddingTop: Spacing.lg, paddingBottom: Spacing.md },
  backButton: { marginBottom: Spacing.md },
  headerTop: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center', marginBottom: Spacing.lg },
  title: { fontSize: 28, fontWeight: 'bold', color: Colors.foreground },
  subtitle: { fontSize: 14, color: Colors.mutedForeground, marginTop: 2 },
  headerActions: { flexDirection: 'row', gap: Spacing.xs },
  iconButton: { width: 40, height: 40, borderRadius: 12, backgroundColor: Colors.secondary, alignItems: 'center', justifyContent: 'center', borderWidth: 1, borderColor: Colors.border, marginLeft: Spacing.xs },
  iconButtonDestructive: { borderColor: 'rgba(239, 68, 68, 0.3)' },
  iconButtonPrimary: { backgroundColor: '#3b82f6', borderColor: '#3b82f6' },
  searchContainer: { flexDirection: 'row', alignItems: 'center', backgroundColor: Colors.secondary, borderRadius: 12, borderWidth: 1, borderColor: Colors.border, paddingHorizontal: Spacing.md, height: 48 },
  searchIcon: { marginRight: Spacing.sm },
  searchInput: { flex: 1, fontSize: 16, color: Colors.foreground },
  content: { flex: 1, paddingHorizontal: Spacing.lg },
  scrollContent: { paddingBottom: Spacing.xl },
  exerciseCard: { flexDirection: 'row', backgroundColor: Colors.secondary, borderRadius: 16, overflow: 'hidden', marginBottom: Spacing.sm, borderWidth: 1, borderColor: Colors.border },
  exerciseImage: { width: 96, height: 96 },
  exerciseInfo: { flex: 1, padding: Spacing.sm, justifyContent: 'space-between' },
  exerciseHeader: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'flex-start' },
  exerciseName: { fontSize: 14, fontWeight: '600', color: Colors.foreground, flex: 1, marginRight: Spacing.sm },
  exerciseCategory: { fontSize: 12, color: Colors.mutedForeground, marginTop: 2 },
  tagsContainer: { flexDirection: 'row', flexWrap: 'wrap', gap: 4, marginTop: Spacing.sm, borderTopWidth: 1, borderColor: 'rgba(0,0,0,0.05)', paddingTop: Spacing.sm },
  tag: { fontSize: 10, paddingHorizontal: 6, paddingVertical: 2, borderRadius: 6, overflow: 'hidden', fontWeight: '500' },
  muscleTag: { fontSize: 10, paddingHorizontal: 6, paddingVertical: 2, borderRadius: 6, backgroundColor: Colors.background, borderWidth: 1, borderColor: Colors.border, color: Colors.foreground, overflow: 'hidden' },
  muscleMoreTag: { fontSize: 10, paddingHorizontal: 6, paddingVertical: 2, color: Colors.mutedForeground },
  emptyState: { paddingVertical: Spacing.xl * 2, alignItems: 'center' },
  emptyStateTitle: { fontSize: 16, color: Colors.mutedForeground, marginBottom: Spacing.xs },
  emptyStateDesc: { fontSize: 14, color: Colors.mutedForeground, textAlign: 'center' },
  modalOverlay: { flex: 1, backgroundColor: 'rgba(0,0,0,0.5)', justifyContent: 'center', padding: Spacing.lg },
  modalContent: { backgroundColor: Colors.background, borderRadius: 24, padding: Spacing.lg },
  modalHeader: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center', marginBottom: Spacing.lg },
  modalTitle: { fontSize: 20, fontWeight: '600', color: Colors.foreground },
  closeButton: { width: 32, height: 32, borderRadius: 16, alignItems: 'center', justifyContent: 'center', backgroundColor: Colors.secondary },
  filterSectionTitle: { fontSize: 14, fontWeight: '500', color: Colors.foreground, marginBottom: Spacing.sm },
  filterChips: { flexDirection: 'row', flexWrap: 'wrap', gap: Spacing.sm, marginBottom: Spacing.lg },
  chip: { paddingHorizontal: 12, paddingVertical: 6, borderRadius: 12, backgroundColor: Colors.secondary, borderWidth: 1, borderColor: Colors.border },
  chipActive: { backgroundColor: '#2563eb', borderColor: '#2563eb' },
  chipText: { fontSize: 14, color: Colors.foreground },
  chipTextActive: { color: '#fff' },
  applyButton: { backgroundColor: '#2563eb', height: 48, borderRadius: 12, alignItems: 'center', justifyContent: 'center', marginTop: Spacing.sm },
  applyButtonText: { color: '#fff', fontSize: 16, fontWeight: '500' },
});
