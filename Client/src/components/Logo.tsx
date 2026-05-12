import React from 'react';
import { View, Text, StyleSheet } from 'react-native';
import Icon from 'react-native-vector-icons/Feather';
import { Spacing } from '../theme/theme';

interface LogoProps {
  subtitleColor: string;
}

export default function Logo({ subtitleColor }: LogoProps) {
  return (
    <View style={styles.logoContainer}>
      <View style={styles.logoIconWrapper}>
        <Icon name="activity" size={40} color="#fff" />
      </View>
      <Text style={styles.logoText}>FitTracker</Text>
      <Text style={[styles.subtitle, { color: subtitleColor }]}>Śledź swoje postępy na siłowni</Text>
    </View>
  );
}

const styles = StyleSheet.create({
  logoContainer: { alignItems: 'center', marginBottom: Spacing.xl * 1.5 },
  logoIconWrapper: {
    width: 80,
    height: 80,
    borderRadius: 24,
    backgroundColor: '#2563eb',
    alignItems: 'center',
    justifyContent: 'center',
    marginBottom: Spacing.lg,
    shadowColor: '#2563eb',
    shadowOpacity: 0.3,
    shadowRadius: 10,
    elevation: 5,
  },
  logoText: { fontSize: 36, fontWeight: 'bold', color: '#3b82f6' },
  subtitle: { fontSize: 14, marginTop: 8 },
});
