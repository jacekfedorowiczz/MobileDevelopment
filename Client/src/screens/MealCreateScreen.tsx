// src/screens/MealCreateScreen.tsx
import React, { useState } from 'react';
import { View, Text, StyleSheet, ScrollView, TextInput, Pressable, Alert } from 'react-native';
import { useSafeAreaInsets } from 'react-native-safe-area-context';
import Icon from 'react-native-vector-icons/Feather';
import { Colors, Spacing } from '../theme/theme';
import BackButton from '../components/BackButton';
import { useTheme } from '../context/ThemeContext';

export default function MealCreateScreen({ navigation }: any) {
  const insets = useSafeAreaInsets();
  const { colors } = useTheme();
  const [mealName, setMealName] = useState('');
  const [searchQuery, setSearchQuery] = useState('');

  const handleSave = () => {
    if (mealName) {
      Alert.alert('Sukces', 'Zapisano posiłek!');
      navigation.goBack();
    } else {
      Alert.alert('Błąd', 'Podaj nazwę posiłku');
    }
  };

  return (
    <View style={[styles.container, { backgroundColor: colors.background }]}>
      <View style={[styles.header, { paddingTop: insets.top + Spacing.sm }]}>
        <BackButton onPress={() => navigation.canGoBack() ? navigation.goBack() : navigation.navigate('Diet')} />
        <Text style={[styles.title, { color: colors.foreground }]}>Nowy posiłek</Text>
        <View style={{ width: 40 }} />
      </View>

      <ScrollView style={styles.content} contentContainerStyle={styles.scrollContent}>
        <View style={styles.inputGroup}>
          <Text style={styles.inputLabel}>Nazwa posiłku</Text>
          <TextInput
            style={styles.textInput}
            value={mealName}
            onChangeText={setMealName}
            placeholder="np. Śniadanie"
            placeholderTextColor={Colors.mutedForeground}
          />
        </View>

        <View style={styles.card}>
          <View style={styles.cardHeader}>
            <Text style={styles.cardTitle}>Produkty</Text>
            <Pressable style={styles.customButton}>
              <Icon name="plus" size={12} color="#2563eb" style={{ marginRight: 4 }} />
              <Text style={styles.customButtonText}>Własny</Text>
            </Pressable>
          </View>

          <View style={styles.searchContainer}>
            <Icon name="search" size={16} color={Colors.mutedForeground} style={styles.searchIcon} />
            <TextInput
              style={styles.searchInput}
              value={searchQuery}
              onChangeText={setSearchQuery}
              placeholder="Szukaj w bazie jedzenia..."
              placeholderTextColor={Colors.mutedForeground}
            />
          </View>

          <View style={styles.productsList}>
            {[1, 2].map((item) => (
              <View key={item} style={styles.productItem}>
                <View style={styles.productInfo}>
                  <Text style={styles.productName}>Owsianka z jabłkiem</Text>
                  <Text style={styles.productDetails}>350 kcal • 100g</Text>
                </View>
                <Pressable style={styles.deleteButton}>
                  <Icon name="trash-2" size={16} color="#ef4444" />
                </Pressable>
              </View>
            ))}
          </View>
        </View>

        <View style={styles.buttonRow}>
          <Pressable style={styles.cancelButton} onPress={() => navigation.goBack()}>
            <Icon name="x" size={16} color={Colors.foreground} style={{ marginRight: 8 }} />
            <Text style={styles.cancelButtonText}>Anuluj</Text>
          </Pressable>
          <Pressable style={styles.saveButton} onPress={handleSave}>
            <Icon name="save" size={16} color="#ffffff" style={{ marginRight: 8 }} />
            <Text style={styles.saveButtonText}>Zapisz</Text>
          </Pressable>
        </View>
      </ScrollView>
    </View>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1 },
  header: { flexDirection: 'row', alignItems: 'center', justifyContent: 'space-between', paddingHorizontal: Spacing.lg, paddingTop: Spacing.xl, paddingBottom: Spacing.md },
  title: { fontSize: 18, fontWeight: '600' },
  content: { flex: 1, paddingHorizontal: Spacing.lg },
  scrollContent: { paddingBottom: Spacing.xl },
  inputGroup: { marginBottom: Spacing.lg },
  inputLabel: { fontSize: 14, fontWeight: '500', color: Colors.foreground, marginBottom: 8 },
  textInput: { height: 48, backgroundColor: Colors.secondary, borderRadius: 12, paddingHorizontal: Spacing.md, borderWidth: 1, borderColor: Colors.border, color: Colors.foreground, fontSize: 14 },
  card: { backgroundColor: Colors.secondary, borderRadius: 16, padding: Spacing.md, borderWidth: 1, borderColor: Colors.border, marginBottom: Spacing.lg },
  cardHeader: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center', marginBottom: Spacing.md },
  cardTitle: { fontSize: 14, fontWeight: '600', color: Colors.foreground },
  customButton: { flexDirection: 'row', alignItems: 'center', backgroundColor: 'rgba(37, 99, 235, 0.1)', paddingHorizontal: 12, paddingVertical: 6, borderRadius: 8 },
  customButtonText: { fontSize: 12, fontWeight: '500', color: '#2563eb' },
  searchContainer: { flexDirection: 'row', alignItems: 'center', backgroundColor: Colors.background, borderRadius: 8, borderWidth: 1, borderColor: Colors.border, paddingHorizontal: Spacing.sm, height: 40, marginBottom: Spacing.md },
  searchIcon: { marginRight: Spacing.sm },
  searchInput: { flex: 1, fontSize: 14, color: Colors.foreground },
  productsList: { gap: Spacing.sm },
  productItem: { flexDirection: 'row', alignItems: 'center', justifyContent: 'space-between', backgroundColor: Colors.background, padding: Spacing.sm, borderRadius: 12, borderWidth: 1, borderColor: Colors.border },
  productInfo: { flex: 1 },
  productName: { fontSize: 14, fontWeight: '500', color: Colors.foreground },
  productDetails: { fontSize: 12, color: Colors.mutedForeground, marginTop: 2 },
  deleteButton: { padding: 8, borderRadius: 8, backgroundColor: 'rgba(239, 68, 68, 0.1)' },
  buttonRow: { flexDirection: 'row', gap: Spacing.sm, marginTop: Spacing.sm },
  cancelButton: { flex: 1, flexDirection: 'row', height: 48, backgroundColor: Colors.secondary, borderRadius: 12, alignItems: 'center', justifyContent: 'center', borderWidth: 1, borderColor: Colors.border },
  cancelButtonText: { fontSize: 14, fontWeight: '500', color: Colors.foreground },
  saveButton: { flex: 1, flexDirection: 'row', height: 48, backgroundColor: '#2563eb', borderRadius: 12, alignItems: 'center', justifyContent: 'center', shadowColor: '#2563eb', shadowOpacity: 0.2, shadowRadius: 10, elevation: 4 },
  saveButtonText: { fontSize: 14, fontWeight: '500', color: '#ffffff' },
});
