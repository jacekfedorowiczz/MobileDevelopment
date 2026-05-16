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
  heroCards: {
    flexDirection: 'row',
    gap: Spacing.md,
    marginBottom: Spacing.md,
  },
  heroCard: {
    flex: 1,
    minHeight: 176,
    backgroundColor: colors.card,
    borderRadius: 20,
    padding: Spacing.md,
    borderWidth: 1,
    borderColor: colors.border
  },
  heroHeader: {
    flexDirection: 'row',
    alignItems: 'center',
    marginBottom: Spacing.md
  },
  heroIconWrapper: {
    width: 38,
    height: 38,
    borderRadius: 12,
    backgroundColor: 'rgba(37, 99, 235, 0.1)',
    alignItems: 'center',
    justifyContent: 'center',
    marginRight: Spacing.sm
  },
  heroTitle: {
    fontSize: 16,
    fontWeight: '700',
    color: colors.foreground 
  },
  heroValue: {
    fontSize: 34,
    fontWeight: '800',
    color: colors.foreground
  },
  heroSubtitle: {
    fontSize: 12,
    color: colors.mutedForeground,
    marginTop: 2
  },
  progressTrack: {
    height: 8,
    borderRadius: 999,
    backgroundColor: colors.muted,
    overflow: 'hidden',
    marginTop: Spacing.md
  },
  progressFill: {
    height: '100%',
    borderRadius: 999,
    backgroundColor: '#2563eb'
  },
  macroText: {
    fontSize: 11,
    color: colors.mutedForeground,
    marginTop: Spacing.sm
  },
  heroStatsRow: {
    flexDirection: 'row',
    flexWrap: 'wrap',
    gap: Spacing.sm,
    marginTop: Spacing.md
  },
  heroStat: {
    color: '#2563eb',
    fontSize: 12,
    fontWeight: '700',
    backgroundColor: 'rgba(37, 99, 235, 0.1)',
    paddingHorizontal: 8,
    paddingVertical: 4,
    borderRadius: 8
  },
  sectionSeparator: {
    height: 1,
    backgroundColor: colors.border,
    marginBottom: Spacing.md
  },
  statsRow: { 
    flexDirection: 'row', 
    justifyContent: 'space-between', 
    marginBottom: Spacing.md 
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
  chartCard: {
    backgroundColor: colors.card,
    borderRadius: 18,
    borderWidth: 1,
    borderColor: colors.border,
    padding: Spacing.md,
    marginBottom: Spacing.md
  },
  chartBars: {
    height: 170,
    flexDirection: 'row',
    alignItems: 'flex-end',
    justifyContent: 'space-between'
  },
  chartColumn: {
    flex: 1,
    alignItems: 'center',
    height: '100%'
  },
  chartBarTrack: {
    flex: 1,
    width: 18,
    borderRadius: 999,
    backgroundColor: colors.muted,
    justifyContent: 'flex-end',
    overflow: 'hidden'
  },
  chartBar: {
    width: '100%',
    borderRadius: 999,
    backgroundColor: '#2563eb'
  },
  chartLabel: {
    marginTop: Spacing.sm,
    fontSize: 11,
    color: colors.mutedForeground
  },
  chartValue: {
    marginTop: 2,
    fontSize: 10,
    color: colors.mutedForeground
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
  emptyText: {
    color: colors.mutedForeground,
    fontSize: 14,
    marginBottom: Spacing.xl
  },
});
