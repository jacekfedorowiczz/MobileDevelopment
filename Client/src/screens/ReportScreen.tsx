// src/screens/ReportScreen.tsx
import React from 'react';
import { View, Text, StyleSheet, Pressable } from 'react-native';
import Icon from 'react-native-vector-icons/Feather';
import { Colors, Spacing } from '../theme/theme';

export default function ReportScreen({ navigation }: any) {
  return (
    <View style={styles.container}>
      <View style={styles.header}>
        <Pressable onPress={() => navigation.goBack()} style={styles.backButton}>
          <Icon name="arrow-left" size={20} color={Colors.foreground} />
        </Pressable>
        <Text style={styles.title}>Raport i Postępy</Text>
      </View>

      <View style={styles.content}>
        <View style={styles.placeholderCard}>
          <View style={styles.iconWrapper}>
            <Icon name="bar-chart-2" size={32} color="#2563eb" />
          </View>
          <Text style={styles.cardTitle}>Moduł w przygotowaniu</Text>
          <Text style={styles.cardDesc}>Wkrótce pojawią się tutaj szczegółowe wykresy i statystyki dotyczące Twoich treningów i diety.</Text>
        </View>

        <View style={styles.grid}>
          <View style={styles.gridItem}>
            <Text style={styles.itemLabel}>Masa ciała</Text>
            <Text style={styles.itemValue}>---</Text>
          </View>
          <View style={styles.gridItem}>
            <Text style={styles.itemLabel}>Tkanka tłuszczowa</Text>
            <Text style={styles.itemValue}>---</Text>
          </View>
        </View>
      </View>
    </View>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1, backgroundColor: Colors.background },
  header: { flexDirection: 'row', alignItems: 'center', paddingHorizontal: Spacing.lg, paddingTop: Spacing.xl, paddingBottom: Spacing.md },
  backButton: { width: 40, height: 40, borderRadius: 20, backgroundColor: Colors.secondary, alignItems: 'center', justifyContent: 'center', borderWidth: 1, borderColor: Colors.border, marginRight: Spacing.md },
  title: { fontSize: 24, fontWeight: 'bold', color: Colors.foreground },
  content: { flex: 1, paddingHorizontal: Spacing.lg },
  placeholderCard: { backgroundColor: 'rgba(255,255,255,0.05)', borderRadius: 24, padding: Spacing.xl, alignItems: 'center', justifyContent: 'center', borderWidth: 1, borderColor: Colors.border, marginBottom: Spacing.lg },
  iconWrapper: { width: 64, height: 64, borderRadius: 16, backgroundColor: 'rgba(37, 99, 235, 0.1)', alignItems: 'center', justifyContent: 'center', marginBottom: Spacing.md },
  cardTitle: { fontSize: 18, fontWeight: '600', color: Colors.foreground, marginBottom: 8 },
  cardDesc: { fontSize: 14, color: Colors.mutedForeground, textAlign: 'center', lineHeight: 20, paddingHorizontal: Spacing.md },
  grid: { flexDirection: 'row', gap: Spacing.sm },
  gridItem: { flex: 1, backgroundColor: Colors.secondary, padding: Spacing.md, borderRadius: 16, borderWidth: 1, borderColor: Colors.border, opacity: 0.6 },
  itemLabel: { fontSize: 14, color: Colors.mutedForeground, marginBottom: 4 },
  itemValue: { fontSize: 24, fontWeight: '600', color: Colors.foreground },
});
