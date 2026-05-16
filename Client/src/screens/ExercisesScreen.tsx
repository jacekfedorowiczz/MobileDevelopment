// src/screens/ExercisesScreen.tsx
import React, { useState, useCallback, useRef } from 'react';
import {
  View, Text, FlatList, TextInput, Pressable,
  ActivityIndicator, Modal, Alert
} from 'react-native';
import { useSafeAreaInsets } from 'react-native-safe-area-context';
import Icon from 'react-native-vector-icons/Feather';
import { Spacing } from '../theme/theme';
import { useTheme } from '../context/ThemeContext';
import { ExerciseApiService, ExerciseDifficulty, ExerciseDto, MuscleGroupDto } from '../api/ExerciseApiService';
import { useAuthStore } from '../store/useAuthStore';
import BackButton, { backButtonSpacing } from '../components/BackButton';
import { styles } from './ExercisesScreen.styles';

const getDifficultyLabel = (difficulty?: ExerciseDifficulty) => {
  switch (difficulty) {
    case ExerciseDifficulty.Beginner: return 'Początkujący';
    case ExerciseDifficulty.Intermediate: return 'Średniozaawansowany';
    case ExerciseDifficulty.Advanced: return 'Zaawansowany';
    case ExerciseDifficulty.Elite: return 'Elita';
    default: return undefined;
  }
};

const getDifficultyColor = (difficulty?: ExerciseDifficulty) => {
  switch (difficulty) {
    case ExerciseDifficulty.Beginner: return '#16a34a';
    case ExerciseDifficulty.Intermediate: return '#d97706';
    case ExerciseDifficulty.Advanced: return '#dc2626';
    case ExerciseDifficulty.Elite: return '#7c3aed';
    default: return '#6b7280';
  }
};

import { useFocusEffect } from '@react-navigation/native';

const base64UrlDecode = (value: string) => {
  const base64 = value.replace(/-/g, '+').replace(/_/g, '/');
  const padded = base64.padEnd(base64.length + (4 - base64.length % 4) % 4, '=');
  const chars = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=';
  let output = '';
  let buffer = 0;
  let bits = 0;

  for (const char of padded) {
    if (char === '=') break;
    const index = chars.indexOf(char);
    if (index < 0) continue;

    buffer = (buffer << 6) | index;
    bits += 6;

    if (bits >= 8) {
      bits -= 8;
      output += String.fromCharCode((buffer >> bits) & 0xff);
    }
  }

  return decodeURIComponent(output.split('').map(char => `%${char.charCodeAt(0).toString(16).padStart(2, '0')}`).join(''));
};

const getAuthUser = (token: string | null) => {
  if (!token) return { userId: null as number | null, role: null as string | null };

  try {
    const payload = JSON.parse(base64UrlDecode(token.split('.')[1] ?? ''));
    const userIdClaim = payload.nameid
      ?? payload.sub
      ?? payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];
    const role = payload.role
      ?? payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];

    return {
      userId: Number.isFinite(Number(userIdClaim)) ? Number(userIdClaim) : null,
      role: typeof role === 'string' ? role : null,
    };
  } catch {
    return { userId: null as number | null, role: null as string | null };
  }
};

