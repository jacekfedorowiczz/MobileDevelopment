// src/screens/RegisterScreen.tsx
import React, { useState } from 'react';
import { StyleSheet, View, Text, Pressable, ScrollView } from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';
import DateTimePicker from '@react-native-community/datetimepicker';
import Icon from 'react-native-vector-icons/Feather';
import { Spacing } from '../theme/theme';
import { useAuthStore } from '../store/useAuthStore';
import { UserService } from '../api/UserService';
import { useTheme } from '../context/ThemeContext';
import Logo from '../components/Logo';
import BackButton, { backButtonSpacing } from '../components/BackButton';
import ErrorMessage from '../components/ErrorMessage';
import FormTextInput from '../components/FormTextInput';
import PrimaryButton from '../components/PrimaryButton';

export default function RegisterScreen({ navigation }: any) {
  const [firstName, setFirstName] = useState('');
  const [lastName, setLastName] = useState('');
  const [email, setEmail] = useState('');
  const [birthDate, setBirthDate] = useState('');
  const [password, setPassword] = useState('');
  const [repeatPassword, setRepeatPassword] = useState('');
  const [phone] = useState('');
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
          <BackButton onPress={() => navigation.goBack()} style={backButtonSpacing} />

          <Logo subtitleColor={colors.mutedForeground} />

          <View style={styles.header}>
            <Text style={styles.title}>Utwórz konto</Text>
          </View>

          <View style={styles.formContainer}>
            <ErrorMessage message={error} />
            <View style={styles.row}>
              <View style={[styles.inputGroup, { flex: 1, marginRight: Spacing.sm }]}>
                <FormTextInput
                  label="Imię"
                  value={firstName}
                  onChangeText={setFirstName}
                  placeholder="Jan"
                />
              </View>
              <View style={[styles.inputGroup, { flex: 1, marginLeft: Spacing.sm }]}>
                <FormTextInput
                  label="Nazwisko"
                  value={lastName}
                  onChangeText={setLastName}
                  placeholder="Kowalski"
                />
              </View>
            </View>

            <FormTextInput
              label="Email"
              value={email}
              onChangeText={setEmail}
              placeholder="twoj@email.com"
              keyboardType="email-address"
              autoCapitalize="none"
            />

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

            <FormTextInput
              label="Hasło"
              value={password}
              onChangeText={setPassword}
              placeholder="••••••••"
              secureTextEntry
            />

            <FormTextInput
              label="Powtórz hasło"
              value={repeatPassword}
              onChangeText={setRepeatPassword}
              placeholder="••••••••"
              secureTextEntry
            />

            <PrimaryButton
              title="Zarejestruj się"
              icon="user-plus"
              onPress={handleRegister}
              loading={isLoading}
              style={styles.registerButton}
            />



          </View>
        </ScrollView>
    </SafeAreaView>
  );
}

const getRegisterStyles = (colors: any) => StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: colors.background,
  },
  scrollContent: {
    flexGrow: 1,
    paddingHorizontal: Spacing.xl,
    paddingTop: Spacing.lg,
    paddingBottom: Spacing.xl * 2,
  },
  header: {
    alignItems: 'center',
    marginBottom: Spacing.sm,
    marginTop: Spacing.md,
  },
  title: {
    fontSize: 22,
    fontWeight: 'bold',
    color: colors.foreground,
  },
  formContainer: {
    width: '100%',
  },
  row: {
    flexDirection: 'row',
    justifyContent: 'space-between',
  },
  inputGroup: {
    marginBottom: 0,
  },
  inputLabel: {
    fontSize: 14,
    fontWeight: '500',
    marginBottom: 8,
    color: colors.foreground,
  },
  calendarInputWrapper: {
    flexDirection: 'row',
    alignItems: 'center',
    height: 48,
    backgroundColor: colors.card,
    borderRadius: 12,
    paddingHorizontal: Spacing.md,
    borderWidth: 1,
    borderColor: colors.border,
  },
  calendarInputText: {
    flex: 1,
    fontSize: 14,
  },
  registerButton: {
    marginTop: Spacing.md,
    shadowColor: '#2563eb',
    shadowOpacity: 0.3,
    shadowRadius: 10,
    elevation: 4,
  },
});
