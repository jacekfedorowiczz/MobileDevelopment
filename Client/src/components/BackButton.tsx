import React from 'react';
import { Pressable, StyleProp, StyleSheet, ViewStyle } from 'react-native';
import Icon from 'react-native-vector-icons/Feather';
import { useTheme } from '../context/ThemeContext';
import { Spacing } from '../theme/theme';

type BackButtonProps = {
  onPress: () => void;
  style?: StyleProp<ViewStyle>;
};

export default function BackButton({ onPress, style }: BackButtonProps) {
  const { colors } = useTheme();

  return (
    <Pressable
      accessibilityRole="button"
      accessibilityLabel="Wróć"
      onPress={onPress}
      style={[styles.button, { backgroundColor: colors.card, borderColor: colors.border }, style]}
    >
      <Icon name="arrow-left" size={20} color={colors.foreground} />
    </Pressable>
  );
}

export const backButtonSpacing = {
  marginBottom: Spacing.md,
};

const styles = StyleSheet.create({
  button: {
    width: 40,
    height: 40,
    borderRadius: 20,
    alignItems: 'center',
    justifyContent: 'center',
    borderWidth: 1,
  },
});
