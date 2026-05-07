// src/screens/ProfileScreen.tsx
import React from 'react';
import { View, Text, StyleSheet, ScrollView, Image, Pressable } from 'react-native';
import Icon from 'react-native-vector-icons/Feather';
import { Colors, Spacing } from '../theme/theme';

const stats = [
  { label: "Treningi w tym miesiącu", value: "24", icon: "calendar" },
  { label: "Średni czas treningu", value: "48 min", icon: "trending-up" },
  { label: "Osiągnięcia", value: "12", icon: "award" },
];

const achievements = [
  { name: "100 treningów", description: "Ukończono 100 sesji", unlocked: true },
  { name: "Konsystencja", description: "30 dni z rzędu", unlocked: true },
  { name: "Motywator", description: "Zaprosiłeś 5 znajomych", unlocked: true },
  { name: "Marathon", description: "200 treningów", unlocked: false },
];

const posts = [
  { id: 1, title: "Poranny bieg wokół jeziora", date: "2 dni temu", likes: 14 },
  { id: 2, title: "Nowy rekord w martwym ciągu!", date: "5 dni temu", likes: 32 },
];

export default function ProfileScreen() {
  const logout = () => {
    // Auth context logout placeholder
  };

  return (
    <ScrollView style={styles.container} contentContainerStyle={styles.scrollContent}>
      <View style={styles.profileHeader}>
        <Image
          source={{ uri: "https://images.unsplash.com/photo-1613145997970-db84a7975fbb?crop=entropy&cs=tinysrgb&fit=max&fm=jpg&q=80&w=200" }}
          style={styles.profileImage}
        />
        <Text style={styles.userName}>Jan Kowalski</Text>
        <Text style={styles.userEmail}>jan.kowalski@email.com</Text>
      </View>

      <View style={styles.statsContainer}>
        {stats.map((stat, i) => (
          <View key={i} style={styles.statCard}>
            <View>
              <Text style={styles.statLabel}>{stat.label}</Text>
              <Text style={styles.statValue}>{stat.value}</Text>
            </View>
            <View style={styles.statIconWrapper}>
              <Icon name={stat.icon} size={28} color={Colors.primary} />
            </View>
          </View>
        ))}
      </View>

      <Text style={styles.sectionTitle}>Twoje posty</Text>
      <View style={styles.postsContainer}>
        {posts.map(post => (
          <View key={post.id} style={styles.postCard}>
            <View style={styles.postHeader}>
              <Text style={styles.postTitle}>{post.title}</Text>
              <Text style={styles.postDate}>{post.date}</Text>
            </View>
            <View style={styles.postActions}>
              <View style={styles.actionItem}>
                <Icon name="heart" size={16} color={Colors.destructive} />
                <Text style={styles.actionText}>{post.likes}</Text>
              </View>
              <View style={styles.actionItem}>
                <Icon name="message-square" size={16} color={Colors.mutedForeground} />
                <Text style={styles.actionText}>Komentarze</Text>
              </View>
            </View>
          </View>
        ))}
      </View>

      <View style={styles.quickActionsContainer}>
        <Pressable style={styles.quickActionCard}>
          <View style={styles.quickActionIconWrapper}>
            <Icon name="activity" size={20} color={Colors.primary} />
          </View>
          <Text style={styles.quickActionTitle}>Twoja dieta</Text>
          <View style={styles.quickActionFooter}>
            <Text style={styles.quickActionSubtitle}>Zarządzaj planem</Text>
            <Icon name="chevron-right" size={12} color={Colors.mutedForeground} />
          </View>
        </Pressable>
        <Pressable style={styles.quickActionCard}>
          <View style={styles.quickActionIconWrapper}>
            <Icon name="clipboard" size={20} color={Colors.primary} />
          </View>
          <Text style={styles.quickActionTitle}>Twoje ćwiczenia</Text>
          <View style={styles.quickActionFooter}>
            <Text style={styles.quickActionSubtitle}>Własna baza</Text>
            <Icon name="chevron-right" size={12} color={Colors.mutedForeground} />
          </View>
        </Pressable>
      </View>

      <Text style={styles.sectionTitle}>Osiągnięcia</Text>
      <View style={styles.achievementsContainer}>
        {achievements.map((ach, i) => (
          <View key={i} style={[styles.achievementCard, ach.unlocked ? styles.achievementUnlocked : styles.achievementLocked]}>
            <View style={[styles.achievementIconWrapper, ach.unlocked ? styles.iconUnlocked : styles.iconLocked]}>
              <Icon name="award" size={24} color={ach.unlocked ? '#fff' : Colors.mutedForeground} />
            </View>
            <Text style={[styles.achievementTitle, ach.unlocked ? styles.textUnlocked : styles.textLocked]}>{ach.name}</Text>
            <Text style={[styles.achievementDesc, ach.unlocked ? styles.descUnlocked : styles.descLocked]}>{ach.description}</Text>
          </View>
        ))}
      </View>
      <Pressable style={styles.moreButton}>
        <Text style={styles.moreButtonText}>Zobacz więcej osiągnięć</Text>
      </Pressable>

      <Pressable style={styles.logoutButton} onPress={logout}>
        <Icon name="log-out" size={20} color="#fff" />
        <Text style={styles.logoutButtonText}>Wyloguj się</Text>
      </Pressable>
    </ScrollView>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1, backgroundColor: Colors.background },
  scrollContent: { padding: Spacing.md, paddingBottom: Spacing.xl },
  profileHeader: { alignItems: 'center', marginVertical: Spacing.lg },
  profileImage: { width: 96, height: 96, borderRadius: 48, borderWidth: 4, borderColor: '#2563eb', marginBottom: Spacing.md },
  userName: { fontSize: 20, fontWeight: '600', color: Colors.foreground },
  userEmail: { fontSize: 14, color: Colors.mutedForeground, marginTop: 4 },
  statsContainer: { marginBottom: Spacing.lg },
  statCard: { backgroundColor: Colors.secondary, padding: Spacing.md, borderRadius: 16, flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center', marginBottom: Spacing.sm, borderWidth: 1, borderColor: Colors.border },
  statLabel: { fontSize: 14, color: Colors.mutedForeground, marginBottom: 6 },
  statValue: { fontSize: 28, fontWeight: '600', color: Colors.foreground },
  statIconWrapper: { width: 56, height: 56, backgroundColor: '#eff6ff', borderRadius: 16, alignItems: 'center', justifyContent: 'center' },
  sectionTitle: { fontSize: 18, fontWeight: '600', marginBottom: Spacing.md, color: Colors.foreground },
  postsContainer: { marginBottom: Spacing.lg },
  postCard: { backgroundColor: Colors.secondary, padding: Spacing.md, borderRadius: 16, marginBottom: Spacing.sm, borderWidth: 1, borderColor: Colors.border },
  postHeader: { flexDirection: 'row', justifyContent: 'space-between', marginBottom: Spacing.sm },
  postTitle: { fontSize: 14, fontWeight: '500', color: Colors.foreground },
  postDate: { fontSize: 12, color: Colors.mutedForeground },
  postActions: { flexDirection: 'row' },
  actionItem: { flexDirection: 'row', alignItems: 'center', marginRight: Spacing.md },
  actionText: { fontSize: 12, color: Colors.mutedForeground, marginLeft: 6 },
  quickActionsContainer: { flexDirection: 'row', justifyContent: 'space-between', marginBottom: Spacing.lg },
  quickActionCard: { flex: 1, backgroundColor: Colors.secondary, padding: Spacing.md, borderRadius: 16, borderWidth: 1, borderColor: Colors.border, marginHorizontal: Spacing.xs },
  quickActionIconWrapper: { width: 40, height: 40, backgroundColor: '#eff6ff', borderRadius: 12, alignItems: 'center', justifyContent: 'center', marginBottom: Spacing.md },
  quickActionTitle: { fontSize: 14, fontWeight: '500', color: Colors.foreground, marginBottom: 4 },
  quickActionFooter: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center' },
  quickActionSubtitle: { fontSize: 12, color: Colors.mutedForeground },
  achievementsContainer: { flexDirection: 'row', flexWrap: 'wrap', justifyContent: 'space-between', marginBottom: Spacing.sm },
  achievementCard: { width: '48%', padding: Spacing.md, borderRadius: 16, marginBottom: Spacing.sm },
  achievementUnlocked: { backgroundColor: '#3b82f6' },
  achievementLocked: { backgroundColor: Colors.secondary, borderWidth: 1, borderColor: Colors.border },
  achievementIconWrapper: { width: 40, height: 40, borderRadius: 12, alignItems: 'center', justifyContent: 'center', marginBottom: Spacing.md },
  iconUnlocked: { backgroundColor: 'rgba(255,255,255,0.2)' },
  iconLocked: { backgroundColor: Colors.muted },
  achievementTitle: { fontSize: 14, fontWeight: '600', marginBottom: 4 },
  textUnlocked: { color: '#fff' },
  textLocked: { color: Colors.foreground },
  achievementDesc: { fontSize: 12 },
  descUnlocked: { color: 'rgba(255,255,255,0.9)' },
  descLocked: { color: Colors.mutedForeground },
  moreButton: { height: 48, backgroundColor: Colors.secondary, borderRadius: 12, alignItems: 'center', justifyContent: 'center', borderWidth: 1, borderColor: Colors.border, marginBottom: Spacing.lg },
  moreButtonText: { fontSize: 14, fontWeight: '500', color: Colors.foreground },
  logoutButton: { height: 48, backgroundColor: Colors.destructive, borderRadius: 12, alignItems: 'center', justifyContent: 'center', flexDirection: 'row' },
  logoutButtonText: { fontSize: 16, fontWeight: '500', color: '#fff', marginLeft: 8 },
});
