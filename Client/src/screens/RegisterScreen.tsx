// src/screens/RegisterScreen.tsx
import React, { useState } from 'react';
import { View, Text, StyleSheet, TextInput, Pressable, ScrollView, ActivityIndicator, SafeAreaView } from 'react-native';
import DateTimePicker from '@react-native-community/datetimepicker';
import Icon from 'react-native-vector-icons/Feather';
import { Spacing } from '../theme/theme';
import { useAuthStore } from '../store/useAuthStore';
import { UserService } from '../api/UserService';
import { useTheme } from '../context/ThemeContext';

export default function RegisterScreen({ navigation }: any) {
  const [firstName, setFirstName] = useState('');
  const [lastName, setLastName] = useState('');
  const [email, setEmail] = useState('');
  const [birthDate, setBirthDate] = useState('');
  const [password, setPassword] = useState('');
  const [phone, setPhone] = useState('');
  const [showDatePicker, setShowDatePicker] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const setAuth = useAuthStore(state => state.setAuth);
  const { colors } = useTheme();

  const handleDateChange = (event: any, selectedDate?: Date) => {
    setShowDatePicker(false);
    if (selectedDate) {
      const formattedDate = selectedDate.toISOString().split('T')[0];
      setBirthDate(formattedDate);
    }
  };

  const handleRegister = async () => {
    if (!firstName || !lastName || !email || !password || !birthDate) {
      setError('Wypełnij wszystkie pola.');
      return;
    }

    setError(null);
    setIsLoading(true);

    try {
      const response = await UserService.register({
        email,
        password,
        firstName,
        lastName,
        mobilePhone: phone || '123456789',
        dateOfBirth: birthDate // Zakładamy format YYYY-MM-DD
      });

      if (response.isSuccess && response.value) {
        setAuth(response.value.accessToken, response.value.refreshToken);
      } else {
        setError(response.errorMessage || 'Wystąpił błąd podczas rejestracji.');
      }
    } catch (err: any) {
      setError(err.message || 'Błąd połączenia z serwerem.');
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <SafeAreaView style={[styles.container, { backgroundColor: colors.background }]}>
        <ScrollView contentContainerStyle={styles.scrollContent}>
          <Pressable onPress={() => navigation.goBack()} style={styles.backButton}>
            <Icon name="arrow-left" size={16} color={colors.mutedForeground} />
            <Text style={[styles.backButtonText, { color: colors.mutedForeground }]}>Wróć do logowania</Text>
          </Pressable>

          <View style={styles.header}>
            <Text style={[styles.title, { color: colors.foreground }]}>Utwórz konto</Text>
            <Text style={[styles.subtitle, { color: colors.mutedForeground }]}>Dołącz do nas i śledź swoje postępy</Text>
          </View>

          <View style={styles.formContainer}>
            {error && (
              <View style={styles.errorContainer}>
                <Text style={styles.errorText}>{error}</Text>
              </View>
            )}
            <View style={styles.row}>
              <View style={[styles.inputGroup, { flex: 1, marginRight: Spacing.sm }]}>
                <Text style={[styles.inputLabel, { color: colors.foreground }]}>Imię</Text>
                <TextInput
                  style={[styles.textInput, { color: colors.foreground, borderColor: colors.border }]}
                  value={firstName}
                  onChangeText={setFirstName}
                  placeholder="Jan"
                  placeholderTextColor={colors.mutedForeground}
                />
              </View>
              <View style={[styles.inputGroup, { flex: 1, marginLeft: Spacing.sm }]}>
                <Text style={[styles.inputLabel, { color: colors.foreground }]}>Nazwisko</Text>
                <TextInput
                  style={[styles.textInput, { color: colors.foreground, borderColor: colors.border }]}
                  value={lastName}
                  onChangeText={setLastName}
                  placeholder="Kowalski"
                  placeholderTextColor={colors.mutedForeground}
                />
              </View>
            </View>

            <View style={styles.inputGroup}>
              <Text style={[styles.inputLabel, { color: colors.foreground }]}>Email</Text>
              <TextInput
                style={[styles.textInput, { color: colors.foreground, borderColor: colors.border }]}
                value={email}
                onChangeText={setEmail}
                placeholder="twoj@email.com"
                placeholderTextColor={colors.mutedForeground}
                keyboardType="email-address"
                autoCapitalize="none"
              />
            </View>

            <View style={styles.inputGroup}>
              <Text style={[styles.inputLabel, { color: colors.foreground }]}>Data urodzenia</Text>
              <Pressable onPress={() => setShowDatePicker(true)}>
                <View style={[styles.textInput, { justifyContent: 'center', borderColor: colors.border }]}>
                  <Text style={{ color: birthDate ? colors.foreground : colors.mutedForeground }}>
                    {birthDate || 'YYYY-MM-DD'}
                  </Text>
                </View>
              </Pressable>
              {showDatePicker && (
                <DateTimePicker
                  value={birthDate ? new Date(birthDate) : new Date()}
                  mode="date"
                  display="default"
                  onChange={handleDateChange}
                  maximumDate={new Date()}
                />
              )}
            </View>

            <View style={styles.inputGroup}>
              <Text style={[styles.inputLabel, { color: colors.foreground }]}>Hasło</Text>
              <TextInput
                style={[styles.textInput, { color: colors.foreground, borderColor: colors.border }]}
                value={password}
                onChangeText={setPassword}
                placeholder="••••••••"
                placeholderTextColor={colors.mutedForeground}
                secureTextEntry
              />
            </View>

            <Pressable 
              style={[styles.registerButton, isLoading && styles.registerButtonDisabled]} 
              onPress={handleRegister}
              disabled={isLoading}
            >
              {isLoading ? (
                <ActivityIndicator color="#ffffff" />
              ) : (
                <>
                  <Icon name="user-plus" size={20} color="#ffffff" style={{ marginRight: 8 }} />
                  <Text style={styles.registerButtonText}>Zarejestruj się</Text>
                </>
              )}
            </Pressable>
          </View>
        </ScrollView>
    </SafeAreaView>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1 },
  scrollContent: { flexGrow: 1, justifyContent: 'center', paddingHorizontal: Spacing.xl, paddingVertical: Spacing.xl * 2 },
  backButton: { flexDirection: 'row', alignItems: 'center', marginBottom: Spacing.xl },
  backButtonText: { fontSize: 14, marginLeft: 8 },
  header: { alignItems: 'center', marginBottom: Spacing.xl },
  title: { fontSize: 28, fontWeight: 'bold' },
  subtitle: { fontSize: 14, marginTop: 4 },
  formContainer: { width: '100%' },
  row: { flexDirection: 'row', justifyContent: 'space-between' },
  inputGroup: { marginBottom: Spacing.md },
  inputLabel: { fontSize: 14, fontWeight: '500', marginBottom: 8 },
  textInput: { height: 48, backgroundColor: 'rgba(255,255,255,0.05)', borderRadius: 12, paddingHorizontal: Spacing.md, borderWidth: 1, fontSize: 14 },
  registerButton: { height: 48, backgroundColor: '#2563eb', borderRadius: 12, flexDirection: 'row', alignItems: 'center', justifyContent: 'center', marginTop: Spacing.md, shadowColor: '#2563eb', shadowOpacity: 0.3, shadowRadius: 10, elevation: 4 },
  registerButtonDisabled: { opacity: 0.7 },
  registerButtonText: { fontSize: 16, fontWeight: '600', color: '#ffffff' },
  errorContainer: { backgroundColor: 'rgba(239, 68, 68, 0.1)', borderWidth: 1, borderColor: '#ef4444', borderRadius: 8, padding: 12, marginBottom: Spacing.lg },
  errorText: { color: '#ef4444', fontSize: 14, textAlign: 'center' },
});
