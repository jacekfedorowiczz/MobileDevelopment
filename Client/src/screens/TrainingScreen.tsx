// src/screens/TrainingScreen.tsx
import React from 'react';
import { View, Text, StyleSheet, ScrollView, Pressable } from 'react-native';
import Icon from 'react-native-vector-icons/Feather';
import { Colors, Spacing } from '../theme/theme';

const cards = [
  { title: "Treningi", description: "Twój dziennik treningowy", icon: "activity", path: "WorkoutLog" },
  { title: "Ćwiczenia", description: "Baza ćwiczeń i atlas", icon: "clipboard", path: "Exercises" },
  { title: "Dieta", description: "Plany żywieniowe i posiłki", icon: "coffee", path: "Diet" },
  { title: "Kalkulatory", description: "BMI, zapotrzebowanie kaloryczne", icon: "monitor", path: "Tools" },
  { title: "Siłownie", description: "Znajdź najbliższe obiekty", icon: "map-pin", path: "Gyms" },
];

export default function TrainingScreen({ navigation }: any) {
  return (
    <View style={styles.container}>
      <View style={styles.header}>
        <Text style={styles.title}>Trening</Text>
        <Text style={styles.subtitle}>Wybierz moduł, z którego chcesz skorzystać</Text>
      </View>

      <ScrollView style={styles.content} contentContainerStyle={styles.scrollContent}>
        {cards.map((card, index) => (
          <Pressable
            key={index}
            style={styles.card}
            onPress={() => navigation.navigate(card.path)}
          >
            <View style={styles.iconContainer}>
              <Icon name={card.icon} size={28} color="#ffffff" />
            </View>
            <View style={styles.cardTextContainer}>
              <Text style={styles.cardTitle}>{card.title}</Text>
              <Text style={styles.cardDescription}>{card.description}</Text>
            </View>
            <View style={styles.arrowContainer}>
              <Icon name="chevron-right" size={20} color={Colors.mutedForeground} />
            </View>
          </Pressable>
        ))}
      </ScrollView>
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: Colors.background,
  },
  header: {
    paddingHorizontal: Spacing.lg,
    paddingTop: Spacing.xl,
    paddingBottom: Spacing.md,
  },
  title: {
    fontSize: 32,
    fontWeight: 'bold',
    color: '#1e40af', // from-blue-600 to-blue-800
  },
  subtitle: {
    fontSize: 14,
    color: Colors.mutedForeground,
    marginTop: 4,
  },
  content: {
    flex: 1,
    paddingHorizontal: Spacing.md,
  },
  scrollContent: {
    paddingBottom: Spacing.xl,
  },
  card: {
    flexDirection: 'row',
    alignItems: 'center',
    backgroundColor: Colors.card,
    borderRadius: 24,
    padding: Spacing.md,
    marginBottom: Spacing.md,
    borderWidth: 1,
    borderColor: Colors.border,
    shadowColor: '#000',
    shadowOpacity: 0.05,
    shadowRadius: 10,
    elevation: 2,
  },
  iconContainer: {
    width: 56,
    height: 56,
    borderRadius: 16,
    backgroundColor: Colors.primary, // Could be gradients per card if we add react-native-linear-gradient
    alignItems: 'center',
    justifyContent: 'center',
    marginRight: Spacing.md,
  },
  cardTextContainer: {
    flex: 1,
  },
  cardTitle: {
    fontSize: 18,
    fontWeight: 'bold',
    color: Colors.foreground,
    marginBottom: 4,
  },
  cardDescription: {
    fontSize: 14,
    color: Colors.mutedForeground,
  },
  arrowContainer: {
    width: 40,
    height: 40,
    borderRadius: 20,
    backgroundColor: Colors.muted,
    alignItems: 'center',
    justifyContent: 'center',
  },
});
