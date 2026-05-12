import { StyleSheet } from 'react-native';
import { Spacing } from '../theme/theme';

export const getRegisterStyles = (colors: any) => StyleSheet.create({
  container: { 
    flex: 1,
    backgroundColor: colors.background,
  },
  scrollContent: { 
    flexGrow: 1, 
    paddingHorizontal: Spacing.xl, 
    paddingTop: Spacing.lg,
    paddingBottom: Spacing.xl * 2 
  },
  header: { 
    alignItems: 'center', 
    marginBottom: Spacing.sm,
    marginTop: Spacing.md
  },
  backButton: { 
    flexDirection: 'row', 
    alignItems: 'center', 
    alignSelf: 'flex-start',
    marginBottom: Spacing.lg,
    paddingVertical: Spacing.sm,
  },
  backButtonText: { 
    fontSize: 14, 
    marginLeft: 8,
    fontWeight: '500',
    color: colors.foreground
  },
  title: { 
    fontSize: 22, 
    fontWeight: 'bold',
    color: colors.foreground
  },
  subtitle: { 
    fontSize: 14, 
    marginTop: 4,
    color: colors.mutedForeground
  },
  formContainer: { 
    width: '100%' 
  },
  row: { 
    flexDirection: 'row', 
    justifyContent: 'space-between' 
  },
  inputGroup: { 
    marginBottom: Spacing.md 
  },
  inputLabel: { 
    fontSize: 14, 
    fontWeight: '500', 
    marginBottom: 8,
    color: colors.foreground
  },
  textInput: { 
    height: 48, 
    backgroundColor: 'rgba(255,255,255,0.05)', 
    borderRadius: 12, 
    paddingHorizontal: Spacing.md, 
    borderWidth: 1, 
    borderColor: colors.border,
    fontSize: 14,
    color: colors.foreground
  },
  calendarInputWrapper: {
    flexDirection: 'row',
    alignItems: 'center',
    height: 48, 
    backgroundColor: 'rgba(255,255,255,0.05)', 
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
    height: 48, 
    backgroundColor: '#2563eb', 
    borderRadius: 12, 
    flexDirection: 'row', 
    alignItems: 'center', 
    justifyContent: 'center', 
    marginTop: Spacing.md, 
    shadowColor: '#2563eb', 
    shadowOpacity: 0.3, 
    shadowRadius: 10, 
    elevation: 4 
  },
  registerButtonDisabled: { 
    opacity: 0.7 
  },
  registerButtonText: { 
    fontSize: 16, 
    fontWeight: '600', 
    color: '#ffffff' 
  },
  errorContainer: { 
    backgroundColor: 'rgba(239, 68, 68, 0.1)', 
    borderWidth: 1, 
    borderColor: '#ef4444', 
    borderRadius: 8, 
    padding: 12, 
    marginBottom: Spacing.lg 
  },
  errorText: { 
    color: '#ef4444', 
    fontSize: 14, 
    textAlign: 'center' 
  },
});
