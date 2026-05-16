import React from 'react';
import { StyleProp, StyleSheet, Text, View, ViewStyle } from 'react-native';
import { useSafeAreaInsets } from 'react-native-safe-area-context';
import BackButton, { backButtonSpacing } from './BackButton';
import { useTheme } from '../context/ThemeContext';
import { Spacing } from '../theme/theme';

type ScreenHeaderProps = {
  title: string;
  subtitle?: string;
  onBack?: () => void;
  style?: StyleProp<ViewStyle>;
};

export default function ScreenHeader({ title, subtitle, onBack, style }: ScreenHeaderProps) {
  const insets = useSafeAreaInsets();
  const { colors } = useTheme();

  return (
    <View style={[styles.header, { paddingTop: insets.top + Spacing.sm }, style]}>
      {onBack && <BackButton onPress={onBack} style={backButtonSpacing} />}
      <Text style={[styles.title, { color: colors.foreground }]}>{title}</Text>
      {subtitle && <Text style={[styles.subtitle, { color: colors.mutedForeground }]}>{subtitle}</Text>}
    </View>
  );
}

const styles = StyleSheet.create({
  header: { paddingHorizontal: Spacing.lg, paddingBottom: Spacing.md },
  title: { fontSize: 28, fontWeight: 'bold' },
  subtitle: { fontSize: 14, marginTop: 4, lineHeight: 20 },
});
