// src/screens/RegisterScreen.tsx
import React, { useState } from 'react';
import { View, Text, TextInput, Pressable, ScrollView, ActivityIndicator } from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';
import DateTimePicker from '@react-native-community/datetimepicker';
import Icon from 'react-native-vector-icons/Feather';
import { Spacing } from '../theme/theme';
import { useAuthStore } from '../store/useAuthStore';
import { UserService } from '../api/UserService';
import { useTheme } from '../context/ThemeContext';
import Logo from '../components/Logo';
import { getRegisterStyles } from './RegisterScreen.styles';

export default function RegisterScreen({ navigation }: any) {
  const [firstName, setFirstName] = useState('');
  const [lastName, setLastName] = useState('');
  const [email, setEmail] = useState('');
  const [birthDate, setBirthDate] = useState('');
  const [password, setPassword] = useState('');
  const [repeatPassword, setRepeatPassword] = useState('');
  const [phone, setPhone] = useState('');
  const [showDatePicker, setShowDatePicker] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const setAuth = useAuthStore(state => state.setAuth);
  const { colors } = useTheme();
  const styles = getRegisterStyles(colors);

  const handleDateChange = (event: any, selectedDate?: Date) => {
    setShowDatePicker(false);
    if (selectedDate) {
      const formattedDate = selectedDate.toISOString().split('T')[0];
      setBirthDate(formattedDate);
    }
  };

  const handleRegister = async () => {
    if (!firstName || !lastName || !email || !password || !repeatPassword || !birthDate) {
      setError('Wypełnij wszystkie pola.');
      return;
    }

    if (password !== repeatPassword) {
      setError('Hasła nie są takie same.');
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
    <SafeAreaView style={styles.container}>
        <ScrollView contentContainerStyle={styles.scrollContent}>
          <Pressable onPress={() => navigation.goBack()} style={styles.backButton}>
            <Icon name="arrow-left" size={16} color={colors.foreground} />
            <Text style={styles.backButtonText}>Wróć do logowania</Text>
          </Pressable>

          <Logo subtitleColor={colors.mutedForeground} />

          <View style={styles.header}>
            <Text style={styles.title}>Utwórz konto</Text>
          </View>

          <View style={styles.formContainer}>
            {error && (
              <View style={styles.errorContainer}>
                <Text style={styles.errorText}>{error}</Text>
              </View>
            )}
            <View style={styles.row}>
              <View style={[styles.inputGroup, { flex: 1, marginRight: Spacing.sm }]}>
                <Text style={styles.inputLabel}>Imię</Text>
                <TextInput
                  style={styles.textInput}
                  value={firstName}
                  onChangeText={setFirstName}
                  placeholder="Jan"
                  placeholderTextColor={colors.mutedForeground}
                />
              </View>
              <View style={[styles.inputGroup, { flex: 1, marginLeft: Spacing.sm }]}>
                <Text style={styles.inputLabel}>Nazwisko</Text>
                <TextInput
                  style={styles.textInput}
                  value={lastName}
                  onChangeText={setLastName}
                  placeholder="Kowalski"
                  placeholderTextColor={colors.mutedForeground}
                />
              </View>
            </View>

            <View style={styles.inputGroup}>
              <Text style={styles.inputLabel}>Email</Text>
              <TextInput
                style={styles.textInput}
                value={email}
                onChangeText={setEmail}
                placeholder="twoj@email.com"
                placeholderTextColor={colors.mutedForeground}
                keyboardType="email-address"
                autoCapitalize="none"
              />
            </View>

            <View style={styles.inputGroup}>
              <Text style={styles.inputLabel}>Data urodzenia</Text>
              <Pressable onPress={() => setShowDatePicker(true)}>
                <View style={styles.calendarInputWrapper}>
                  <Text style={[styles.calendarInputText, { color: birthDate ? colors.foreground : colors.mutedForeground }]}>
                    {birthDate || 'YYYY-MM-DD'}
                  </Text>
                  <Icon name="calendar" size={18} color={colors.mutedForeground} />
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
              <Text style={styles.inputLabel}>Hasło</Text>
              <TextInput
                style={styles.textInput}
                value={password}
                onChangeText={setPassword}
                placeholder="••••••••"
                placeholderTextColor={colors.mutedForeground}
                secureTextEntry
              />
            </View>

            <View style={styles.inputGroup}>
              <Text style={styles.inputLabel}>Powtórz hasło</Text>
              <TextInput
                style={styles.textInput}
                value={repeatPassword}
                onChangeText={setRepeatPassword}
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
