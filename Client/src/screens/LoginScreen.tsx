// src/screens/LoginScreen.tsx
import React, { useState } from 'react';
import { View, Text, StyleSheet, TextInput, Pressable, ImageBackground } from 'react-native';
import Icon from 'react-native-vector-icons/Feather';
import { Colors, Spacing } from '../theme/theme';
import { useAuth } from '../context/AuthContext';

export default function LoginScreen({ navigation }: any) {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const { login } = useAuth();

  const handleLogin = () => {
    // Dummy login
    login('dummy-token');
  };

  return (
    <View style={styles.container}>
      <ImageBackground
        source={{ uri: "https://images.unsplash.com/photo-1750698544932-c7471990f1ca?crop=entropy&cs=tinysrgb&fit=max&fm=jpg&q=80&w=800" }}
        style={styles.backgroundImage}
        imageStyle={styles.backgroundImageStyle}
      >
        <View style={styles.overlay} />

        <View style={styles.content}>
          <View style={styles.logoContainer}>
            <View style={styles.logoIconWrapper}>
              <Icon name="activity" size={40} color="#fff" />
            </View>
            <Text style={styles.logoText}>FitTracker</Text>
            <Text style={styles.subtitle}>Śledź swoje postępy na siłowni</Text>
          </View>

          <View style={styles.formContainer}>
            <View style={styles.inputGroup}>
              <Text style={styles.inputLabel}>Email</Text>
              <TextInput
                style={styles.textInput}
                value={email}
                onChangeText={setEmail}
                placeholder="twoj@email.com"
                placeholderTextColor={Colors.mutedForeground}
                keyboardType="email-address"
                autoCapitalize="none"
              />
            </View>

            <View style={styles.inputGroup}>
              <Text style={styles.inputLabel}>Hasło</Text>
              <TextInput
                style={styles.textInput}
                value={password}
                onChangeText={setPassword}
                placeholder="••••••••"
                placeholderTextColor={Colors.mutedForeground}
                secureTextEntry
              />
            </View>

            <Pressable style={styles.loginButton} onPress={handleLogin}>
              <Text style={styles.loginButtonText}>Zaloguj się</Text>
            </Pressable>
          </View>

          <Pressable style={styles.registerLink} onPress={() => navigation.navigate('Register')}>
            <Text style={styles.registerLinkText}>Nie masz konta? <Text style={styles.registerLinkHighlight}>Zarejestruj się</Text></Text>
          </Pressable>
        </View>
      </ImageBackground>
    </View>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1, backgroundColor: Colors.background },
  backgroundImage: { flex: 1, width: '100%', height: '100%' },
  backgroundImageStyle: { opacity: 0.2 },
  overlay: { ...StyleSheet.absoluteFillObject, backgroundColor: 'rgba(3, 2, 19, 0.8)' },
  content: { flex: 1, justifyContent: 'center', paddingHorizontal: Spacing.xl },
  logoContainer: { alignItems: 'center', marginBottom: Spacing.xl * 1.5 },
  logoIconWrapper: { width: 80, height: 80, borderRadius: 24, backgroundColor: '#2563eb', alignItems: 'center', justifyContent: 'center', marginBottom: Spacing.lg, shadowColor: '#2563eb', shadowOpacity: 0.3, shadowRadius: 10, elevation: 5 },
  logoText: { fontSize: 36, fontWeight: 'bold', color: '#3b82f6' },
  subtitle: { fontSize: 14, color: Colors.mutedForeground, marginTop: 8 },
  formContainer: { width: '100%' },
  inputGroup: { marginBottom: Spacing.lg },
  inputLabel: { fontSize: 14, fontWeight: '500', color: Colors.foreground, marginBottom: 8 },
  textInput: { height: 48, backgroundColor: 'rgba(255,255,255,0.05)', borderRadius: 12, paddingHorizontal: Spacing.md, borderWidth: 1, borderColor: Colors.border, color: Colors.foreground, fontSize: 14 },
  loginButton: { height: 48, backgroundColor: '#2563eb', borderRadius: 12, alignItems: 'center', justifyContent: 'center', marginTop: Spacing.md, shadowColor: '#2563eb', shadowOpacity: 0.3, shadowRadius: 10, elevation: 4 },
  loginButtonText: { fontSize: 16, fontWeight: '600', color: '#ffffff' },
  registerLink: { marginTop: Spacing.xl, alignItems: 'center' },
  registerLinkText: { fontSize: 14, color: Colors.mutedForeground },
  registerLinkHighlight: { color: '#3b82f6', fontWeight: '600' },
});
