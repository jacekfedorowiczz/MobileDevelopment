import React from 'react';
import {
  KeyboardTypeOptions,
  StyleProp,
  StyleSheet,
  Text,
  TextInput,
  TextInputProps,
  TextStyle,
  View,
  ViewStyle,
} from 'react-native';
import { useTheme } from '../context/ThemeContext';
import { Spacing } from '../theme/theme';

type FormTextInputProps = TextInputProps & {
  label: string;
  suffix?: string;
  containerStyle?: StyleProp<ViewStyle>;
  inputStyle?: StyleProp<TextStyle>;
  keyboardType?: KeyboardTypeOptions;
};

export default function FormTextInput({
  label,
  suffix,
  containerStyle,
  inputStyle,
  multiline,
  ...props
}: FormTextInputProps) {
  const { colors } = useTheme();

  return (
    <View style={[styles.container, containerStyle]}>
      <Text style={[styles.label, { color: colors.foreground }]}>{label}</Text>
      <View style={[
        styles.inputWrapper,
        multiline && styles.textAreaWrapper,
        { backgroundColor: colors.card, borderColor: colors.border },
      ]}>
        <TextInput
          {...props}
          multiline={multiline}
          placeholderTextColor={colors.mutedForeground}
          style={[
            styles.input,
            multiline && styles.textArea,
            { color: colors.foreground },
            inputStyle,
          ]}
        />
        {suffix && <Text style={[styles.suffix, { color: colors.mutedForeground }]}>{suffix}</Text>}
      </View>
    </View>
  );
}

const styles = StyleSheet.create({
  container: { marginBottom: Spacing.md },
  label: { fontSize: 14, fontWeight: '600', marginBottom: 8 },
  inputWrapper: {
    minHeight: 48,
    borderRadius: 12,
    paddingHorizontal: Spacing.md,
    borderWidth: 1,
    flexDirection: 'row',
    alignItems: 'center',
  },
  textAreaWrapper: {
    minHeight: 96,
    alignItems: 'flex-start',
    paddingTop: Spacing.sm,
  },
  input: { flex: 1, minHeight: 48, fontSize: 14 },
  textArea: { minHeight: 80, textAlignVertical: 'top' },
  suffix: { fontSize: 13, fontWeight: '500', marginLeft: Spacing.sm },
});
