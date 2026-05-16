// src/screens/WorkoutLogScreen.tsx
import React, { useCallback, useState } from 'react';
import { View, Text, StyleSheet, FlatList, Pressable, ActivityIndicator, RefreshControl } from 'react-native';
import { useSafeAreaInsets } from 'react-native-safe-area-context';
import Icon from 'react-native-vector-icons/Feather';
import { Colors, Spacing } from '../theme/theme';
import { useTheme } from '../context/ThemeContext';
import { WorkoutSessionApiService, WorkoutSessionSummaryDto } from '../api/WorkoutSessionApiService';
import BackButton, { backButtonSpacing } from '../components/BackButton';

const getIntensityColor = (rpe?: number) => {
  if (!rpe) return Colors.mutedForeground;
  if (rpe >= 8) return '#dc2626';
  if (rpe >= 6) return '#f59e0b';
  return '#16a34a';
};

const getIntensityLabel = (rpe?: number) => {
  if (!rpe) return '—';
  if (rpe >= 8) return 'Wysoka';
  if (rpe >= 6) return 'Średnia';
  return 'Niska';
};

const formatDuration = (startTime: string, endTime?: string) => {
  if (!endTime) return 'W trakcie';
  const diff = new Date(endTime).getTime() - new Date(startTime).getTime();
  const min = Math.round(diff / 60000);
  return `${min} min`;
};

const formatDate = (iso: string) => {
  return new Date(iso).toLocaleDateString('pl-PL', { day: 'numeric', month: 'long', year: 'numeric' });
};

import { useFocusEffect } from '@react-navigation/native';

