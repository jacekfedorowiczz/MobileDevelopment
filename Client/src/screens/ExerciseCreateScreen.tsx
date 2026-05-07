// src/screens/ExerciseCreateScreen.tsx
import React, { useState } from 'react';
import { View, Text, StyleSheet, ScrollView, TextInput, Pressable, Alert } from 'react-native';
import Icon from 'react-native-vector-icons/Feather';
import { Colors, Spacing } from '../theme/theme';
import { Picker } from '@react-native-picker/picker'; // Assumption that this could be used or a custom modal

const categories = ["Klatka piersiowa", "Plecy", "Nogi", "Barki", "Biceps", "Triceps"];

export default function ExerciseCreateScreen({ navigation }: any) {
  const [name, setName] = useState("");
  const [category, setCategory] = useState("Klatka piersiowa");
  const [description, setDescription] = useState("");
  const [muscleGroups, setMuscleGroups] = useState("");
  const [difficulty, setDifficulty] = useState("Początkujący");

  const handleAddExercise = () => {
    if (name && category) {
      Alert.alert("Sukces", "Dodano nowe ćwiczenie!");
      navigation.goBack();
    } else {
      Alert.alert("Błąd", "Uzupełnij wymagane pola");
    }
  };

  return (
    <View style={styles.container}>
      <View style={styles.header}>
        <Pressable onPress={() => navigation.goBack()} style={styles.backButton}>
          <Icon name="arrow-left" size={20} color={Colors.foreground} />
        </Pressable>
        <Text style={styles.title}>Nowe ćwiczenie</Text>
      </View>

      <ScrollView style={styles.content} contentContainerStyle={styles.scrollContent}>
        <View style={styles.card}>
          <Text style={styles.cardTitle}>Informacje główne</Text>

          <View style={styles.inputGroup}>
            <Text style={styles.inputLabel}>Zdjęcie (opcjonalnie)</Text>
            <Pressable style={styles.imageUploadBox}>
              <View style={styles.uploadIconWrapper}>
                <Icon name="upload-cloud" size={20} color={Colors.mutedForeground} />
              </View>
              <Text style={styles.uploadText}>Wybierz plik</Text>
            </Pressable>
          </View>

          <View style={styles.inputGroup}>
            <Text style={styles.inputLabel}>Nazwa ćwiczenia</Text>
            <TextInput
              style={styles.textInput}
              value={name}
              onChangeText={setName}
              placeholder="np. Wyciskanie hantli"
              placeholderTextColor={Colors.mutedForeground}
            />
          </View>

          <View style={styles.inputGroup}>
            <Text style={styles.inputLabel}>Opis <Text style={styles.optionalText}>(opcjonalnie)</Text></Text>
            <TextInput
              style={[styles.textInput, styles.textArea]}
              value={description}
              onChangeText={setDescription}
              placeholder="Dodaj krótkie wskazówki lub notatki..."
              placeholderTextColor={Colors.mutedForeground}
              multiline
              maxLength={256}
              textAlignVertical="top"
            />
            <Text style={styles.charCount}>{description.length} / 256</Text>
          </View>

          <View style={styles.inputGroup}>
            <Text style={styles.inputLabel}>Kategoria</Text>
            {/* Simple fallback using touchables for demo if Picker isn't available natively without linking */}
            <View style={styles.pickerFake}>
              <Text style={{ color: Colors.foreground }}>{category}</Text>
              <Icon name="chevron-down" size={16} color={Colors.mutedForeground} />
            </View>
          </View>

          <View style={styles.inputGroup}>
            <Text style={styles.inputLabel}>Partie mięśniowe <Text style={styles.optionalText}>(oddziel przecinkiem)</Text></Text>
            <TextInput
              style={styles.textInput}
              value={muscleGroups}
              onChangeText={setMuscleGroups}
              placeholder="np. Klatka, Triceps"
              placeholderTextColor={Colors.mutedForeground}
            />
          </View>

          <View style={styles.inputGroup}>
            <Text style={styles.inputLabel}>Poziom trudności</Text>
            <View style={styles.pickerFake}>
              <Text style={{ color: Colors.foreground }}>{difficulty}</Text>
              <Icon name="chevron-down" size={16} color={Colors.mutedForeground} />
            </View>
          </View>

          <View style={styles.buttonRow}>
            <Pressable style={styles.cancelButton} onPress={() => navigation.goBack()}>
              <Text style={styles.cancelButtonText}>Anuluj</Text>
            </Pressable>
            <Pressable style={styles.submitButton} onPress={handleAddExercise}>
              <Text style={styles.submitButtonText}>Dodaj ćwiczenie</Text>
            </Pressable>
          </View>
        </View>
      </ScrollView>
    </View>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1, backgroundColor: Colors.background },
  header: { flexDirection: 'row', alignItems: 'center', paddingHorizontal: Spacing.lg, paddingTop: Spacing.xl, paddingBottom: Spacing.md },
  backButton: { width: 40, height: 40, borderRadius: 20, backgroundColor: Colors.secondary, alignItems: 'center', justifyContent: 'center', borderWidth: 1, borderColor: Colors.border, marginRight: Spacing.md },
  title: { fontSize: 24, fontWeight: 'bold', color: Colors.foreground },
  content: { flex: 1, paddingHorizontal: Spacing.lg },
  scrollContent: { paddingBottom: Spacing.xl },
  card: { backgroundColor: Colors.secondary, borderRadius: 24, padding: Spacing.lg, borderWidth: 1, borderColor: Colors.border },
  cardTitle: { fontSize: 20, fontWeight: '600', color: Colors.foreground, marginBottom: Spacing.lg },
  inputGroup: { marginBottom: Spacing.md },
  inputLabel: { fontSize: 14, fontWeight: '500', color: Colors.foreground, marginBottom: 8 },
  optionalText: { fontWeight: 'normal', color: Colors.mutedForeground },
  textInput: { height: 48, backgroundColor: Colors.background, borderRadius: 12, paddingHorizontal: Spacing.md, borderWidth: 1, borderColor: Colors.border, color: Colors.foreground, fontSize: 14 },
  textArea: { height: 96, paddingTop: Spacing.sm },
  charCount: { fontSize: 12, color: Colors.mutedForeground, textAlign: 'right', marginTop: 4 },
  imageUploadBox: { height: 120, borderWidth: 2, borderColor: Colors.border, borderStyle: 'dashed', borderRadius: 12, alignItems: 'center', justifyContent: 'center' },
  uploadIconWrapper: { width: 40, height: 40, borderRadius: 20, backgroundColor: Colors.background, alignItems: 'center', justifyContent: 'center', marginBottom: 8 },
  uploadText: { fontSize: 14, fontWeight: '500', color: Colors.mutedForeground },
  pickerFake: { height: 48, backgroundColor: Colors.background, borderRadius: 12, paddingHorizontal: Spacing.md, borderWidth: 1, borderColor: Colors.border, flexDirection: 'row', alignItems: 'center', justifyContent: 'space-between' },
  buttonRow: { flexDirection: 'row', gap: Spacing.sm, marginTop: Spacing.md },
  cancelButton: { flex: 1, height: 48, backgroundColor: Colors.background, borderRadius: 12, alignItems: 'center', justifyContent: 'center', borderWidth: 1, borderColor: Colors.border },
  cancelButtonText: { fontSize: 14, fontWeight: '500', color: Colors.foreground },
  submitButton: { flex: 1, height: 48, backgroundColor: '#2563eb', borderRadius: 12, alignItems: 'center', justifyContent: 'center' },
  submitButtonText: { fontSize: 14, fontWeight: '500', color: '#fff' },
});
