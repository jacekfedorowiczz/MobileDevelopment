import React from 'react';
import { View, Text, StyleSheet, ScrollView, Pressable } from 'react-native';
import { useSafeAreaInsets } from 'react-native-safe-area-context';
import Icon from 'react-native-vector-icons/Feather';
import { Spacing } from '../theme/theme';
import { useTheme } from '../context/ThemeContext';
import { useWorkoutStore } from '../store/useWorkoutStore';
import BackButton, { backButtonSpacing } from '../components/BackButton';

const cards = [
  { title: "Treningi", description: "Twój dziennik treningowy", icon: "activity", path: "WorkoutLog", color: "#2563eb" },
  { title: "Ćwiczenia", description: "Baza ćwiczeń i atlas", icon: "clipboard", path: "Exercises", color: "#7c3aed" },
  { title: "Dieta", description: "Plany żywieniowe i posiłki", icon: "coffee", path: "Diet", color: "#059669" },
  { title: "Kalkulatory", description: "BMI, zapotrzebowanie kaloryczne", icon: "monitor", path: "Tools", color: "#ea580c" },
  { title: "Siłownie", description: "Znajdź najbliższe obiekty", icon: "map-pin", path: "Gyms", color: "#db2777" },
];

export default function TrainingScreen({ navigation }: any) {
  const { colors, isDark } = useTheme();
  const { isTraining, activeSession } = useWorkoutStore();
  const insets = useSafeAreaInsets();

  return (
    <View style={[styles.container, { backgroundColor: colors.background }]}>
      <View style={[styles.header, { paddingTop: insets.top + Spacing.md }]}>
        <BackButton onPress={() => navigation.getParent()?.navigate('Dashboard')} style={backButtonSpacing} />
        <Text style={[styles.title, { color: isDark ? '#60a5fa' : '#1e40af' }]}>Trening</Text>
        <Text style={[styles.subtitle, { color: colors.mutedForeground }]}>Wybierz moduł, z którego chcesz skorzystać</Text>
      </View>

      {isTraining && activeSession && (
        <Pressable
          style={styles.activeWorkoutBanner}
          onPress={() => navigation.navigate('WorkoutLog')}
        >
          <View style={styles.activeIconWrapper}>
            <Icon name="play" size={16} color="#fff" />
          </View>
          <View style={{ flex: 1 }}>
            <Text style={styles.activeTitle}>Aktywny trening: {activeSession.name}</Text>
            <Text style={styles.activeSubtitle}>Kliknij, aby wrócić do wpisywania serii</Text>
          </View>
          <Icon name="chevron-right" size={20} color="#fff" />
        </Pressable>
      )}

      <ScrollView style={styles.content} contentContainerStyle={styles.scrollContent}>
        {cards.map((card, index) => (
          <Pressable
            key={index}
            style={[styles.card, { backgroundColor: colors.card, borderColor: colors.border }]}
            onPress={() => navigation.navigate(card.path)}
          >
            <View style={[styles.iconContainer, { backgroundColor: card.color }]}>
              <Icon name={card.icon} size={28} color="#ffffff" />
            </View>
            <View style={styles.cardTextContainer}>
              <Text style={[styles.cardTitle, { color: colors.foreground }]}>{card.title}</Text>
              <Text style={[styles.cardDescription, { color: colors.mutedForeground }]}>{card.description}</Text>
            </View>
            <View style={[styles.arrowContainer, { backgroundColor: colors.muted }]}>
              <Icon name="chevron-right" size={20} color={colors.mutedForeground} />
            </View>
          </Pressable>
        ))}
      </ScrollView>
    </View>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1 },
  header: {
    paddingHorizontal: Spacing.lg,
    paddingTop: Spacing.xl,
    paddingBottom: Spacing.md,
  },
  title: { fontSize: 32, fontWeight: 'bold' },
  subtitle: { fontSize: 14, marginTop: 4 },
  activeWorkoutBanner: {
    flexDirection: 'row',
    alignItems: 'center',
    backgroundColor: '#2563eb',
    marginHorizontal: Spacing.md,
    marginBottom: Spacing.md,
    padding: Spacing.md,
    borderRadius: 16,
    shadowColor: '#2563eb',
    shadowOpacity: 0.3,
    shadowRadius: 10,
    elevation: 5,
  },
  activeIconWrapper: {
    width: 32,
    height: 32,
    borderRadius: 16,
    backgroundColor: 'rgba(255,255,255,0.2)',
    alignItems: 'center',
    justifyContent: 'center',
    marginRight: Spacing.md,
  },
  activeTitle: { color: '#fff', fontWeight: 'bold', fontSize: 14 },
  activeSubtitle: { color: 'rgba(255,255,255,0.8)', fontSize: 12 },
  content: { flex: 1, paddingHorizontal: Spacing.md },
  scrollContent: { paddingBottom: Spacing.xl },
  card: {
    flexDirection: 'row',
    alignItems: 'center',
    borderRadius: 24,
    padding: Spacing.md,
    marginBottom: Spacing.md,
    borderWidth: 1,
    shadowColor: '#000',
    shadowOpacity: 0.05,
    shadowRadius: 10,
    elevation: 2,
  },
  iconContainer: {
    width: 56,
    height: 56,
    borderRadius: 16,
    alignItems: 'center',
    justifyContent: 'center',
    marginRight: Spacing.md,
  },
  cardTextContainer: { flex: 1 },
  cardTitle: { fontSize: 18, fontWeight: 'bold', marginBottom: 4 },
  cardDescription: { fontSize: 14 },
  arrowContainer: {
    width: 40,
    height: 40,
    borderRadius: 20,
    alignItems: 'center',
    justifyContent: 'center',
  },
});