export default function ExercisesScreen({ navigation, route }: any) {
  const { colors } = useTheme();
  const insets = useSafeAreaInsets();
  const mode = route?.params?.mode;
  const onSelect = route?.params?.onSelect;
  const userToken = useAuthStore(state => state.userToken);
  const currentUser = getAuthUser(userToken);

  const [exercises, setExercises] = useState<ExerciseDto[]>([]);
  const [muscleGroups, setMuscleGroups] = useState<MuscleGroupDto[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [searchQuery, setSearchQuery] = useState('');
  const [selectedGroupIds, setSelectedGroupIds] = useState<number[]>([]);
  const [showFilter, setShowFilter] = useState(false);
  const [page, setPage] = useState(1);
  const [hasMore, setHasMore] = useState(true);
  const [isLoadingMore, setIsLoadingMore] = useState(false);

  // Refs to hold the latest filter values and avoid stale closures
  const searchRef = useRef('');
  const groupIdsRef = useRef<number[]>([]);
  const debounceTimer = useRef<ReturnType<typeof setTimeout> | null>(null);

  const loadMuscleGroups = useCallback(async () => {
    try {
      const groups = await ExerciseApiService.getMuscleGroups();
      setMuscleGroups(groups ?? []);
    } catch { }
  }, []);

  const loadExercises = useCallback(async (pageNum: number, replace: boolean, search?: string, groupIds?: number[]) => {
    try {
      const result = await ExerciseApiService.getPagedExercises(pageNum, 20, search, groupIds);
      const items = result?.items ?? [];
      setExercises(prev => replace ? items : [...prev, ...items]);
      setHasMore(result?.hasNextPage ?? false);
      setPage(pageNum);
    } catch (e: any) {
      console.error('Load exercises error:', e);
    }
  }, []);

  useFocusEffect(
    useCallback(() => {
      let isActive = true;

      Promise.all([
        loadMuscleGroups(),
        loadExercises(1, true, searchRef.current, groupIdsRef.current.length ? groupIdsRef.current : undefined),
      ]).finally(() => {
        if (isActive) setIsLoading(false);
      });

      return () => {
        isActive = false;
      };
    }, [loadMuscleGroups, loadExercises])
  );

  const onSearch = (q: string) => {
    setSearchQuery(q);
    searchRef.current = q;

    // Debounce: wait 350ms after user stops typing before firing request
    if (debounceTimer.current) clearTimeout(debounceTimer.current);
    debounceTimer.current = setTimeout(() => {
      loadExercises(1, true, searchRef.current, groupIdsRef.current.length ? groupIdsRef.current : undefined);
    }, 350);
  };

  const applyFilters = () => {
    setShowFilter(false);
    const ids = groupIdsRef.current;
    loadExercises(1, true, searchRef.current, ids.length ? ids : undefined);
  };

  const onEndReached = async () => {
    if (!hasMore || isLoadingMore) return;
    setIsLoadingMore(true);
    await loadExercises(page + 1, false, searchRef.current, groupIdsRef.current.length ? groupIdsRef.current : undefined);
    setIsLoadingMore(false);
  };

  const toggleGroup = (id: number) => {
    const next = groupIdsRef.current.includes(id)
      ? groupIdsRef.current.filter(x => x !== id)
      : [...groupIdsRef.current, id];

    groupIdsRef.current = next;
    setSelectedGroupIds(next);
  };

  const handleDeleteExercise = (id: number, name: string) => {
    Alert.alert('Usuń ćwiczenie', `Czy na pewno chcesz usunąć ćwiczenie "${name}"?`, [
      { text: 'Anuluj', style: 'cancel' },
      {
        text: 'Usuń', style: 'destructive',
        onPress: async () => {
          try {
            await ExerciseApiService.deleteExercise(id);
            setExercises(prev => prev.filter(ex => ex.id !== id));
            Alert.alert('Usunięto', `Ćwiczenie "${name}" zostało usunięte.`);
          } catch (e: any) {
            Alert.alert('Błąd', e.message ?? 'Nie udało się usunąć ćwiczenia');
          }
        },
      },
    ]);
  };

  const renderItem = ({ item }: { item: ExerciseDto }) => {
    const muscles = item.targetedMuscles?.map(m => m.name).join(', ') ?? '';
    const difficultyLabel = getDifficultyLabel(item.difficulty);
    const diffColor = getDifficultyColor(item.difficulty);
    const canDelete = mode !== 'pick'
      && (currentUser.role === 'Administrator' || item.createdByUserId === currentUser.userId);

    return (
      <Pressable
        style={[styles.card, { backgroundColor: colors.card, borderColor: colors.border }]}
        onPress={() => {
          if (mode === 'pick') {
            onSelect?.(item.id, item.name);
            navigation.goBack();
          } else {
            navigation.navigate('ExerciseDetail', { id: item.id });
          }
        }}
      >
        <View style={styles.cardBody}>
          <View style={{ flex: 1 }}>
            <Text style={[styles.cardName, { color: colors.foreground }]} numberOfLines={1}>{item.name}</Text>
            {!!muscles && <Text style={[styles.cardMuscles, { color: colors.mutedForeground }]} numberOfLines={1}>{muscles}</Text>}
          </View>
          {!!difficultyLabel && (
            <View style={[styles.diffBadge, { backgroundColor: diffColor + '20' }]}>
              <Text style={[styles.diffText, { color: diffColor }]}>{difficultyLabel}</Text>
            </View>
          )}
          {item.isCompound && (
            <View style={[styles.compoundBadge, { backgroundColor: colors.background, borderColor: colors.border }]}>
              <Text style={[styles.compoundText, { color: colors.mutedForeground }]}>Złożone</Text>
            </View>
          )}
          {canDelete && (
            <Pressable
              style={styles.deleteButton}
              onPress={() => handleDeleteExercise(item.id, item.name)}
            >
              <Icon name="trash-2" size={18} color="#ef4444" />
            </Pressable>
          )}
        </View>
      </Pressable>
    );
  };

  return (
    <View style={[styles.container, { backgroundColor: colors.background }]}>
      {/* Header */}
      <View style={[styles.header, { backgroundColor: colors.background, paddingTop: insets.top + Spacing.sm }]}>
        <BackButton
          onPress={() => mode === 'pick' && navigation.getState().index > 0 ? navigation.goBack() : navigation.navigate('TrainingHub')}
          style={backButtonSpacing}
        />
        <View style={styles.headerTop}>
          <View>
            <Text style={[styles.title, { color: colors.foreground }]}>{mode === 'pick' ? 'Wybierz ćwiczenie' : 'Ćwiczenia'}</Text>
            <Text style={[styles.subtitle, { color: colors.mutedForeground }]}>{exercises.length} dostępnych</Text>
          </View>
          <View style={styles.headerActions}>
            <Pressable style={[styles.iconButton, { backgroundColor: colors.card, borderColor: colors.border }]} onPress={() => setShowFilter(true)}>
              <Icon name="filter" size={20} color={selectedGroupIds.length > 0 ? '#3b82f6' : colors.foreground} />
            </Pressable>
            {mode !== 'pick' && (
              <Pressable style={[styles.iconButton, styles.iconButtonPrimary]} onPress={() => navigation.navigate('ExerciseCreate')}>
                <Icon name="plus" size={20} color="#fff" />
              </Pressable>
            )}
          </View>
        </View>

        <View style={[styles.searchContainer, { backgroundColor: colors.card, borderColor: colors.border }]}>
          <Icon name="search" size={18} color={colors.mutedForeground} style={styles.searchIcon} />
          <TextInput
            style={[styles.searchInput, { color: colors.foreground }]}
            placeholder="Szukaj ćwiczenia..."
            value={searchQuery}
            onChangeText={onSearch}
            placeholderTextColor={colors.mutedForeground}
          />
          {searchQuery.length > 0 && (
            <Pressable onPress={() => onSearch('')}>
              <Icon name="x" size={16} color={colors.mutedForeground} />
            </Pressable>
          )}
        </View>
      </View>

      {isLoading ? (
        <View style={styles.centered}>
          <ActivityIndicator size="large" color="#3b82f6" />
        </View>
      ) : (
        <FlatList
          data={exercises}
          keyExtractor={item => String(item.id)}
          renderItem={renderItem}
          contentContainerStyle={[styles.listContent, { paddingBottom: insets.bottom + Spacing.xl }]}
          onEndReached={onEndReached}
          onEndReachedThreshold={0.3}
          ListFooterComponent={isLoadingMore ? <ActivityIndicator style={{ margin: 16 }} color="#3b82f6" /> : null}
          ListEmptyComponent={
            <View style={styles.emptyState}>
              <Icon name="search" size={40} color={colors.mutedForeground} />
              <Text style={[styles.emptyTitle, { color: colors.foreground }]}>Brak ćwiczeń</Text>
              <Text style={[styles.emptyDesc, { color: colors.mutedForeground }]}>Dodaj pierwsze ćwiczenie lub zmień filtry</Text>
            </View>
          }
        />
      )}

      {/* Filter Modal */}
      <Modal visible={showFilter} transparent animationType="slide" onRequestClose={() => setShowFilter(false)}>
        <View style={styles.modalOverlay}>
          <Pressable style={styles.modalDismiss} onPress={() => setShowFilter(false)} />
          <View style={[styles.modalSheet, { backgroundColor: colors.card, paddingBottom: insets.bottom + Spacing.lg }]}>
            <View style={styles.modalHandle} />
            <View style={styles.modalHeader}>
              <Text style={[styles.modalTitle, { color: colors.foreground }]}>Filtruj ćwiczenia</Text>
              <Pressable onPress={() => {
                const cleared: number[] = [];
                setSelectedGroupIds(cleared);
                groupIdsRef.current = cleared;
                setShowFilter(false);
                loadExercises(1, true, searchRef.current, undefined);
              }}>
                <Text style={{ color: '#ef4444', fontSize: 14 }}>Wyczyść</Text>
              </Pressable>
            </View>

            <Text style={[styles.filterLabel, { color: colors.mutedForeground }]}>Partie mięśniowe</Text>
            <View style={styles.chipsContainer}>
              {muscleGroups.map(mg => {
                const active = selectedGroupIds.includes(mg.id);
                return (
                  <Pressable
                    key={mg.id}
                    style={[styles.chip, { borderColor: active ? '#3b82f6' : colors.border, backgroundColor: active ? '#3b82f620' : colors.background }]}
                    onPress={() => toggleGroup(mg.id)}
                  >
                    {active && <Icon name="check" size={12} color="#3b82f6" style={{ marginRight: 4 }} />}
                    <Text style={[styles.chipText, { color: active ? '#3b82f6' : colors.foreground }]}>{mg.name}</Text>
                  </Pressable>
                );
              })}
            </View>

            <Pressable style={styles.applyBtn} onPress={() => applyFilters()}>
              <Text style={styles.applyBtnText}>Zastosuj filtry</Text>
            </Pressable>
          </View>
        </View>
      </Modal>
    </View>
  );
}

