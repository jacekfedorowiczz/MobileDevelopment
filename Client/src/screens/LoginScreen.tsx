import React, { useState } from 'react';
import { View, Text, StyleSheet, Pressable, SafeAreaView } from 'react-native';
import { Spacing } from '../theme/theme';
import { useAuthStore } from '../store/useAuthStore';
import { UserService } from '../api/UserService';
import { useTheme } from '../context/ThemeContext';
import Logo from '../components/Logo';
import ErrorMessage from '../components/ErrorMessage';
import FormTextInput from '../components/FormTextInput';
import PrimaryButton from '../components/PrimaryButton';

export default function LoginScreen({ navigation }: any) {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [isLoading, setIsLoading] = useState(false);
  const [errorMessage, setErrorMessage] = useState<string | null>(null);

  const setAuth = useAuthStore(state => state.setAuth);
  const { colors } = useTheme();

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

          <Logo subtitleColor={colors.mutedForeground} />

          <View style={styles.formContainer}>
            <ErrorMessage message={errorMessage} />

            <FormTextInput
              label="Email"
              value={email}
              onChangeText={setEmail}
              placeholder="Twój email"
              autoCapitalize="none"
            />

            <FormTextInput
              label="Hasło"
              value={password}
              onChangeText={setPassword}
              placeholder="••••••••"
              secureTextEntry
            />

            <PrimaryButton
              title="Zaloguj się"
              icon="log-in"
              onPress={handleLogin}
              loading={isLoading}
              style={styles.loginButton}
            />
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
  formContainer: { width: '100%' },
  loginButton: { marginTop: Spacing.md, shadowColor: '#2563eb', shadowOpacity: 0.3, shadowRadius: 10, elevation: 4 },
  registerLink: { marginTop: Spacing.xl, alignItems: 'center' },
  registerLinkText: { fontSize: 14 },
  registerLinkHighlight: { color: '#3b82f6', fontWeight: '600' },
});

