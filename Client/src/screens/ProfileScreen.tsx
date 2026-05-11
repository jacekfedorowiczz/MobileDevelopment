// src/screens/ProfileScreen.tsx
import React, { useState } from 'react';
import { View, Text, StyleSheet, ScrollView, Image, Pressable, Modal, TouchableWithoutFeedback, Alert } from 'react-native';
import Icon from 'react-native-vector-icons/Feather';
import { Spacing } from '../theme/theme';
import { useAuth } from '../context/AuthContext';
import { useTheme } from '../context/ThemeContext';
import { launchCamera, launchImageLibrary, ImagePickerResponse } from 'react-native-image-picker';

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

export default function ProfileScreen() {
  const { logout } = useAuth();
  const { colors, isDark } = useTheme();
  const [profileImage, setProfileImage] = useState("https://images.unsplash.com/photo-1613145997970-db84a7975fbb?crop=entropy&cs=tinysrgb&fit=max&fm=jpg&q=80&w=200");
  const [modalVisible, setModalVisible] = useState(false);

  const handleImagePickerResponse = (response: ImagePickerResponse) => {
    if (response.didCancel) return;
    if (response.errorCode) {
      Alert.alert('Błąd', response.errorMessage || 'Wystąpił błąd podczas wybierania zdjęcia');
      return;
    }
    if (response.assets && response.assets.length > 0) {
      const uri = response.assets[0].uri;
      if (uri) {
        setProfileImage(uri);
        // Tu docelowo wyślesz zdjęcie na backend używając np. FormData
        console.log('Nowe zdjęcie gotowe do wysyłki:', uri);
      }
    }
    setModalVisible(false);
  };

  const openCamera = () => {
    launchCamera({ mediaType: 'photo', quality: 0.8 }, handleImagePickerResponse);
  };

  const openGallery = () => {
    launchImageLibrary({ mediaType: 'photo', quality: 0.8 }, handleImagePickerResponse);
  };

  return (
    <ScrollView style={[styles.container, { backgroundColor: colors.background }]} contentContainerStyle={styles.scrollContent}>
      <View style={styles.profileHeader}>
        <View style={styles.imageContainer}>
          <Image
            source={{ uri: profileImage }}
            style={[styles.profileImage, { borderColor: '#2563eb' }]}
          />
          <Pressable style={styles.editImageBadge} onPress={() => setModalVisible(true)}>
            <Icon name="camera" size={16} color="#fff" />
          </Pressable>
        </View>
        <Text style={[styles.userName, { color: colors.foreground }]}>Jan Kowalski</Text>
        <Text style={[styles.userEmail, { color: colors.mutedForeground }]}>jan.kowalski@email.com</Text>
      </View>

      <View style={styles.statsContainer}>
        {stats.map((stat, i) => (
          <View key={i} style={[styles.statCard, { backgroundColor: colors.secondary, borderColor: colors.border }]}>
            <View>
              <Text style={[styles.statLabel, { color: colors.mutedForeground }]}>{stat.label}</Text>
              <Text style={[styles.statValue, { color: colors.foreground }]}>{stat.value}</Text>
            </View>
            <View style={[styles.statIconWrapper, { backgroundColor: isDark ? '#1e293b' : '#eff6ff' }]}>
              <Icon name={stat.icon} size={28} color="#2563eb" />
            </View>
          </View>
        ))}
      </View>

      <View style={styles.quickActionsContainer}>
        <Pressable style={[styles.quickActionCard, { backgroundColor: colors.secondary, borderColor: colors.border }]}>
          <View style={[styles.quickActionIconWrapper, { backgroundColor: isDark ? '#1e293b' : '#eff6ff' }]}>
            <Icon name="activity" size={20} color="#2563eb" />
          </View>
          <Text style={[styles.quickActionTitle, { color: colors.foreground }]}>Twoja dieta</Text>
          <View style={styles.quickActionFooter}>
            <Text style={[styles.quickActionSubtitle, { color: colors.mutedForeground }]}>Zarządzaj planem</Text>
            <Icon name="chevron-right" size={12} color={colors.mutedForeground} />
          </View>
        </Pressable>
        <Pressable style={[styles.quickActionCard, { backgroundColor: colors.secondary, borderColor: colors.border }]}>
          <View style={[styles.quickActionIconWrapper, { backgroundColor: isDark ? '#1e293b' : '#eff6ff' }]}>
            <Icon name="clipboard" size={20} color="#2563eb" />
          </View>
          <Text style={[styles.quickActionTitle, { color: colors.foreground }]}>Twoje ćwiczenia</Text>
          <View style={styles.quickActionFooter}>
            <Text style={[styles.quickActionSubtitle, { color: colors.mutedForeground }]}>Własna baza</Text>
            <Icon name="chevron-right" size={12} color={colors.mutedForeground} />
          </View>
        </Pressable>
      </View>

      <Text style={[styles.sectionTitle, { color: colors.foreground }]}>Osiągnięcia</Text>
      <View style={styles.achievementsContainer}>
        {achievements.map((ach, i) => (
          <View key={i} style={[styles.achievementCard, ach.unlocked ? styles.achievementUnlocked : [styles.achievementLocked, { backgroundColor: colors.secondary, borderColor: colors.border }]]}>
            <View style={[styles.achievementIconWrapper, ach.unlocked ? styles.iconUnlocked : [styles.iconLocked, { backgroundColor: colors.muted }]]}>
              <Icon name="award" size={24} color={ach.unlocked ? '#fff' : colors.mutedForeground} />
            </View>
            <Text style={[styles.achievementTitle, { color: ach.unlocked ? '#fff' : colors.foreground }]}>{ach.name}</Text>
            <Text style={[styles.achievementDesc, { color: ach.unlocked ? 'rgba(255,255,255,0.9)' : colors.mutedForeground }]}>{ach.description}</Text>
          </View>
        ))}
      </View>

      <Pressable style={styles.logoutButton} onPress={logout}>
        <Icon name="log-out" size={20} color="#fff" />
        <Text style={styles.logoutButtonText}>Wyloguj się</Text>
      </Pressable>

      <Modal
        animationType="slide"
        transparent={true}
        visible={modalVisible}
        onRequestClose={() => setModalVisible(false)}
      >
        <TouchableWithoutFeedback onPress={() => setModalVisible(false)}>
          <View style={styles.modalOverlay}>
            <TouchableWithoutFeedback>
              <View style={[styles.modalContent, { backgroundColor: colors.card }]}>
                <View style={[styles.modalHeader, { borderBottomColor: colors.border }]}>
                  <Text style={[styles.modalTitle, { color: colors.foreground }]}>Zmień zdjęcie profilowe</Text>
                </View>
                
                <Pressable style={styles.modalOption} onPress={openCamera}>
                  <View style={[styles.optionIcon, { backgroundColor: '#eff6ff' }]}>
                    <Icon name="camera" size={20} color="#2563eb" />
                  </View>
                  <Text style={[styles.optionText, { color: colors.foreground }]}>Zrób zdjęcie</Text>
                </Pressable>

                <Pressable style={styles.modalOption} onPress={openGallery}>
                  <View style={[styles.optionIcon, { backgroundColor: '#f0fdf4' }]}>
                    <Icon name="image" size={20} color="#22c55e" />
                  </View>
                  <Text style={[styles.optionText, { color: colors.foreground }]}>Wybierz z galerii</Text>
                </Pressable>

                <Pressable style={[styles.modalOption, styles.cancelOption]} onPress={() => setModalVisible(false)}>
                  <Text style={styles.cancelText}>Anuluj</Text>
                </Pressable>
              </View>
            </TouchableWithoutFeedback>
          </View>
        </TouchableWithoutFeedback>
      </Modal>
    </ScrollView>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1 },
  scrollContent: { padding: Spacing.md, paddingBottom: Spacing.xl },
  profileHeader: { alignItems: 'center', marginVertical: Spacing.lg },
  imageContainer: { position: 'relative' },
  profileImage: { width: 100, height: 100, borderRadius: 50, borderWidth: 3 },
  editImageBadge: { position: 'absolute', bottom: 0, right: 0, backgroundColor: '#2563eb', width: 32, height: 32, borderRadius: 16, alignItems: 'center', justifyContent: 'center', borderWidth: 3, borderColor: '#fff' },
  userName: { fontSize: 22, fontWeight: '700', marginTop: Spacing.md },
  userEmail: { fontSize: 14, marginTop: 4 },
  statsContainer: { marginBottom: Spacing.lg },
  statCard: { padding: Spacing.md, borderRadius: 16, flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center', marginBottom: Spacing.sm, borderWidth: 1 },
  statLabel: { fontSize: 13, marginBottom: 4 },
  statValue: { fontSize: 24, fontWeight: '700' },
  statIconWrapper: { width: 52, height: 52, borderRadius: 12, alignItems: 'center', justifyContent: 'center' },
  sectionTitle: { fontSize: 18, fontWeight: '700', marginBottom: Spacing.md },
  quickActionsContainer: { flexDirection: 'row', justifyContent: 'space-between', marginBottom: Spacing.lg },
  quickActionCard: { flex: 1, padding: Spacing.md, borderRadius: 16, borderWidth: 1, marginHorizontal: Spacing.xs },
  quickActionIconWrapper: { width: 40, height: 40, borderRadius: 10, alignItems: 'center', justifyContent: 'center', marginBottom: Spacing.md },
  quickActionTitle: { fontSize: 14, fontWeight: '600', marginBottom: 4 },
  quickActionFooter: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center' },
  quickActionSubtitle: { fontSize: 12 },
  achievementsContainer: { flexDirection: 'row', flexWrap: 'wrap', justifyContent: 'space-between', marginBottom: Spacing.md },
  achievementCard: { width: '48%', padding: Spacing.md, borderRadius: 16, marginBottom: Spacing.sm },
  achievementUnlocked: { backgroundColor: '#3b82f6' },
  achievementLocked: { borderWidth: 1 },
  achievementIconWrapper: { width: 40, height: 40, borderRadius: 10, alignItems: 'center', justifyContent: 'center', marginBottom: Spacing.md },
  iconUnlocked: { backgroundColor: 'rgba(255,255,255,0.2)' },
  iconLocked: { },
  achievementTitle: { fontSize: 14, fontWeight: '700', marginBottom: 4 },
  achievementDesc: { fontSize: 12 },
  logoutButton: { height: 52, backgroundColor: '#ef4444', borderRadius: 14, alignItems: 'center', justifyContent: 'center', flexDirection: 'row', marginTop: Spacing.md },
  logoutButtonText: { fontSize: 16, fontWeight: '700', color: '#fff', marginLeft: 10 },
  
  // Modal styles
  modalOverlay: { flex: 1, backgroundColor: 'rgba(0,0,0,0.5)', justifyContent: 'flex-end' },
  modalContent: { borderTopLeftRadius: 24, borderTopRightRadius: 24, padding: Spacing.lg, paddingBottom: 40 },
  modalHeader: { paddingBottom: Spacing.md, marginBottom: Spacing.md, borderBottomWidth: 1 },
  modalTitle: { fontSize: 18, fontWeight: '700', textAlign: 'center' },
  modalOption: { flexDirection: 'row', alignItems: 'center', paddingVertical: Spacing.md },
  optionIcon: { width: 44, height: 44, borderRadius: 22, alignItems: 'center', justifyContent: 'center', marginRight: Spacing.md },
  optionText: { fontSize: 16, fontWeight: '500' },
  cancelOption: { justifyContent: 'center', marginTop: Spacing.sm },
  cancelText: { fontSize: 16, color: '#ef4444', fontWeight: '600' },
});
