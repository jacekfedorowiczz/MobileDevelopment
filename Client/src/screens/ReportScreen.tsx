// src/screens/ReportScreen.tsx
import React from 'react';
import { View, Text, StyleSheet } from 'react-native';
import { useSafeAreaInsets } from 'react-native-safe-area-context';
import Icon from 'react-native-vector-icons/Feather';
import { Spacing } from '../theme/theme';
import BackButton from '../components/BackButton';
import { useTheme } from '../context/ThemeContext';

export default function ReportScreen({ navigation }: any) {
  const insets = useSafeAreaInsets();
  const { colors } = useTheme();

  return (
    <View style={[styles.container, { backgroundColor: colors.background }]}>
      <View style={[styles.header, { paddingTop: insets.top + Spacing.sm }]}>
        <BackButton onPress={() => navigation.navigate('TrainingHub')} />
        <Text style={[styles.title, { color: colors.foreground }]}>Raport i Postępy</Text>
      </View>

      <View style={styles.content}>
        <View style={[styles.placeholderCard, { backgroundColor: colors.card, borderColor: colors.border }]}>
          <View style={styles.iconWrapper}>
            <Icon name="bar-chart-2" size={32} color="#2563eb" />
          </View>
          <Text style={[styles.cardTitle, { color: colors.foreground }]}>Moduł w przygotowaniu</Text>
          <Text style={[styles.cardDesc, { color: colors.mutedForeground }]}>Wkrótce pojawią się tutaj szczegółowe wykresy i statystyki dotyczące Twoich treningów i diety.</Text>
        </View>

        <View style={styles.grid}>
          <View style={[styles.gridItem, { backgroundColor: colors.card, borderColor: colors.border }]}>
            <Text style={[styles.itemLabel, { color: colors.mutedForeground }]}>Masa ciała</Text>
            <Text style={[styles.itemValue, { color: colors.foreground }]}>---</Text>
          </View>
          <View style={[styles.gridItem, { backgroundColor: colors.card, borderColor: colors.border }]}>
            <Text style={[styles.itemLabel, { color: colors.mutedForeground }]}>Tkanka tłuszczowa</Text>
            <Text style={[styles.itemValue, { color: colors.foreground }]}>---</Text>
          </View>
        </View>
      </View>
    </View>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1 },
  header: { flexDirection: 'row', alignItems: 'center', gap: Spacing.md, paddingHorizontal: Spacing.lg, paddingTop: Spacing.xl, paddingBottom: Spacing.md },
  title: { fontSize: 24, fontWeight: 'bold' },
  content: { flex: 1, paddingHorizontal: Spacing.lg },
  placeholderCard: { borderRadius: 24, padding: Spacing.xl, alignItems: 'center', justifyContent: 'center', borderWidth: 1, marginBottom: Spacing.lg },
  iconWrapper: { width: 64, height: 64, borderRadius: 16, backgroundColor: 'rgba(37, 99, 235, 0.1)', alignItems: 'center', justifyContent: 'center', marginBottom: Spacing.md },
  cardTitle: { fontSize: 18, fontWeight: '600', marginBottom: 8 },
  cardDesc: { fontSize: 14, textAlign: 'center', lineHeight: 20, paddingHorizontal: Spacing.md },
  grid: { flexDirection: 'row', gap: Spacing.sm },
  gridItem: { flex: 1, padding: Spacing.md, borderRadius: 16, borderWidth: 1, opacity: 0.6 },
  itemLabel: { fontSize: 14, marginBottom: 4 },
  itemValue: { fontSize: 24, fontWeight: '600' },
});
