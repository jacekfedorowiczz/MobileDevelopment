import { StyleSheet } from 'react-native';
import { Spacing } from '../theme/theme';

export const getDashboardStyles = (colors: any) => StyleSheet.create({
  container: { 
    flex: 1, 
    backgroundColor: colors.background 
  },
  scrollContent: {
    padding: Spacing.xl,
    paddingTop: Spacing.lg
  },
  welcome: { 
    fontSize: 24, 
    fontWeight: '600', 
    color: colors.foreground 
  },
  date: { 
    fontSize: 14, 
    color: colors.mutedForeground, 
    marginBottom: Spacing.xl 
  },
  actionsRow: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    marginBottom: Spacing.xl * 1.5,
  },
  actionButton: {
    flex: 1,
    backgroundColor: colors.secondary,
    borderRadius: 12,
    padding: Spacing.sm,
    marginHorizontal: Spacing.xs,
    alignItems: 'center',
    borderWidth: 1,
    borderColor: colors.border
  },
  actionIconWrapper: {
    marginBottom: Spacing.xs,
  },
  actionTitle: { 
    fontSize: 16, 
    fontWeight: '500', 
    color: colors.foreground 
  },
  actionDesc: { 
    fontSize: 12, 
    color: colors.mutedForeground 
  },
  placeholderBox: {
    height: 120,
    backgroundColor: colors.secondary,
    borderRadius: 12,
    justifyContent: 'center',
    alignItems: 'center',
    marginBottom: Spacing.xl * 1.5,
    borderWidth: 1,
    borderColor: colors.border
  },
  placeholderText: { 
    color: colors.mutedForeground 
  },
  statsRow: { 
    flexDirection: 'row', 
    justifyContent: 'space-between', 
    marginBottom: Spacing.xl * 1.5 
  },
  statBox: { 
    alignItems: 'center', 
    flex: 1,
    backgroundColor: colors.secondary,
    borderRadius: 12,
    paddingVertical: Spacing.md,
    marginHorizontal: Spacing.xs,
    borderWidth: 1,
    borderColor: colors.border
  },
  statIconWrapper: {
    marginBottom: Spacing.xs
  },
  statValue: { 
    fontSize: 20, 
    fontWeight: '600', 
    color: colors.foreground 
  },
  statLabel: { 
    fontSize: 12, 
    color: colors.mutedForeground 
  },
  sectionHeader: { 
    fontSize: 18, 
    fontWeight: '600', 
    marginBottom: Spacing.md, 
    color: colors.foreground 
  },
  workoutItem: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    paddingVertical: Spacing.md,
    borderBottomWidth: 1,
    borderColor: colors.border,
  },
  workoutName: { 
    fontSize: 16, 
    fontWeight: '500', 
    color: colors.foreground 
  },
  workoutDetails: { 
    fontSize: 12, 
    color: colors.mutedForeground,
    marginTop: 4
  },
  workoutDate: {
    backgroundColor: colors.background,
    borderRadius: 6,
    borderWidth: 1,
    borderColor: colors.border,
    paddingHorizontal: Spacing.xs,
    paddingVertical: 2,
    alignSelf: 'flex-start'
  },
  workoutDateText: { 
    fontSize: 12, 
    color: colors.mutedForeground 
  },
});
