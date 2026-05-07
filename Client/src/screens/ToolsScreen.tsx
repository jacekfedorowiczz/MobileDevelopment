// src/screens/ToolsScreen.tsx
import React, { useState } from 'react';
import { View, Text, StyleSheet, ScrollView, TextInput, Pressable, Image } from 'react-native';
import Icon from 'react-native-vector-icons/Feather';
import { Colors, Spacing } from '../theme/theme';

const calculators = [
  { id: "bmi", name: "Kalkulator BMI", description: "Oblicz swój wskaźnik masy ciała i sprawdź, czy Twoja waga jest prawidłowa.", image: "https://images.unsplash.com/photo-1750521279808-f66baaed923d?crop=entropy&cs=tinysrgb&fit=max&fm=jpg&q=80&w=600" },
  { id: "calories", name: "Kalkulator kalorii", description: "Sprawdź swoje dzienne zapotrzebowanie kaloryczne (TDEE).", image: "https://images.unsplash.com/photo-1670164747721-d3500ef757a6?crop=entropy&cs=tinysrgb&fit=max&fm=jpg&q=80&w=600" },
  { id: "one-rm", name: "Kalkulator 1RM", description: "Oblicz swoje maksymalne obciążenie na jedno powtórzenie.", image: "https://images.unsplash.com/photo-1750521280541-bbf9d813a890?crop=entropy&cs=tinysrgb&fit=max&fm=jpg&q=80&w=600" },
  { id: "water", name: "Kalkulator nawodnienia", description: "Oblicz ile wody powinieneś wypijać każdego dnia.", image: "https://images.unsplash.com/photo-1523362628745-0c100150b504?crop=entropy&cs=tinysrgb&fit=max&fm=jpg&q=80&w=600" },
  { id: "hr-max", name: "Strefy tętna", description: "Wyznacz swoje maksymalne tętno (HRmax) oraz strefy treningowe.", image: "https://images.unsplash.com/photo-1571019614242-c5c5dee9f50b?crop=entropy&cs=tinysrgb&fit=max&fm=jpg&q=80&w=600" },
];

export default function ToolsScreen({ navigation }: any) {
  const [activeCalculator, setActiveCalculator] = useState<string | null>(null);
  const [weight, setWeight] = useState("");
  const [height, setHeight] = useState("");
  
  // A simplified BMI calculator for demonstration
  const calculateBMI = () => {
    const w = parseFloat(weight);
    const h = parseFloat(height) / 100;
    if (w && h) return (w / (h * h)).toFixed(1);
    return null;
  };

  if (activeCalculator === "bmi") {
    const bmiResult = calculateBMI();
    return (
      <View style={styles.container}>
        <View style={styles.header}>
          <Pressable onPress={() => setActiveCalculator(null)} style={styles.backButtonTextWrap}>
            <Icon name="arrow-left" size={16} color="#2563eb" />
            <Text style={styles.backButtonText}>Powrót do kalkulatorów</Text>
          </Pressable>
          <Text style={styles.title}>Kalkulator BMI</Text>
          <Text style={styles.subtitle}>Oblicz swój wskaźnik masy ciała</Text>
        </View>

        <ScrollView style={styles.content} contentContainerStyle={styles.scrollContent}>
          <View style={styles.inputGroup}>
            <Text style={styles.inputLabel}>Waga (kg)</Text>
            <TextInput style={styles.textInput} keyboardType="numeric" value={weight} onChangeText={setWeight} placeholder="75" placeholderTextColor={Colors.mutedForeground} />
          </View>
          <View style={styles.inputGroup}>
            <Text style={styles.inputLabel}>Wzrost (cm)</Text>
            <TextInput style={styles.textInput} keyboardType="numeric" value={height} onChangeText={setHeight} placeholder="180" placeholderTextColor={Colors.mutedForeground} />
          </View>

          {bmiResult && (
            <View style={styles.resultBox}>
              <Text style={styles.resultLabel}>Twoje BMI wynosi</Text>
              <Text style={styles.resultValue}>{bmiResult}</Text>
            </View>
          )}

          <Pressable style={styles.primaryButton}>
            <Text style={styles.primaryButtonText}>Wylicz BMI</Text>
          </Pressable>
        </ScrollView>
      </View>
    );
  }

  // Fallback to tools list
  return (
    <View style={styles.container}>
      <View style={styles.header}>
        <Pressable onPress={() => navigation.goBack()} style={styles.backButton}>
          <Icon name="arrow-left" size={20} color={Colors.foreground} />
        </Pressable>
        <Text style={styles.title}>Kalkulatory</Text>
        <Text style={styles.subtitle}>Wybierz odpowiednie narzędzie, aby dokładnie śledzić swoje parametry i skuteczniej optymalizować plan treningowy oraz dietę.</Text>
      </View>

      <ScrollView style={styles.content} contentContainerStyle={styles.scrollContent}>
        {calculators.map((calc) => (
          <Pressable key={calc.id} style={styles.calcCard} onPress={() => setActiveCalculator(calc.id)}>
            <Image source={{ uri: calc.image }} style={styles.calcImage} />
            <View style={styles.calcInfo}>
              <Text style={styles.calcName}>{calc.name}</Text>
              <Text style={styles.calcDesc} numberOfLines={2}>{calc.description}</Text>
            </View>
            <Icon name="chevron-right" size={20} color={Colors.mutedForeground} style={{ marginRight: Spacing.md }} />
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
  backButtonTextWrap: { flexDirection: 'row', alignItems: 'center', marginBottom: Spacing.lg },
  backButtonText: { color: '#2563eb', fontWeight: '500', marginLeft: 4 },
  title: { fontSize: 28, fontWeight: 'bold', color: Colors.foreground },
  subtitle: { fontSize: 14, color: Colors.mutedForeground, marginTop: 8, lineHeight: 20 },
  content: { flex: 1, paddingHorizontal: Spacing.lg },
  scrollContent: { paddingBottom: Spacing.xl },
  calcCard: { flexDirection: 'row', backgroundColor: Colors.secondary, borderRadius: 16, overflow: 'hidden', marginBottom: Spacing.md, borderWidth: 1, borderColor: Colors.border, alignItems: 'center' },
  calcImage: { width: 100, height: 100 },
  calcInfo: { flex: 1, padding: Spacing.sm, paddingLeft: Spacing.md },
  calcName: { fontSize: 16, fontWeight: '600', color: Colors.foreground, marginBottom: 4 },
  calcDesc: { fontSize: 12, color: Colors.mutedForeground, lineHeight: 18 },
  inputGroup: { marginBottom: Spacing.md },
  inputLabel: { fontSize: 14, fontWeight: '500', marginBottom: 8, color: Colors.foreground },
  textInput: { height: 48, backgroundColor: Colors.secondary, borderRadius: 12, paddingHorizontal: Spacing.md, borderWidth: 1, borderColor: Colors.border, color: Colors.foreground },
  resultBox: { backgroundColor: '#2563eb', padding: Spacing.xl, borderRadius: 16, alignItems: 'center', marginTop: Spacing.lg, marginBottom: Spacing.lg },
  resultLabel: { color: 'rgba(255,255,255,0.9)', fontSize: 14, marginBottom: 8 },
  resultValue: { fontSize: 48, fontWeight: 'bold', color: '#ffffff' },
  primaryButton: { height: 48, backgroundColor: '#2563eb', borderRadius: 12, alignItems: 'center', justifyContent: 'center' },
  primaryButtonText: { color: '#ffffff', fontSize: 16, fontWeight: '500' },
});
