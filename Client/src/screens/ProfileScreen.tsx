// src/screens/ProfileScreen.tsx
import React, { useState, useCallback } from 'react';
import { View, Text, ScrollView, Image, Pressable, Modal, TouchableWithoutFeedback, Alert, ActivityIndicator, TextInput, KeyboardAvoidingView, Platform } from 'react-native';
import { useFocusEffect, useNavigation } from '@react-navigation/native';
import { SafeAreaView } from 'react-native-safe-area-context';
import Icon from 'react-native-vector-icons/Feather';
import { useAuthStore } from '../store/useAuthStore';
import { useTheme } from '../context/ThemeContext';
import { launchCamera, launchImageLibrary, ImagePickerResponse } from 'react-native-image-picker';
import { ProfileService, ProfileData } from '../api/ProfileService';
import { AchievementApiService } from '../api/AchievementApiService';
import { getProfileStyles } from './ProfileScreen.styles';
import { Spacing } from '../theme/theme';
import { API_BASE_URL } from '../api/api-config';

interface UIAchievement {
  id: number;
  name: string;
  description: string;
  icon: string;
  unlocked: boolean;
  unlockedAt?: string;
}

export default function ProfileScreen() {
  const navigation = useNavigation<any>();
  const logout = useAuthStore(state => state.logout);
  const { colors, isDark, toggleTheme } = useTheme();
  const styles = getProfileStyles(colors, isDark);

  const [profileData, setProfileData] = useState<ProfileData | null>(null);
  const [achievements, setAchievements] = useState<UIAchievement[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [profileImage, setProfileImage] = useState("https://images.unsplash.com/photo-1613145997970-db84a7975fbb?crop=entropy&cs=tinysrgb&fit=max&fm=jpg&q=80&w=200");
  const [modalVisible, setModalVisible] = useState(false);
  const [editModalVisible, setEditModalVisible] = useState(false);
  const [isSavingEdit, setIsSavingEdit] = useState(false);

  // Edit form state
  const [editFirstName, setEditFirstName] = useState('');
  const [editLastName, setEditLastName] = useState('');
  const [editEmail, setEditEmail] = useState('');
  const [editWeight, setEditWeight] = useState('');
  const [editHeight, setEditHeight] = useState('');

  const fetchProfile = useCallback(async () => {
    setIsLoading(true);
    try {
      const [profileRes, allAchRes, myAchRes] = await Promise.all([
        ProfileService.getProfile(),
        AchievementApiService.getAllAchievements(),
        AchievementApiService.getMyAchievements()
      ]);

      if (profileRes.isSuccess && profileRes.value) {
        setProfileData(profileRes.value);
        if (profileRes.value.profileImageUrl) {
          let url = profileRes.value.profileImageUrl;
          if (!url.startsWith('http')) {
             const serverUrl = API_BASE_URL.replace('/api/v1/mobile', '');
             url = `${serverUrl}${url}`;
          }
          setProfileImage(url);
        }
      }

      const unlockedById = new Map(myAchRes.map(ma => [ma.achievement.id, ma]));
      const merged: UIAchievement[] = allAchRes.map(a => ({
        id: a.id,
        name: a.name,
        description: a.description,
        icon: a.iconCode || 'award',
        unlocked: unlockedById.has(a.id),
        unlockedAt: unlockedById.get(a.id)?.unlockedAt,
      }));
      setAchievements(getProfileAchievementsPreview(merged));

    } catch (e) {
      console.error('Error fetching profile data:', e);
    } finally {
      setIsLoading(false);
    }
  }, []);

  useFocusEffect(
    useCallback(() => {
      fetchProfile();
    }, [fetchProfile])
  );

  const openEditModal = () => {
    if (profileData) {
      setEditFirstName(profileData.firstName);
      setEditLastName(profileData.lastName);
      setEditEmail(profileData.email);
      setEditWeight(profileData.weight ? String(profileData.weight) : '');
      setEditHeight(profileData.height ? String(profileData.height) : '');
    }
    setEditModalVisible(true);
  };

  const handleSaveEdit = async () => {
    setIsSavingEdit(true);
    try {
      const response = await ProfileService.updateProfile({
        firstName: editFirstName,
        lastName: editLastName,
        email: editEmail,
        weight: editWeight ? parseFloat(editWeight) : undefined,
        height: editHeight ? parseFloat(editHeight) : undefined,
      });

      if (response.isSuccess) {
        setEditModalVisible(false);
        await fetchProfile();
      } else {
        Alert.alert('Błąd', response.errorMessage || 'Nie udało się zaktualizować profilu.');
      }
    } catch {
      Alert.alert('Błąd', 'Nie udało się zapisać zmian.');
    } finally {
      setIsSavingEdit(false);
    }
  };

  const handleLogout = async () => {
    const response = await ProfileService.logout();
    if (!response.isSuccess) {
      console.warn('Wylogowanie po stronie serwera się nie powiodło', response.errorMessage);
    }
    logout();
  };

  const handleImagePickerResponse = async (response: ImagePickerResponse) => {
    if (response.didCancel) return;
    if (response.errorCode) {
      Alert.alert('Błąd', response.errorMessage || 'Wystąpił błąd podczas wybierania zdjęcia');
      return;
    }
    if (response.assets && response.assets.length > 0) {
      const asset = response.assets[0];
      const uri = asset.uri;
      if (uri) {
        // Temporarily set the image locally so the user sees it immediately
        setProfileImage(uri);
        setModalVisible(false);

        // Upload to backend
        const fileName = asset.fileName || `avatar-${Date.now()}.jpg`;
        const type = asset.type || 'image/jpeg';
        
        setIsLoading(true);
        const uploadRes = await ProfileService.uploadAvatar(uri, fileName, type);
        if (!uploadRes.isSuccess) {
           Alert.alert('Błąd', uploadRes.errorMessage || 'Nie udało się wgrać zdjęcia');
           // Revert back or just re-fetch profile
           await fetchProfile();
        } else {
           // Successfully uploaded. Re-fetch to get the proper URL from server.
           await fetchProfile();
        }
        setIsLoading(false);
      }
    } else {
      setModalVisible(false);
    }
  };

  const openCamera = () => {
    launchCamera({ mediaType: 'photo', quality: 0.8 }, handleImagePickerResponse);
  };

  const openGallery = () => {
    launchImageLibrary({ mediaType: 'photo', quality: 0.8 }, handleImagePickerResponse);
  };

  return (
    <SafeAreaView style={styles.container}>
      <ScrollView contentContainerStyle={styles.scrollContent}>


        {isLoading ? (
          <ActivityIndicator size="large" color="#2563eb" style={{ marginTop: 40 }} />
        ) : (
          <>
            <View style={styles.profileHeader}>
              <View style={styles.imageContainer}>
                <Image
                  source={{ uri: profileImage }}
                  style={styles.profileImage}
                />
                <Pressable style={styles.editImageBadge} onPress={() => setModalVisible(true)}>
                  <Icon name="camera" size={16} color="#fff" />
                </Pressable>
              </View>
              <Text style={styles.userName}>{profileData?.firstName} {profileData?.lastName}</Text>
              <Text style={styles.userEmail}>{profileData?.email}</Text>
            </View>

            <View style={styles.statsContainer}>
              <View style={styles.statCard}>
                <View>
                  <Text style={styles.statLabel}>Treningi w tym miesiącu</Text>
                  <Text style={styles.statValue}>{profileData?.workoutsThisMonth ?? 0}</Text>
                </View>
                <View style={styles.statIconWrapper}>
                  <Icon name="calendar" size={28} color="#2563eb" />
                </View>
              </View>

              <View style={styles.statCard}>
                <View>
                  <Text style={styles.statLabel}>Średni czas treningu</Text>
                  <Text style={styles.statValue}>{profileData?.averageWorkoutTime ?? 0} min</Text>
                </View>
                <View style={styles.statIconWrapper}>
                  <Icon name="trending-up" size={28} color="#2563eb" />
                </View>
              </View>

              <View style={styles.statCard}>
                <View>
                  <Text style={styles.statLabel}>Osiągnięcia</Text>
                  <Text style={styles.statValue}>{profileData?.achievementsCount ?? 0}</Text>
                </View>
                <View style={styles.statIconWrapper}>
                  <Icon name="award" size={28} color="#2563eb" />
                </View>
              </View>
            </View>

            <View style={styles.quickActionsContainer}>
              <Pressable style={styles.quickActionCard} onPress={() => navigation.navigate('Training', { screen: 'Diet' })}>
                <View style={styles.quickActionIconWrapper}>
                  <Icon name="activity" size={20} color="#2563eb" />
                </View>
                <Text style={styles.quickActionTitle}>Twoja dieta</Text>
                <View style={styles.quickActionFooter}>
                  <Text style={styles.quickActionSubtitle}>Zarządzaj planem</Text>
                  <Icon name="chevron-right" size={12} color={colors.mutedForeground} />
                </View>
              </Pressable>
              <Pressable style={styles.quickActionCard} onPress={() => navigation.navigate('Training', { screen: 'Exercises' })}>
                <View style={styles.quickActionIconWrapper}>
                  <Icon name="clipboard" size={20} color="#2563eb" />
                </View>
                <Text style={styles.quickActionTitle}>Twoje ćwiczenia</Text>
                <View style={styles.quickActionFooter}>
                  <Text style={styles.quickActionSubtitle}>Własna baza</Text>
                  <Icon name="chevron-right" size={12} color={colors.mutedForeground} />
                </View>
              </Pressable>
            </View>

            <View style={styles.sectionHeaderRow}>
              <Text style={styles.sectionTitle}>Osiągnięcia</Text>
              <Pressable style={styles.viewAllButton} onPress={() => navigation.navigate('Achievements')}>
                <Text style={styles.viewAllText}>Zobacz wszystkie</Text>
                <Icon name="chevron-right" size={14} color="#2563eb" />
              </Pressable>
            </View>
            <View style={styles.achievementsContainer}>
              {achievements.map((ach, i) => (
                <View key={ach.id || i} style={[styles.achievementCard, ach.unlocked ? styles.achievementUnlocked : styles.achievementLocked]}>
                  {ach.unlocked && (
                    <View style={styles.achievementMedalBadge}>
                      <Icon name="award" size={15} color="#78350f" />
                    </View>
                  )}
                  <View style={[styles.achievementIconWrapper, ach.unlocked ? styles.iconUnlocked : styles.iconLocked]}>
                    <Icon name={ach.icon as any} size={24} color={ach.unlocked ? '#fff' : colors.mutedForeground} />
                  </View>
                  <Text style={ach.unlocked ? styles.achievementTitleUnlocked : styles.achievementTitleLocked}>{ach.name}</Text>
                  <Text style={ach.unlocked ? styles.achievementDescUnlocked : styles.achievementDescLocked}>{ach.description}</Text>
                </View>
              ))}
            </View>

            <Pressable style={[styles.logoutButton, { backgroundColor: colors.secondary, marginBottom: 12, borderWidth: 1, borderColor: colors.border, marginTop: Spacing.xl }]} onPress={openEditModal}>
              <Icon name="edit-2" size={20} color={colors.foreground} />
              <Text style={[styles.logoutButtonText, { color: colors.foreground }]}>Edytuj dane</Text>
            </Pressable>

            <Pressable style={[styles.logoutButton, { backgroundColor: colors.secondary, marginBottom: 12, borderWidth: 1, borderColor: colors.border, marginTop: Spacing.xl }]} onPress={toggleTheme}>
              <Icon name={isDark ? "sun" : "moon"} size={20} color={colors.foreground} />
              <Text style={[styles.logoutButtonText, { color: colors.foreground }]}>Zmień motyw ({isDark ? 'Ciemny' : 'Jasny'})</Text>
            </Pressable>

            <Pressable style={[styles.logoutButton, { marginTop: 0 }]} onPress={handleLogout}>
              <Icon name="log-out" size={20} color="#fff" />
              <Text style={styles.logoutButtonText}>Wyloguj się</Text>
            </Pressable>
          </>
        )}

        <Modal
          animationType="slide"
          transparent={true}
          visible={modalVisible}
          onRequestClose={() => setModalVisible(false)}
        >
          <TouchableWithoutFeedback onPress={() => setModalVisible(false)}>
            <View style={styles.modalOverlay}>
              <TouchableWithoutFeedback>
                <View style={styles.modalContent}>
                  <View style={styles.modalHeader}>
                    <Text style={styles.modalTitle}>Zmień zdjęcie profilowe</Text>
                  </View>
                  
                  <Pressable style={styles.modalOption} onPress={openCamera}>
                    <View style={styles.optionIcon1}>
                      <Icon name="camera" size={20} color="#2563eb" />
                    </View>
                    <Text style={styles.optionText}>Zrób zdjęcie</Text>
                  </Pressable>

                  <Pressable style={styles.modalOption} onPress={openGallery}>
                    <View style={styles.optionIcon2}>
                      <Icon name="image" size={20} color="#22c55e" />
                    </View>
                    <Text style={styles.optionText}>Wybierz z galerii</Text>
                  </Pressable>

                  <Pressable style={[styles.modalOption, styles.cancelOption]} onPress={() => setModalVisible(false)}>
                    <Text style={styles.cancelText}>Anuluj</Text>
                  </Pressable>
                </View>
              </TouchableWithoutFeedback>
            </View>
          </TouchableWithoutFeedback>
        </Modal>

        {/* Edit Profile Modal */}
        <Modal
          animationType="slide"
          transparent={true}
          visible={editModalVisible}
          onRequestClose={() => setEditModalVisible(false)}
        >
          <KeyboardAvoidingView behavior={Platform.OS === 'ios' ? 'padding' : 'height'} style={{ flex: 1, backgroundColor: 'rgba(0,0,0,0.5)', justifyContent: 'flex-end' }}>
            <Pressable style={{ flex: 1 }} onPress={() => setEditModalVisible(false)} />
            <View style={{ backgroundColor: colors.background, padding: Spacing.xl, borderTopLeftRadius: 24, borderTopRightRadius: 24 }}>
              <Text style={{ fontSize: 20, fontWeight: '700', color: colors.foreground, marginBottom: Spacing.xl }}>Edytuj profil</Text>

              <Text style={{ color: colors.mutedForeground, marginBottom: 4, fontSize: 14 }}>Imię</Text>
              <TextInput style={{ backgroundColor: colors.card, color: colors.foreground, borderRadius: 8, padding: 12, marginBottom: Spacing.md, borderWidth: 1, borderColor: colors.border }} value={editFirstName} onChangeText={setEditFirstName} />

              <Text style={{ color: colors.mutedForeground, marginBottom: 4, fontSize: 14 }}>Nazwisko</Text>
              <TextInput style={{ backgroundColor: colors.card, color: colors.foreground, borderRadius: 8, padding: 12, marginBottom: Spacing.md, borderWidth: 1, borderColor: colors.border }} value={editLastName} onChangeText={setEditLastName} />

              <Text style={{ color: colors.mutedForeground, marginBottom: 4, fontSize: 14 }}>Email</Text>
              <TextInput style={{ backgroundColor: colors.card, color: colors.foreground, borderRadius: 8, padding: 12, marginBottom: Spacing.md, borderWidth: 1, borderColor: colors.border }} value={editEmail} onChangeText={setEditEmail} keyboardType="email-address" autoCapitalize="none" />

              <View style={{ flexDirection: 'row', gap: Spacing.md }}>
                <View style={{ flex: 1 }}>
                  <Text style={{ color: colors.mutedForeground, marginBottom: 4, fontSize: 14 }}>Waga (kg)</Text>
                  <TextInput style={{ backgroundColor: colors.card, color: colors.foreground, borderRadius: 8, padding: 12, marginBottom: Spacing.md, borderWidth: 1, borderColor: colors.border }} value={editWeight} onChangeText={setEditWeight} keyboardType="numeric" />
                </View>
                <View style={{ flex: 1 }}>
                  <Text style={{ color: colors.mutedForeground, marginBottom: 4, fontSize: 14 }}>Wzrost (cm)</Text>
                  <TextInput style={{ backgroundColor: colors.card, color: colors.foreground, borderRadius: 8, padding: 12, marginBottom: Spacing.md, borderWidth: 1, borderColor: colors.border }} value={editHeight} onChangeText={setEditHeight} keyboardType="numeric" />
                </View>
              </View>

              <Pressable
                style={{ backgroundColor: '#2563eb', padding: 16, borderRadius: 12, alignItems: 'center', marginTop: Spacing.md }}
                onPress={handleSaveEdit}
                disabled={isSavingEdit}
              >
                {isSavingEdit ? <ActivityIndicator color="#fff" /> : <Text style={{ color: '#fff', fontWeight: '600', fontSize: 16 }}>Zapisz zmiany</Text>}
              </Pressable>
            </View>
          </KeyboardAvoidingView>
        </Modal>

      </ScrollView>
    </SafeAreaView>
  );
}

const getProfileAchievementsPreview = (items: UIAchievement[]) => {
  const unlocked = items
    .filter(item => item.unlocked)
    .sort((a, b) => new Date(b.unlockedAt ?? 0).getTime() - new Date(a.unlockedAt ?? 0).getTime());

  const locked = items
    .filter(item => !item.unlocked)
    .sort(() => Math.random() - 0.5);

  return [...unlocked, ...locked].slice(0, 4);
};
