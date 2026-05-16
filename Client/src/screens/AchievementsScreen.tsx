import React, { useEffect, useState } from 'react';
import { ActivityIndicator, ScrollView, StyleSheet, Text, View } from 'react-native';
import { useSafeAreaInsets } from 'react-native-safe-area-context';
import Icon from 'react-native-vector-icons/Feather';
import { AchievementApiService } from '../api/AchievementApiService';
import BackButton, { backButtonSpacing } from '../components/BackButton';
import { useTheme } from '../context/ThemeContext';
import { Spacing } from '../theme/theme';

type UIAchievement = {
  id: number;
  name: string;
  description: string;
  icon: string;
  unlocked: boolean;
  unlockedAt?: string;
};

export default function AchievementsScreen({ navigation }: any) {
  const { colors } = useTheme();
  const insets = useSafeAreaInsets();
  const [achievements, setAchievements] = useState<UIAchievement[]>([]);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    loadAchievements();
  }, []);

  const loadAchievements = async () => {
    try {
      const [allAchievements, myAchievements] = await Promise.all([
        AchievementApiService.getAllAchievements(),
        AchievementApiService.getMyAchievements(),
      ]);

      const unlockedById = new Map(myAchievements.map(item => [item.achievement.id, item]));
      const merged = allAchievements.map(item => ({
        id: item.id,
        name: item.name,
        description: item.description,
        icon: item.iconCode || 'award',
        unlocked: unlockedById.has(item.id),
        unlockedAt: unlockedById.get(item.id)?.unlockedAt,
      }));

      setAchievements(merged.sort((a, b) => Number(b.unlocked) - Number(a.unlocked)));
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <View style={[styles.container, { backgroundColor: colors.background }]}>
      <View style={[styles.header, { paddingTop: insets.top + Spacing.sm }]}>
        <BackButton onPress={() => navigation.goBack()} style={backButtonSpacing} />
        <Text style={[styles.title, { color: colors.foreground }]}>Wszystkie osiągnięcia</Text>
        <Text style={[styles.subtitle, { color: colors.mutedForeground }]}>Odblokowane i te, które nadal czekają</Text>
      </View>

      {isLoading ? (
        <View style={styles.centered}>
          <ActivityIndicator size="large" color="#2563eb" />
        </View>
      ) : (
        <ScrollView contentContainerStyle={[styles.content, { paddingBottom: insets.bottom + Spacing.xl }]}>
          {achievements.map(item => (
            <View key={item.id} style={[styles.card, { backgroundColor: item.unlocked ? '#2563eb' : colors.card, borderColor: item.unlocked ? '#fbbf24' : colors.border }]}>
              {item.unlocked && (
                <View style={styles.medalBadge}>
                  <Icon name="award" size={15} color="#78350f" />
                </View>
              )}
              <View style={[styles.iconWrapper, { backgroundColor: item.unlocked ? 'rgba(255,255,255,0.18)' : colors.muted }]}>
                <Icon name={item.icon as any} size={22} color={item.unlocked ? '#fff' : colors.mutedForeground} />
              </View>
              <View style={{ flex: 1 }}>
                <Text style={[styles.cardTitle, { color: item.unlocked ? '#fff' : colors.foreground }]}>{item.name}</Text>
                <Text style={[styles.cardDesc, { color: item.unlocked ? 'rgba(255,255,255,0.85)' : colors.mutedForeground }]}>{item.description}</Text>
                {item.unlockedAt && (
                  <Text style={styles.unlockedAt}>Zdobyto: {new Date(item.unlockedAt).toLocaleDateString('pl-PL')}</Text>
                )}
              </View>
            </View>
          ))}
        </ScrollView>
      )}
    </View>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1 },
  header: { paddingHorizontal: Spacing.lg, paddingBottom: Spacing.md },
  title: { fontSize: 28, fontWeight: '800' },
  subtitle: { fontSize: 14, marginTop: 4 },
  centered: { flex: 1, alignItems: 'center', justifyContent: 'center' },
  content: { paddingHorizontal: Spacing.lg, paddingTop: Spacing.sm },
  card: { flexDirection: 'row', borderRadius: 16, borderWidth: 1, padding: Spacing.md, marginBottom: Spacing.sm, position: 'relative', overflow: 'hidden' },
  medalBadge: {
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
    borderColor: 'rgba(255,255,255,0.7)',
    zIndex: 1,
  },
  iconWrapper: { width: 44, height: 44, borderRadius: 12, alignItems: 'center', justifyContent: 'center', marginRight: Spacing.md },
  cardTitle: { fontSize: 15, fontWeight: '800', marginBottom: 4 },
  cardDesc: { fontSize: 13, lineHeight: 18 },
  unlockedAt: { color: 'rgba(255,255,255,0.78)', fontSize: 12, marginTop: Spacing.xs },
});
