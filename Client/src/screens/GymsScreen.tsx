// src/screens/GymsScreen.tsx
import React, { useState, useEffect, useCallback, useRef } from 'react';
import { Alert, Linking, View, Text, StyleSheet, ScrollView, TextInput, Pressable, ActivityIndicator } from 'react-native';
import { useFocusEffect } from '@react-navigation/native';
import { useSafeAreaInsets } from 'react-native-safe-area-context';
import Icon from 'react-native-vector-icons/Feather';
import { Spacing } from '../theme/theme';
import { useTheme } from '../context/ThemeContext';
import { GymApiService, GymDto } from '../api/GymApiService';
import BackButton, { backButtonSpacing } from '../components/BackButton';

export default function GymsScreen({ navigation, route }: any) {
  const { colors } = useTheme();
  const [searchQuery, setSearchQuery] = useState("");
  const [gyms, setGyms] = useState<GymDto[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const insets = useSafeAreaInsets();

  const debounceTimer = useRef<ReturnType<typeof setTimeout> | null>(null);
  const handledCreatedMessageRef = useRef<string | null>(null);

  const loadGyms = useCallback(async (search?: string) => {
    try {
      setIsLoading(true);
      const data = await GymApiService.getAllGyms(search);
      setGyms(Array.isArray(data) ? data : []);
    } catch (err) {
      console.error(err);
      setGyms([]);
    } finally {
      setIsLoading(false);
    }
  }, []);

  useFocusEffect(
    useCallback(() => {
      loadGyms(searchQuery);
    }, [loadGyms, searchQuery])
  );

  useEffect(() => {
    const message = route?.params?.gymCreatedMessage;
    if (!message || handledCreatedMessageRef.current === message) {
      return;
    }

    handledCreatedMessageRef.current = message;
    Alert.alert('Zapisano', message);
    navigation.setParams({ gymCreatedMessage: undefined });
  }, [navigation, route?.params?.gymCreatedMessage]);

  // Debounce search
  useEffect(() => {
    if (debounceTimer.current) clearTimeout(debounceTimer.current);
    debounceTimer.current = setTimeout(() => {
      loadGyms(searchQuery);
    }, 400);

    return () => {
      if (debounceTimer.current) clearTimeout(debounceTimer.current);
    };
  }, [loadGyms, searchQuery]);

  // We still keep a local filter for extra smoothness, 
  // but the source 'gyms' will now be filtered by the server too.
  const filteredGyms = gyms;

  const hasCoordinates = (gym: GymDto) => {
    return Number.isFinite(gym.latitude) && Number.isFinite(gym.longitude)
      && gym.latitude !== 0 && gym.longitude !== 0;
  };

  const openMaps = async (gym: GymDto) => {
    if (!hasCoordinates(gym)) return;

    const query = encodeURIComponent(`${gym.latitude},${gym.longitude}`);
    const url = `https://www.google.com/maps/dir/?api=1&destination=${query}&travelmode=driving`;

    try {
      await Linking.openURL(url);
    } catch {
      Alert.alert('Błąd', 'Nie udało się otworzyć Google Maps.');
    }
  };

  return (
    <View style={[styles.container, { backgroundColor: colors.background }]}>
      <View style={[styles.header, { backgroundColor: colors.background, paddingTop: insets.top + Spacing.sm }]}>
        <BackButton onPress={() => navigation.navigate('TrainingHub')} style={backButtonSpacing} />
        <View style={styles.titleRow}>
          <View style={{ flex: 1 }}>
            <Text style={[styles.title, { color: colors.foreground }]}>Siłownie w okolicy</Text>
            <View style={styles.locationRow}>
              <Icon name="map-pin" size={16} color="#3b82f6" />
              <Text style={[styles.locationText, { color: colors.mutedForeground }]}>Twoja lokalizacja: Warszawa Centrum</Text>
            </View>
          </View>
          <Pressable style={[styles.addButton, { backgroundColor: '#3b82f6' }]} onPress={() => navigation.navigate('GymCreate')}>
            <Icon name="plus" size={20} color="#fff" />
          </Pressable>
        </View>

        <View style={[styles.searchContainer, { backgroundColor: colors.card, borderColor: colors.border }]}>
          <Icon name="search" size={20} color={colors.mutedForeground} style={styles.searchIcon} />
          <TextInput
            style={[styles.searchInput, { color: colors.foreground }]}
            placeholder="Wyszukaj klub albo park..."
            value={searchQuery}
            onChangeText={setSearchQuery}
            placeholderTextColor={colors.mutedForeground}
          />
        </View>
      </View>

      {isLoading ? (
        <View style={{ flex: 1, justifyContent: 'center', alignItems: 'center' }}>
          <ActivityIndicator size="large" color="#3b82f6" />
        </View>
      ) : (
        <ScrollView style={styles.content} contentContainerStyle={styles.scrollContent}>
          {filteredGyms.length === 0 ? (
            <View style={{ marginTop: 40, alignItems: 'center' }}>
              <Text style={{ color: colors.mutedForeground }}>Brak wyników wyszukiwania.</Text>
            </View>
          ) : (
            filteredGyms.map((gym, idx) => {
              const canNavigate = hasCoordinates(gym);

              return (
                <Pressable key={gym.id || idx} style={[styles.gymCard, { backgroundColor: colors.card, borderColor: colors.border }]}>
                  <View style={styles.gymHeader}>
                    <View style={styles.gymInfo}>
                      <Text style={[styles.gymName, { color: colors.foreground }]}>{gym.name}</Text>
                      {!gym.isActive && (
                        <Text style={styles.pendingText}>Oczekuje na aktywację</Text>
                      )}
                      <View style={styles.addressRow}>
                        <Icon name="map-pin" size={12} color={colors.mutedForeground} />
                        <Text style={[styles.addressText, { color: colors.mutedForeground }]}>
                          {`${gym.street || ''}${gym.street ? ', ' : ''}${gym.zipCode || ''} ${gym.city || ''}`}
                        </Text>
                      </View>
                    </View>
                    <View style={[styles.ratingBadge, { backgroundColor: colors.background, borderColor: colors.border }]}>
                      <Icon name="star" size={12} color="#f59e0b" />
                      <Text style={[styles.ratingText, { color: colors.foreground }]}>{gym.rating}</Text>
                    </View>
                  </View>

                  {canNavigate && (
                    <View style={[styles.gymFooter, { borderColor: colors.border }]}>
                      <Pressable style={[styles.navButton, { backgroundColor: colors.foreground }]} onPress={() => openMaps(gym)}>
                        <Icon name="navigation" size={14} color={colors.background} />
                        <Text style={[styles.navButtonText, { color: colors.background }]}>Nawiguj</Text>
                      </Pressable>
                    </View>
                  )}
                </Pressable>
              );
            })
          )}
        </ScrollView>
      )}
    </View>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1 },
  header: { paddingHorizontal: Spacing.lg, paddingBottom: Spacing.md },
  titleRow: { flexDirection: 'row', alignItems: 'flex-start', justifyContent: 'space-between', gap: Spacing.md },
  title: { fontSize: 28, fontWeight: 'bold' },
  locationRow: { flexDirection: 'row', alignItems: 'center', marginTop: 8, marginBottom: Spacing.lg },
  locationText: { fontSize: 14, marginLeft: 6 },
  addButton: { width: 44, height: 44, borderRadius: 14, alignItems: 'center', justifyContent: 'center' },
  searchContainer: { flexDirection: 'row', alignItems: 'center', borderRadius: 12, borderWidth: 1, paddingHorizontal: Spacing.md, height: 48 },
  searchIcon: { marginRight: Spacing.sm },
  searchInput: { flex: 1, fontSize: 16 },
  content: { flex: 1, paddingHorizontal: Spacing.lg },
  scrollContent: { paddingBottom: Spacing.xl },
  gymCard: { padding: Spacing.md, borderRadius: 16, marginBottom: Spacing.md, borderWidth: 1 },
  gymHeader: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'flex-start', marginBottom: Spacing.md },
  gymInfo: { flex: 1, paddingRight: Spacing.sm },
  gymName: { fontSize: 16, fontWeight: '600', marginBottom: 4 },
  pendingText: { color: '#f59e0b', fontSize: 12, fontWeight: '600', marginBottom: 4 },
  addressRow: { flexDirection: 'row', alignItems: 'center' },
  addressText: { fontSize: 12, marginLeft: 4 },
  ratingBadge: { flexDirection: 'row', alignItems: 'center', paddingHorizontal: 8, paddingVertical: 4, borderRadius: 8, borderWidth: 1 },
  ratingText: { fontSize: 12, fontWeight: '600', marginLeft: 4 },
  gymFooter: { flexDirection: 'row', justifyContent: 'flex-end', alignItems: 'center', paddingTop: Spacing.md, borderTopWidth: 1 },
  navButton: { flexDirection: 'row', alignItems: 'center', paddingHorizontal: 16, paddingVertical: 8, borderRadius: 12 },
  navButtonText: { fontSize: 12, fontWeight: '500', marginLeft: 6 },
});
