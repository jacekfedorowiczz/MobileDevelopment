import React, { useState } from 'react';
import { View, Text, StyleSheet, TextInput, Pressable, ActivityIndicator, Alert, SafeAreaView } from 'react-native';
import Icon from 'react-native-vector-icons/Feather';
import { Spacing } from '../theme/theme';
import { useAuthStore } from '../store/useAuthStore';
import { UserService } from '../api/UserService';
import { useTheme } from '../context/ThemeContext';

export default function LoginScreen({ navigation }: any) {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [isLoading, setIsLoading] = useState(false);
  const [errorMessage, setErrorMessage] = useState<string | null>(null);

  const setAuth = useAuthStore(state => state.setAuth);
  const { colors, toggleTheme, isDark } = useTheme();

  const handleLogin = async () => {
    if (!email || !password) {
      setErrorMessage('Wprowadź email i hasło.');
      return;
    }

    setErrorMessage(null);
    setIsLoading(true);

    try {
      const response = await UserService.login({
        email: email,
        password: password 
      });

      if (response.isSuccess && response.value) {
        setAuth(response.value.accessToken, response.value.refreshToken);
      } else {
        setErrorMessage(response.errorMessage || 'Błędny login lub hasło.');
      }
    } catch (error: any) {
      setErrorMessage(error.message || 'Wystąpił błąd podczas logowania.');
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <SafeAreaView style={[styles.container, { backgroundColor: colors.background }]}>
        <View style={styles.content}>

          <View style={styles.logoContainer}>
            <View style={styles.logoIconWrapper}>
              <Icon name="activity" size={40} color="#fff" />
            </View>
            <Text style={styles.logoText}>FitTracker</Text>
            <Text style={[styles.subtitle, { color: colors.mutedForeground }]}>Śledź swoje postępy na siłowni</Text>
          </View>

          <View style={styles.formContainer}>
            {errorMessage && (
              <View style={styles.errorContainer}>
                <Text style={styles.errorText}>{errorMessage}</Text>
              </View>
            )}

            <View style={styles.inputGroup}>
              <Text style={[styles.inputLabel, { color: colors.foreground }]}>Email</Text>
              <TextInput
                style={[styles.textInput, { color: colors.foreground, borderColor: colors.border, backgroundColor: 'rgba(255,255,255,0.05)' }]}
                value={email}
                onChangeText={setEmail}
                placeholder="Twój email"
                placeholderTextColor={colors.mutedForeground}
                autoCapitalize="none"
              />
            </View>

            <View style={styles.inputGroup}>
              <Text style={[styles.inputLabel, { color: colors.foreground }]}>Hasło</Text>
              <TextInput
                style={[styles.textInput, { color: colors.foreground, borderColor: colors.border, backgroundColor: 'rgba(255,255,255,0.05)' }]}
                value={password}
                onChangeText={setPassword}
                placeholder="••••••••"
                placeholderTextColor={colors.mutedForeground}
                secureTextEntry
              />
            </View>

            <Pressable 
              style={[styles.loginButton, isLoading && styles.loginButtonDisabled]} 
              onPress={handleLogin}
              disabled={isLoading}
            >
              {isLoading ? (
                <ActivityIndicator color="#ffffff" />
              ) : (
                <>
                  <Icon name="log-in" size={20} color="#ffffff" style={{ marginRight: 8 }} />
                  <Text style={styles.loginButtonText}>Zaloguj się</Text>
                </>
              )}
            </Pressable>
          </View>

          <Pressable style={styles.registerLink} onPress={() => navigation.navigate('Register')}>
            <Text style={[styles.registerLinkText, { color: colors.mutedForeground }]}>
              Nie masz konta? <Text style={styles.registerLinkHighlight}>Zarejestruj się</Text>
            </Text>
          </Pressable>
        </View>
    </SafeAreaView>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1 },
  content: { flex: 1, justifyContent: 'center', paddingHorizontal: Spacing.xl },
  logoContainer: { alignItems: 'center', marginBottom: Spacing.xl * 1.5 },
  logoIconWrapper: { width: 80, height: 80, borderRadius: 24, backgroundColor: '#2563eb', alignItems: 'center', justifyContent: 'center', marginBottom: Spacing.lg, shadowColor: '#2563eb', shadowOpacity: 0.3, shadowRadius: 10, elevation: 5 },
  logoText: { fontSize: 36, fontWeight: 'bold', color: '#3b82f6' },
  subtitle: { fontSize: 14, marginTop: 8 },
  formContainer: { width: '100%' },
  inputGroup: { marginBottom: Spacing.lg },
  inputLabel: { fontSize: 14, fontWeight: '500', marginBottom: 8 },
  textInput: { height: 48, borderRadius: 12, paddingHorizontal: Spacing.md, borderWidth: 1, fontSize: 14 },
  loginButton: { height: 48, backgroundColor: '#2563eb', borderRadius: 12, flexDirection: 'row', alignItems: 'center', justifyContent: 'center', marginTop: Spacing.md, shadowColor: '#2563eb', shadowOpacity: 0.3, shadowRadius: 10, elevation: 4 },
  loginButtonDisabled: { backgroundColor: '#1d4ed8', opacity: 0.7 },
  loginButtonText: { fontSize: 16, fontWeight: '600', color: '#ffffff' },
  registerLink: { marginTop: Spacing.xl, alignItems: 'center' },
  registerLinkText: { fontSize: 14 },
  registerLinkHighlight: { color: '#3b82f6', fontWeight: '600' },
  errorContainer: { backgroundColor: 'rgba(239, 68, 68, 0.1)', borderWidth: 1, borderColor: '#ef4444', borderRadius: 8, padding: 12, marginBottom: Spacing.lg },
  errorText: { color: '#ef4444', fontSize: 14, textAlign: 'center' },
});

