// src/screens/DietScreen.tsx
import React, { useState } from 'react';
import { View, Text, StyleSheet, ScrollView, Pressable } from 'react-native';
import Icon from 'react-native-vector-icons/Feather';
import { Colors, Spacing } from '../theme/theme';

const days = [
  { day: 'PON', dateStr: '6', active: false },
  { day: 'WTO', dateStr: '7', active: false },
  { day: 'ŚRO', dateStr: '8', active: false },
  { day: 'CZW', dateStr: '9', active: false },
  { day: 'PIĄ', dateStr: '10', active: true },
  { day: 'SOB', dateStr: '11', active: false },
  { day: 'NIE', dateStr: '12', active: false },
];

export default function DietScreen({ navigation }: any) {
  const [selectedDate, setSelectedDate] = useState('10');

  return (
    <View style={styles.container}>
      <View style={styles.header}>
        <Pressable onPress={() => navigation.goBack()} style={styles.backButton}>
          <Icon name="arrow-left" size={20} color={Colors.foreground} />
        </Pressable>
        <View style={styles.headerTop}>
          <Text style={styles.title}>Dieta</Text>
          <Pressable style={styles.editButton}>
            <Icon name="edit-2" size={14} color={Colors.foreground} />
            <Text style={styles.editButtonText}>Założenia</Text>
          </Pressable>
        </View>
        <Text style={styles.subtitle}>Kwiecień 2026</Text>

        <View style={styles.calendarRow}>
          <Pressable style={styles.navButton}>
            <Icon name="chevron-left" size={20} color={Colors.mutedForeground} />
          </Pressable>
          <View style={styles.daysRow}>
            {days.map((item, index) => {
              const isActive = item.dateStr === selectedDate;
              return (
                <Pressable
                  key={index}
                  onPress={() => setSelectedDate(item.dateStr)}
                  style={[styles.dayItem, isActive && styles.dayActive]}
                >
                  <Text style={[styles.dayName, isActive && styles.dayNameActive]}>{item.day}</Text>
                  <Text style={[styles.dayNumber, isActive && styles.dayNumberActive]}>{item.dateStr}</Text>
                </Pressable>
              );
            })}
          </View>
          <Pressable style={styles.navButton}>
            <Icon name="chevron-right" size={20} color={Colors.mutedForeground} />
          </Pressable>
        </View>
      </View>

      <ScrollView style={styles.content} contentContainerStyle={styles.scrollContent}>
        <View style={styles.statsRow}>
          <View style={[styles.statBox, styles.statBoxPrimary]}>
            <View style={styles.statHeader}>
              <Icon name="target" size={16} color="#fff" />
              <Text style={styles.statLabelPrimary}>Spożyte</Text>
            </View>
            <Text style={styles.statValuePrimary}>1,450</Text>
            <Text style={styles.statUnitPrimary}>kcal</Text>
          </View>
          <View style={[styles.statBox, styles.statBoxSecondary]}>
            <View style={styles.statHeader}>
              <Icon name="pie-chart" size={16} color={Colors.primary} />
              <Text style={styles.statLabelSecondary}>Pozostało</Text>
            </View>
            <Text style={styles.statValueSecondary}>890</Text>
            <Text style={styles.statUnitSecondary}>kcal z 2340</Text>
          </View>
        </View>

        <View style={styles.sectionHeaderRow}>
          <Text style={styles.sectionTitle}>Posiłki</Text>
          <View style={styles.sectionActions}>
            <Pressable style={[styles.actionButton, styles.actionButtonDelete]}>
              <Icon name="trash-2" size={20} color="#ef4444" />
            </Pressable>
            <Pressable style={[styles.actionButton, styles.actionButtonAdd]}>
              <Icon name="plus" size={20} color="#fff" />
            </Pressable>
          </View>
        </View>

        <View style={styles.mealsList}>
          <Pressable style={styles.mealCard}>
            <View style={styles.mealIconWrapper}>
              <Icon name="coffee" size={20} color={Colors.primary} />
            </View>
            <View style={styles.mealInfo}>
              <Text style={styles.mealName}>Śniadanie</Text>
              <Text style={styles.mealDesc}>Owsianka z orzechami</Text>
            </View>
            <Text style={styles.mealCalories}>450 kcal</Text>
          </Pressable>

          <Pressable style={styles.mealCard}>
            <View style={styles.mealIconWrapper}>
              <Icon name="shopping-bag" size={20} color={Colors.primary} />
            </View>
            <View style={styles.mealInfo}>
              <Text style={styles.mealName}>Obiad</Text>
              <Text style={styles.mealDesc}>Kurczak z ryżem</Text>
            </View>
            <Text style={styles.mealCalories}>620 kcal</Text>
          </Pressable>
        </View>
      </ScrollView>
    </View>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1, backgroundColor: Colors.background },
  header: { paddingHorizontal: Spacing.lg, paddingTop: Spacing.lg, paddingBottom: Spacing.md },
  backButton: { marginBottom: Spacing.md },
  headerTop: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center', marginBottom: 4 },
  title: { fontSize: 28, fontWeight: 'bold', color: Colors.foreground },
  editButton: { flexDirection: 'row', alignItems: 'center', backgroundColor: Colors.secondary, paddingHorizontal: 12, paddingVertical: 6, borderRadius: 8, borderWidth: 1, borderColor: Colors.border },
  editButtonText: { fontSize: 14, fontWeight: '500', marginLeft: 6, color: Colors.foreground },
  subtitle: { fontSize: 14, color: Colors.mutedForeground, marginBottom: Spacing.lg },
  calendarRow: { flexDirection: 'row', alignItems: 'center', justifyContent: 'space-between' },
  navButton: { width: 32, height: 32, borderRadius: 16, backgroundColor: Colors.secondary, alignItems: 'center', justifyContent: 'center', borderWidth: 1, borderColor: Colors.border },
  daysRow: { flexDirection: 'row', flex: 1, justifyContent: 'space-between', paddingHorizontal: Spacing.xs },
  dayItem: { width: 44, height: 56, borderRadius: 16, alignItems: 'center', justifyContent: 'center', backgroundColor: Colors.secondary, borderWidth: 1, borderColor: Colors.border },
  dayActive: { backgroundColor: Colors.primary, borderColor: Colors.primary },
  dayName: { fontSize: 10, fontWeight: '500', color: Colors.mutedForeground, marginBottom: 4 },
  dayNameActive: { color: 'rgba(255,255,255,0.8)' },
  dayNumber: { fontSize: 14, fontWeight: 'bold', color: Colors.foreground },
  dayNumberActive: { color: '#fff' },
  content: { flex: 1, paddingHorizontal: Spacing.lg },
  scrollContent: { paddingBottom: Spacing.xl },
  statsRow: { flexDirection: 'row', justifyContent: 'space-between', marginBottom: Spacing.lg },
  statBox: { flex: 1, padding: Spacing.md, borderRadius: 16, marginHorizontal: Spacing.xs },
  statBoxPrimary: { backgroundColor: '#2563eb' },
  statBoxSecondary: { backgroundColor: Colors.secondary, borderWidth: 1, borderColor: Colors.border },
  statHeader: { flexDirection: 'row', alignItems: 'center', marginBottom: Spacing.sm },
  statLabelPrimary: { fontSize: 14, color: 'rgba(255,255,255,0.9)', marginLeft: 6 },
  statLabelSecondary: { fontSize: 14, color: Colors.mutedForeground, marginLeft: 6 },
  statValuePrimary: { fontSize: 32, fontWeight: 'bold', color: '#fff' },
  statValueSecondary: { fontSize: 32, fontWeight: 'bold', color: Colors.foreground },
  statUnitPrimary: { fontSize: 12, color: 'rgba(255,255,255,0.75)', marginTop: 4 },
  statUnitSecondary: { fontSize: 12, color: Colors.mutedForeground, marginTop: 4 },
  sectionHeaderRow: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center', marginBottom: Spacing.md },
  sectionTitle: { fontSize: 18, fontWeight: '600', color: Colors.foreground },
  sectionActions: { flexDirection: 'row', alignItems: 'center' },
  actionButton: { width: 40, height: 40, borderRadius: 12, alignItems: 'center', justifyContent: 'center', marginLeft: Spacing.sm },
  actionButtonDelete: { backgroundColor: Colors.secondary, borderWidth: 1, borderColor: Colors.border },
  actionButtonAdd: { backgroundColor: '#3b82f6' },
  mealsList: { gap: Spacing.sm },
  mealCard: { backgroundColor: Colors.secondary, padding: Spacing.md, borderRadius: 16, flexDirection: 'row', alignItems: 'center', borderWidth: 1, borderColor: Colors.border },
  mealIconWrapper: { width: 40, height: 40, borderRadius: 20, backgroundColor: '#eff6ff', alignItems: 'center', justifyContent: 'center', marginRight: Spacing.md },
  mealInfo: { flex: 1 },
  mealName: { fontSize: 14, fontWeight: '500', color: Colors.foreground, marginBottom: 2 },
  mealDesc: { fontSize: 12, color: Colors.mutedForeground },
  mealCalories: { fontSize: 14, fontWeight: '600', color: Colors.foreground },
});
