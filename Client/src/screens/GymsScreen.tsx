// src/screens/GymsScreen.tsx
import React, { useState } from 'react';
import { View, Text, StyleSheet, ScrollView, TextInput, Pressable } from 'react-native';
import Icon from 'react-native-vector-icons/Feather';
import { Colors, Spacing } from '../theme/theme';

const gyms = [
  { name: "FitGym Centrum", address: "ul. Główna 12, Warszawa", dist: "1.2 km", rating: "4.8" },
  { name: "PowerHouse", address: "ul. Sportowa 5, Warszawa", dist: "2.5 km", rating: "4.5" },
  { name: "Iron & Steel Club", address: "ul. Przemysłowa 9, Warszawa", dist: "3.1 km", rating: "4.7" },
  { name: "Calisthenics Park", address: "Park Miejski", dist: "0.8 km", rating: "4.9" },
];

export default function GymsScreen({ navigation }: any) {
  const [searchQuery, setSearchQuery] = useState("");

  const filteredGyms = gyms.filter(gym => 
    gym.name.toLowerCase().includes(searchQuery.toLowerCase()) || 
    gym.address.toLowerCase().includes(searchQuery.toLowerCase())
  );

  return (
    <View style={styles.container}>
      <View style={styles.header}>
        <Pressable onPress={() => navigation.goBack()} style={styles.backButton}>
          <Icon name="arrow-left" size={20} color={Colors.foreground} />
        </Pressable>
        <Text style={styles.title}>Siłownie w okolicy</Text>
        <View style={styles.locationRow}>
          <Icon name="map-pin" size={16} color={Colors.primary} />
          <Text style={styles.locationText}>Twoja lokalizacja: Warszawa Centrum</Text>
        </View>

        <View style={styles.searchContainer}>
          <Icon name="search" size={20} color={Colors.mutedForeground} style={styles.searchIcon} />
          <TextInput
            style={styles.searchInput}
            placeholder="Wyszukaj klub albo park..."
            value={searchQuery}
            onChangeText={setSearchQuery}
            placeholderTextColor={Colors.mutedForeground}
          />
        </View>
      </View>

      <ScrollView style={styles.content} contentContainerStyle={styles.scrollContent}>
        {filteredGyms.map((gym, idx) => (
          <Pressable key={idx} style={styles.gymCard}>
            <View style={styles.gymHeader}>
              <View style={styles.gymInfo}>
                <Text style={styles.gymName}>{gym.name}</Text>
                <View style={styles.addressRow}>
                  <Icon name="map-pin" size={12} color={Colors.mutedForeground} />
                  <Text style={styles.addressText}>{gym.address}</Text>
                </View>
              </View>
              <View style={styles.ratingBadge}>
                <Icon name="star" size={12} color="#f59e0b" />
                <Text style={styles.ratingText}>{gym.rating}</Text>
              </View>
            </View>

            <View style={styles.gymFooter}>
              <View style={styles.distBadge}>
                <Text style={styles.distText}>{gym.dist} stąd</Text>
              </View>
              <Pressable style={styles.navButton}>
                <Icon name="navigation" size={14} color={Colors.background} />
                <Text style={styles.navButtonText}>Nawiguj</Text>
              </Pressable>
            </View>
          </Pressable>
        ))}
      </ScrollView>
    </View>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1, backgroundColor: Colors.background },
  header: { paddingHorizontal: Spacing.lg, paddingTop: Spacing.lg, paddingBottom: Spacing.md },
  backButton: { marginBottom: Spacing.md },
  title: { fontSize: 28, fontWeight: 'bold', color: Colors.foreground },
  locationRow: { flexDirection: 'row', alignItems: 'center', marginTop: 8, marginBottom: Spacing.lg },
  locationText: { fontSize: 14, color: Colors.mutedForeground, marginLeft: 6 },
  searchContainer: { flexDirection: 'row', alignItems: 'center', backgroundColor: Colors.secondary, borderRadius: 12, borderWidth: 1, borderColor: Colors.border, paddingHorizontal: Spacing.md, height: 48 },
  searchIcon: { marginRight: Spacing.sm },
  searchInput: { flex: 1, fontSize: 16, color: Colors.foreground },
  content: { flex: 1, paddingHorizontal: Spacing.lg },
  scrollContent: { paddingBottom: Spacing.xl },
  gymCard: { backgroundColor: Colors.secondary, padding: Spacing.md, borderRadius: 16, marginBottom: Spacing.md, borderWidth: 1, borderColor: Colors.border },
  gymHeader: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'flex-start', marginBottom: Spacing.md },
  gymInfo: { flex: 1, paddingRight: Spacing.sm },
  gymName: { fontSize: 16, fontWeight: '600', color: Colors.foreground, marginBottom: 4 },
  addressRow: { flexDirection: 'row', alignItems: 'center' },
  addressText: { fontSize: 12, color: Colors.mutedForeground, marginLeft: 4 },
  ratingBadge: { flexDirection: 'row', alignItems: 'center', backgroundColor: Colors.background, paddingHorizontal: 8, paddingVertical: 4, borderRadius: 8, borderWidth: 1, borderColor: Colors.border },
  ratingText: { fontSize: 12, fontWeight: '600', color: Colors.foreground, marginLeft: 4 },
  gymFooter: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center', paddingTop: Spacing.md, borderTopWidth: 1, borderColor: Colors.border },
  distBadge: { backgroundColor: 'rgba(37, 99, 235, 0.1)', paddingHorizontal: 8, paddingVertical: 4, borderRadius: 6 },
  distText: { fontSize: 14, fontWeight: '500', color: '#2563eb' },
  navButton: { flexDirection: 'row', alignItems: 'center', backgroundColor: Colors.foreground, paddingHorizontal: 16, paddingVertical: 8, borderRadius: 12 },
  navButtonText: { fontSize: 12, fontWeight: '500', color: Colors.background, marginLeft: 6 },
});
