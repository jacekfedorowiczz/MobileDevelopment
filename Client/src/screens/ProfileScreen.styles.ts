import { StyleSheet } from 'react-native';
import { Spacing } from '../theme/theme';

export const getProfileStyles = (colors: any, isDark: boolean) => StyleSheet.create({
  container: { 
    flex: 1,
    backgroundColor: colors.background
  },
  scrollContent: { 
    padding: Spacing.xl, 
    paddingTop: Spacing.lg,
    paddingBottom: Spacing.xl 
  },
  headerRow: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    marginBottom: Spacing.md
  },
  themeToggleBtn: {
    padding: Spacing.sm,
    borderRadius: 12,
    backgroundColor: colors.secondary,
    borderWidth: 1,
    borderColor: colors.border
  },
  profileHeader: { 
    alignItems: 'center', 
    marginVertical: Spacing.md 
  },
  imageContainer: { 
    position: 'relative' 
  },
  profileImage: { 
    width: 100, 
    height: 100, 
    borderRadius: 50, 
    borderWidth: 3,
    borderColor: '#2563eb'
  },
  editImageBadge: { 
    position: 'absolute', 
    bottom: 0, 
    right: 0, 
    backgroundColor: '#2563eb', 
    width: 32, 
    height: 32, 
    borderRadius: 16, 
    alignItems: 'center', 
    justifyContent: 'center', 
    borderWidth: 3, 
    borderColor: colors.background 
  },
  userName: { 
    fontSize: 22, 
    fontWeight: '700', 
    marginTop: Spacing.md,
    color: colors.foreground
  },
  userEmail: { 
    fontSize: 14, 
    marginTop: 4,
    color: colors.mutedForeground
  },
  statsContainer: { 
    marginBottom: Spacing.lg 
  },
  statCard: { 
    padding: Spacing.md, 
    borderRadius: 16, 
    flexDirection: 'row', 
    justifyContent: 'space-between', 
    alignItems: 'center', 
    marginBottom: Spacing.sm, 
    borderWidth: 1,
    backgroundColor: colors.secondary,
    borderColor: colors.border
  },
  statLabel: { 
    fontSize: 13, 
    marginBottom: 4,
    color: colors.mutedForeground
  },
  statValue: { 
    fontSize: 24, 
    fontWeight: '700',
    color: colors.foreground
  },
  statIconWrapper: { 
    width: 52, 
    height: 52, 
    borderRadius: 12, 
    alignItems: 'center', 
    justifyContent: 'center',
    backgroundColor: isDark ? '#1e293b' : '#eff6ff'
  },
  sectionTitle: { 
    fontSize: 18, 
    fontWeight: '700', 
    color: colors.foreground
  },
  sectionHeaderRow: {
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'space-between',
    marginBottom: Spacing.md
  },
  viewAllButton: {
    flexDirection: 'row',
    alignItems: 'center',
    paddingLeft: Spacing.sm,
    paddingVertical: Spacing.xs
  },
  viewAllText: {
    color: '#2563eb',
    fontSize: 13,
    fontWeight: '700',
    marginRight: 2
  },
  quickActionsContainer: { 
    flexDirection: 'row', 
    justifyContent: 'space-between', 
    marginBottom: Spacing.lg 
  },
  quickActionCard: { 
    flex: 1, 
    padding: Spacing.md, 
    borderRadius: 16, 
    borderWidth: 1, 
    marginHorizontal: Spacing.xs,
    backgroundColor: colors.secondary,
    borderColor: colors.border
  },
  quickActionIconWrapper: { 
    width: 40, 
    height: 40, 
    borderRadius: 10, 
    alignItems: 'center', 
    justifyContent: 'center', 
    marginBottom: Spacing.md,
    backgroundColor: isDark ? '#1e293b' : '#eff6ff'
  },
  quickActionTitle: { 
    fontSize: 14, 
    fontWeight: '600', 
    marginBottom: 4,
    color: colors.foreground
  },
  quickActionFooter: { 
    flexDirection: 'row', 
    justifyContent: 'space-between', 
    alignItems: 'center' 
  },
  quickActionSubtitle: { 
    fontSize: 12,
    color: colors.mutedForeground
  },
  achievementsContainer: { 
    flexDirection: 'row', 
    flexWrap: 'wrap', 
    justifyContent: 'space-between', 
    marginBottom: Spacing.md 
  },
  achievementCard: { 
    width: '48%', 
    padding: Spacing.md, 
    borderRadius: 16, 
    marginBottom: Spacing.sm,
    position: 'relative',
    overflow: 'hidden'
  },
  achievementUnlocked: { 
    backgroundColor: '#2563eb',
    borderWidth: 1,
    borderColor: '#fbbf24'
  },
  achievementLocked: { 
    borderWidth: 1,
    backgroundColor: colors.secondary,
    borderColor: colors.border
  },
  achievementIconWrapper: { 
    width: 40, 
    height: 40, 
    borderRadius: 10, 
    alignItems: 'center', 
    justifyContent: 'center', 
    marginBottom: Spacing.md 
  },
  iconUnlocked: { 
    backgroundColor: 'rgba(255,255,255,0.2)' 
  },
  iconLocked: { 
    backgroundColor: colors.muted
  },
  achievementMedalBadge: {
    position: 'absolute',
    top: 8,
    right: 8,
    width: 28,
    height: 28,
    borderRadius: 14,
    backgroundColor: '#fbbf24',
    alignItems: 'center',
    justifyContent: 'center',
    borderWidth: 2,
    borderColor: 'rgba(255,255,255,0.7)'
  },
  achievementTitleUnlocked: { 
    fontSize: 14, 
    fontWeight: '700', 
    marginBottom: 4,
    color: '#fff'
  },
  achievementTitleLocked: {
    fontSize: 14, 
    fontWeight: '700', 
    marginBottom: 4,
    color: colors.foreground
  },
  achievementDescUnlocked: { 
    fontSize: 12,
    color: 'rgba(255,255,255,0.9)'
  },
  achievementDescLocked: {
    fontSize: 12,
    color: colors.mutedForeground
  },
  logoutButton: { 
    height: 52, 
    backgroundColor: '#ef4444', 
    borderRadius: 14, 
    alignItems: 'center', 
    justifyContent: 'center', 
    flexDirection: 'row', 
    marginTop: Spacing.md,
    marginBottom: Spacing.xl
  },
  logoutButtonText: { 
    fontSize: 16, 
    fontWeight: '700', 
    color: '#fff', 
    marginLeft: 10 
  },
  
  // Modal styles
  modalOverlay: { flex: 1, backgroundColor: 'rgba(0,0,0,0.5)', justifyContent: 'flex-end' },
  modalContent: { borderTopLeftRadius: 24, borderTopRightRadius: 24, padding: Spacing.lg, paddingBottom: 40, backgroundColor: colors.card },
  modalHeader: { paddingBottom: Spacing.md, marginBottom: Spacing.md, borderBottomWidth: 1, borderBottomColor: colors.border },
  modalTitle: { fontSize: 18, fontWeight: '700', textAlign: 'center', color: colors.foreground },
  modalOption: { flexDirection: 'row', alignItems: 'center', paddingVertical: Spacing.md },
  optionIcon1: { width: 44, height: 44, borderRadius: 22, alignItems: 'center', justifyContent: 'center', marginRight: Spacing.md, backgroundColor: '#eff6ff' },
  optionIcon2: { width: 44, height: 44, borderRadius: 22, alignItems: 'center', justifyContent: 'center', marginRight: Spacing.md, backgroundColor: '#f0fdf4' },
  optionText: { fontSize: 16, fontWeight: '500', color: colors.foreground },
  cancelOption: { justifyContent: 'center', marginTop: Spacing.sm },
  cancelText: { fontSize: 16, color: '#ef4444', fontWeight: '600' },
});
