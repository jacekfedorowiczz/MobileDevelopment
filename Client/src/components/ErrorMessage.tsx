import React from 'react';
import { StyleSheet, Text, View } from 'react-native';
import { Spacing } from '../theme/theme';

type ErrorMessageProps = {
  message?: string | null;
};

export default function ErrorMessage({ message }: ErrorMessageProps) {
  if (!message) return null;

  return (
    <View style={styles.container}>
      <Text style={styles.text}>{message}</Text>
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    backgroundColor: 'rgba(239, 68, 68, 0.1)',
    borderWidth: 1,
    borderColor: '#ef4444',
    borderRadius: 8,
    padding: 12,
    marginBottom: Spacing.lg,
  },
  text: { color: '#ef4444', fontSize: 14, textAlign: 'center' },
});
