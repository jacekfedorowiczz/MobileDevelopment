// src/screens/RegisterScreen.tsx
import React, { useState } from 'react';
import { View, Text, StyleSheet, TextInput, Pressable, ImageBackground, ScrollView } from 'react-native';
import Icon from 'react-native-vector-icons/Feather';
import { Colors, Spacing } from '../theme/theme';
import { useAuth } from '../context/AuthContext';

export default function RegisterScreen({ navigation }: any) {
  const [firstName, setFirstName] = useState('');
  const [lastName, setLastName] = useState('');
  const [email, setEmail] = useState('');
  const [birthDate, setBirthDate] = useState('');
  const [password, setPassword] = useState('');
  const { login } = useAuth();

  const handleRegister = () => {
    // Dummy register
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

        <ScrollView contentContainerStyle={styles.scrollContent}>
          <Pressable onPress={() => navigation.goBack()} style={styles.backButton}>
            <Icon name="arrow-left" size={16} color={Colors.mutedForeground} />
            <Text style={styles.backButtonText}>Wróć do logowania</Text>
          </Pressable>

          <View style={styles.header}>
            <Text style={styles.title}>Utwórz konto</Text>
            <Text style={styles.subtitle}>Dołącz do nas i śledź swoje postępy</Text>
          </View>

          <View style={styles.formContainer}>
            <View style={styles.row}>
              <View style={[styles.inputGroup, { flex: 1, marginRight: Spacing.sm }]}>
                <Text style={styles.inputLabel}>Imię</Text>
                <TextInput
                  style={styles.textInput}
                  value={firstName}
                  onChangeText={setFirstName}
                  placeholder="Jan"
                  placeholderTextColor={Colors.mutedForeground}
                />
              </View>
              <View style={[styles.inputGroup, { flex: 1, marginLeft: Spacing.sm }]}>
                <Text style={styles.inputLabel}>Nazwisko</Text>
                <TextInput
                  style={styles.textInput}
                  value={lastName}
                  onChangeText={setLastName}
                  placeholder="Kowalski"
                  placeholderTextColor={Colors.mutedForeground}
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
                placeholderTextColor={Colors.mutedForeground}
                keyboardType="email-address"
                autoCapitalize="none"
              />
            </View>

            <View style={styles.inputGroup}>
              <Text style={styles.inputLabel}>Data urodzenia</Text>
              <TextInput
                style={styles.textInput}
                value={birthDate}
                onChangeText={setBirthDate}
                placeholder="RRRR-MM-DD"
                placeholderTextColor={Colors.mutedForeground}
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

            <Pressable style={styles.registerButton} onPress={handleRegister}>
              <Text style={styles.registerButtonText}>Zarejestruj się</Text>
            </Pressable>
          </View>
        </ScrollView>
      </ImageBackground>
    </View>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1, backgroundColor: Colors.background },
  backgroundImage: { flex: 1, width: '100%', height: '100%' },
  backgroundImageStyle: { opacity: 0.2 },
  overlay: { ...StyleSheet.absoluteFillObject, backgroundColor: 'rgba(3, 2, 19, 0.8)' },
  scrollContent: { flexGrow: 1, justifyContent: 'center', paddingHorizontal: Spacing.xl, paddingVertical: Spacing.xl * 2 },
  backButton: { flexDirection: 'row', alignItems: 'center', marginBottom: Spacing.xl },
  backButtonText: { fontSize: 14, color: Colors.mutedForeground, marginLeft: 8 },
  header: { alignItems: 'center', marginBottom: Spacing.xl },
  title: { fontSize: 28, fontWeight: 'bold', color: Colors.foreground },
  subtitle: { fontSize: 14, color: Colors.mutedForeground, marginTop: 4 },
  formContainer: { width: '100%' },
  row: { flexDirection: 'row', justifyContent: 'space-between' },
  inputGroup: { marginBottom: Spacing.md },
  inputLabel: { fontSize: 14, fontWeight: '500', color: Colors.foreground, marginBottom: 8 },
  textInput: { height: 48, backgroundColor: 'rgba(255,255,255,0.05)', borderRadius: 12, paddingHorizontal: Spacing.md, borderWidth: 1, borderColor: Colors.border, color: Colors.foreground, fontSize: 14 },
  registerButton: { height: 48, backgroundColor: '#2563eb', borderRadius: 12, alignItems: 'center', justifyContent: 'center', marginTop: Spacing.md, shadowColor: '#2563eb', shadowOpacity: 0.3, shadowRadius: 10, elevation: 4 },
  registerButtonText: { fontSize: 16, fontWeight: '600', color: '#ffffff' },
});
