import React, { useState } from 'react';
import { Alert, ScrollView, StyleSheet, Text, View } from 'react-native';
import { useSafeAreaInsets } from 'react-native-safe-area-context';
import Icon from 'react-native-vector-icons/Feather';
import { GymApiService } from '../api/GymApiService';
import { useTheme } from '../context/ThemeContext';
import { Spacing } from '../theme/theme';
import FormTextInput from '../components/FormTextInput';
import PrimaryButton from '../components/PrimaryButton';
import ScreenHeader from '../components/ScreenHeader';

export default function GymCreateScreen({ navigation }: any) {
  const { colors } = useTheme();
  const insets = useSafeAreaInsets();
  const [name, setName] = useState('');
  const [street, setStreet] = useState('');
  const [city, setCity] = useState('');
  const [zipCode, setZipCode] = useState('');
  const [description, setDescription] = useState('');
  const [latitude, setLatitude] = useState('');
  const [longitude, setLongitude] = useState('');
  const [isSaving, setIsSaving] = useState(false);

  const handleSave = async () => {
    if (!name.trim() || !street.trim() || !city.trim() || !zipCode.trim()) {
      Alert.alert('Brak danych', 'Uzupełnij nazwę, ulicę, miasto i kod pocztowy.');
      return;
    }

    setIsSaving(true);
    try {
      const createdGym = await GymApiService.createGym({
        name: name.trim(),
        street: street.trim(),
        city: city.trim(),
        zipCode: zipCode.trim(),
        latitude: Number(latitude.replace(',', '.')) || 0,
        longitude: Number(longitude.replace(',', '.')) || 0,
        rating: 0,
        description: description.trim() || undefined,
      });

      const message = createdGym.isActive
        ? 'Siłownia została dodana i jest już aktywna.'
        : 'Dziękujemy za zgłoszenie. Siłownia będzie widoczna publicznie po weryfikacji przez administratora.';

      navigation.navigate('Gyms', { gymCreatedMessage: message });
    } catch (e: any) {
      Alert.alert('Błąd', e.message ?? 'Nie udało się dodać siłowni.');
    } finally {
      setIsSaving(false);
    }
  };

  return (
    <View style={[styles.container, { backgroundColor: colors.background }]}>
      <ScreenHeader
        title="Dodaj siłownię"
        subtitle="Zgłoś nowe miejsce do atlasu siłowni"
        onBack={() => navigation.goBack()}
      />

      <ScrollView style={styles.content} contentContainerStyle={[styles.scrollContent, { paddingBottom: insets.bottom + Spacing.xl }]}>
        <FormTextInput label="Nazwa" value={name} onChangeText={setName} placeholder="np. CityFit Centrum" />
        <FormTextInput label="Ulica" value={street} onChangeText={setStreet} placeholder="np. Rejtana 65" />
        <FormTextInput label="Miasto" value={city} onChangeText={setCity} placeholder="np. Rzeszów" />
        <FormTextInput label="Kod pocztowy" value={zipCode} onChangeText={setZipCode} placeholder="np. 35-326" />
        <FormTextInput label="Szerokość geograficzna (opcjonalnie)" value={latitude} onChangeText={setLatitude} placeholder="np. 50.0245" keyboardType="decimal-pad" />
        <FormTextInput label="Długość geograficzna (opcjonalnie)" value={longitude} onChangeText={setLongitude} placeholder="np. 22.0156" keyboardType="decimal-pad" />
        <FormTextInput label="Opis (opcjonalnie)" value={description} onChangeText={setDescription} placeholder="Krótki opis miejsca" multiline />

        <View style={[styles.infoBox, { backgroundColor: colors.card, borderColor: colors.border }]}>
          <Icon name="info" size={18} color="#3b82f6" />
          <Text style={[styles.infoText, { color: colors.mutedForeground }]}>
            Zgłoszenie siłowni może wymagać weryfikacji przed publicznym wyświetleniem.
          </Text>
        </View>

        <PrimaryButton
          title="Dodaj siłownię"
          icon="check"
          loading={isSaving}
          onPress={handleSave}
          style={styles.saveButton}
        />
      </ScrollView>
    </View>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1 },
  content: { flex: 1, paddingHorizontal: Spacing.lg },
  scrollContent: { paddingTop: Spacing.md },
  infoBox: { flexDirection: 'row', gap: Spacing.sm, borderWidth: 1, borderRadius: 14, padding: Spacing.md, marginBottom: Spacing.lg },
  infoText: { flex: 1, fontSize: 13, lineHeight: 18 },
  saveButton: { minHeight: 54 },
});