export default function WorkoutLogScreen({ navigation }: any) {
  const { colors } = useTheme();
  const insets = useSafeAreaInsets();
  const [sessions, setSessions] = useState<WorkoutSessionSummaryDto[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [isRefreshing, setIsRefreshing] = useState(false);
  const [isLoadingMore, setIsLoadingMore] = useState(false);
  const [page, setPage] = useState(1);
  const [hasMore, setHasMore] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const loadSessions = useCallback(async (pageNum: number, replace: boolean) => {
    try {
      const result = await WorkoutSessionApiService.getPagedSessions(pageNum, 15);
      setSessions(prev => replace ? result.items : [...prev, ...result.items]);
      setHasMore(result.hasNextPage);
      setPage(pageNum);
      setError(null);
    } catch (e: any) {
      setError(e.message ?? 'Błąd pobierania treningów');
    }
  }, []);

  useFocusEffect(
    useCallback(() => {
      let isActive = true;

      loadSessions(1, true).finally(() => {
        if (isActive) {
          setIsLoading(false);
        }
      });

      return () => {
        isActive = false;
      };
    }, [loadSessions])
  );

  const onRefresh = useCallback(async () => {
    setIsRefreshing(true);
    await loadSessions(1, true);
    setIsRefreshing(false);
  }, [loadSessions]);

  const onEndReached = useCallback(async () => {
    if (isLoadingMore || !hasMore) return;
    setIsLoadingMore(true);
    await loadSessions(page + 1, false);
    setIsLoadingMore(false);
  }, [isLoadingMore, hasMore, page, loadSessions]);

  const renderItem = ({ item }: { item: WorkoutSessionSummaryDto }) => (
    <Pressable
      style={[styles.workoutCard, { backgroundColor: colors.card, borderColor: colors.border }]}
      onPress={() => navigation.navigate('WorkoutDetail', { id: item.id })}
    >
      <View style={styles.workoutHeader}>
        <View>
          <Text style={[styles.workoutName, { color: colors.foreground }]}>{item.name}</Text>
          <Text style={[styles.workoutTime, { color: colors.mutedForeground }]}>{formatDate(item.startTime)}</Text>
        </View>
        <View style={styles.chevronWrapper}>
          <Icon name="chevron-right" size={20} color="#3b82f6" />
        </View>
      </View>
      <View style={styles.statsRow}>
        <View style={[styles.statBox, { backgroundColor: colors.background, borderColor: colors.border }]}>
          <Text style={[styles.statLabel, { color: colors.mutedForeground }]}>Ćwiczenia</Text>
          <Text style={[styles.statValue, { color: colors.foreground }]}>{item.exerciseCount}</Text>
        </View>
        <View style={[styles.statBox, { backgroundColor: colors.background, borderColor: colors.border }]}>
          <Text style={[styles.statLabel, { color: colors.mutedForeground }]}>Czas</Text>
          <Text style={[styles.statValue, { color: colors.foreground }]}>{formatDuration(item.startTime, item.endTime)}</Text>
        </View>
        <View style={[styles.statBox, { backgroundColor: colors.background, borderColor: colors.border }]}>
          <Text style={[styles.statLabel, { color: colors.mutedForeground }]}>Intensywność</Text>
          <Text style={[styles.statValue, { color: getIntensityColor(item.globalSessionRpe ?? undefined) }]}>
            {getIntensityLabel(item.globalSessionRpe ?? undefined)}
          </Text>
        </View>
      </View>
    </Pressable>
  );

  return (
    <View style={[styles.container, { backgroundColor: colors.background }]}>
      <View style={[styles.header, { backgroundColor: colors.background, paddingTop: insets.top + Spacing.sm }]}>
        <BackButton onPress={() => navigation.navigate('TrainingHub')} style={backButtonSpacing} />
        <View style={styles.headerTop}>
          <Text style={[styles.title, { color: colors.foreground }]}>Dziennik</Text>
          <View style={styles.headerActions}>
            <Pressable
              style={[styles.iconButton, styles.iconButtonPrimary]}
              onPress={() => navigation.navigate('WorkoutCreate')}
            >
              <Icon name="plus" size={20} color="#fff" />
            </Pressable>
          </View>
        </View>
      </View>

      {isLoading ? (
        <View style={styles.centered}>
          <ActivityIndicator size="large" color="#3b82f6" />
        </View>
      ) : error ? (
        <View style={styles.centered}>
          <Icon name="wifi-off" size={40} color={Colors.mutedForeground} />
          <Text style={[styles.errorText, { color: Colors.mutedForeground }]}>{error}</Text>
          <Pressable style={styles.retryButton} onPress={onRefresh}>
            <Text style={styles.retryText}>Spróbuj ponownie</Text>
          </Pressable>
        </View>
      ) : (
        <FlatList
          data={sessions}
          keyExtractor={item => String(item.id)}
          renderItem={renderItem}
          contentContainerStyle={[styles.scrollContent, sessions.length === 0 && styles.emptyContainer]}
          refreshControl={<RefreshControl refreshing={isRefreshing} onRefresh={onRefresh} tintColor="#3b82f6" />}
          onEndReached={onEndReached}
          onEndReachedThreshold={0.3}
          ListEmptyComponent={
            <View style={styles.emptyState}>
              <Icon name="activity" size={48} color={Colors.mutedForeground} />
              <Text style={[styles.emptyTitle, { color: colors.foreground }]}>Brak treningów</Text>
              <Text style={[styles.emptyDesc, { color: colors.mutedForeground }]}>Zacznij swój pierwszy trening!</Text>
              <Pressable style={styles.startButton} onPress={() => navigation.navigate('WorkoutCreate')}>
                <Icon name="plus" size={16} color="#fff" />
                <Text style={styles.startButtonText}>Nowy trening</Text>
              </Pressable>
            </View>
          }
          ListFooterComponent={isLoadingMore ? <ActivityIndicator style={{ marginVertical: 16 }} color="#3b82f6" /> : null}
        />
      )}
    </View>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1 },
  header: { paddingHorizontal: Spacing.lg, paddingTop: Spacing.lg, paddingBottom: Spacing.md },
  headerTop: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center' },
  title: { fontSize: 28, fontWeight: 'bold' },
  headerActions: { flexDirection: 'row', gap: Spacing.xs },
  iconButton: { width: 44, height: 44, borderRadius: 12, alignItems: 'center', justifyContent: 'center', borderWidth: 1, borderColor: Colors.border, marginLeft: Spacing.xs },
  iconButtonPrimary: { backgroundColor: '#3b82f6', borderColor: '#3b82f6' },
  scrollContent: { paddingHorizontal: Spacing.lg, paddingBottom: Spacing.xl },
  emptyContainer: { flex: 1 },
  workoutCard: { borderRadius: 16, padding: Spacing.md, marginBottom: Spacing.sm, borderWidth: 1 },
  workoutHeader: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'flex-start', marginBottom: Spacing.md },
  workoutName: { fontSize: 18, fontWeight: '600', marginBottom: 4 },
  workoutTime: { fontSize: 14 },
  chevronWrapper: { width: 32, height: 32, borderRadius: 8, backgroundColor: 'rgba(59, 130, 246, 0.1)', alignItems: 'center', justifyContent: 'center' },
  statsRow: { flexDirection: 'row', gap: Spacing.sm },
  statBox: { flex: 1, borderRadius: 12, padding: Spacing.sm, borderWidth: 1 },
  statLabel: { fontSize: 12, marginBottom: 6 },
  statValue: { fontSize: 16, fontWeight: '600' },
  centered: { flex: 1, justifyContent: 'center', alignItems: 'center', padding: Spacing.xl },
  errorText: { marginTop: Spacing.md, fontSize: 14, textAlign: 'center' },
  retryButton: { marginTop: Spacing.lg, paddingHorizontal: Spacing.lg, paddingVertical: Spacing.sm, backgroundColor: '#3b82f6', borderRadius: 12 },
  retryText: { color: '#fff', fontWeight: '500' },
  emptyState: { flex: 1, alignItems: 'center', justifyContent: 'center', paddingVertical: Spacing.xl * 2 },
  emptyTitle: { fontSize: 20, fontWeight: '600', marginTop: Spacing.lg },
  emptyDesc: { fontSize: 14, marginTop: Spacing.sm, marginBottom: Spacing.xl },
  startButton: { flexDirection: 'row', alignItems: 'center', gap: 8, backgroundColor: '#3b82f6', paddingHorizontal: Spacing.lg, paddingVertical: Spacing.md, borderRadius: 12 },
  startButtonText: { color: '#fff', fontWeight: '600', fontSize: 16 },
});


