// src/screens/ProfileScreen.tsx
import React, { useState, useEffect } from 'react';
import { View, Text, ScrollView, Image, Pressable, Modal, TouchableWithoutFeedback, Alert, ActivityIndicator } from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';
import Icon from 'react-native-vector-icons/Feather';
import { useAuthStore } from '../store/useAuthStore';
import { useTheme } from '../context/ThemeContext';
import { launchCamera, launchImageLibrary, ImagePickerResponse } from 'react-native-image-picker';
import { ProfileService, ProfileData } from '../api/ProfileService';
import { getProfileStyles } from './ProfileScreen.styles';
import { Spacing } from '../theme/theme';

const achievements = [
  { name: "100 treningów", description: "Ukończono 100 sesji", unlocked: true },
  { name: "Konsystencja", description: "30 dni z rzędu", unlocked: true },
  { name: "Motywator", description: "Zaprosiłeś 5 znajomych", unlocked: true },
  { name: "Marathon", description: "200 treningów", unlocked: false },
];

export default function ProfileScreen() {
  const logout = useAuthStore(state => state.logout);
  const { colors, isDark, toggleTheme } = useTheme();
  const styles = getProfileStyles(colors, isDark);

  const [profileData, setProfileData] = useState<ProfileData | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [profileImage, setProfileImage] = useState("https://images.unsplash.com/photo-1613145997970-db84a7975fbb?crop=entropy&cs=tinysrgb&fit=max&fm=jpg&q=80&w=200");
  const [modalVisible, setModalVisible] = useState(false);

  useEffect(() => {
    fetchProfile();
  }, []);

  const fetchProfile = async () => {
    setIsLoading(true);
    const response = await ProfileService.getProfile();
    if (response.isSuccess && response.value) {
      setProfileData(response.value);
      if (response.value.profileImageUrl) {
        setProfileImage(response.value.profileImageUrl);
      }
    } else {
      console.error(response.errorMessage);
      // Fallback data if API is not fully implemented yet
      setProfileData({
        firstName: "Jan",
        lastName: "Kowalski",
        email: "jan.kowalski@email.com",
        workoutsThisMonth: 24,
        averageWorkoutTime: 48,
        achievementsCount: 12
      });
    }
    setIsLoading(false);
  };

  const handleLogout = async () => {
    const response = await ProfileService.logout();
    if (!response.isSuccess) {
      console.warn('Wylogowanie po stronie serwera się nie powiodło', response.errorMessage);
    }
    logout();
  };

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
                  <Text style={styles.statValue}>{profileData?.workoutsThisMonth}</Text>
                </View>
                <View style={styles.statIconWrapper}>
                  <Icon name="calendar" size={28} color="#2563eb" />
                </View>
              </View>

              <View style={styles.statCard}>
                <View>
                  <Text style={styles.statLabel}>Średni czas treningu</Text>
                  <Text style={styles.statValue}>{profileData?.averageWorkoutTime} min</Text>
                </View>
                <View style={styles.statIconWrapper}>
                  <Icon name="trending-up" size={28} color="#2563eb" />
                </View>
              </View>

              <View style={styles.statCard}>
                <View>
                  <Text style={styles.statLabel}>Osiągnięcia</Text>
                  <Text style={styles.statValue}>{profileData?.achievementsCount}</Text>
                </View>
                <View style={styles.statIconWrapper}>
                  <Icon name="award" size={28} color="#2563eb" />
                </View>
              </View>
            </View>

            <View style={styles.quickActionsContainer}>
              <Pressable style={styles.quickActionCard}>
                <View style={styles.quickActionIconWrapper}>
                  <Icon name="activity" size={20} color="#2563eb" />
                </View>
                <Text style={styles.quickActionTitle}>Twoja dieta</Text>
                <View style={styles.quickActionFooter}>
                  <Text style={styles.quickActionSubtitle}>Zarządzaj planem</Text>
                  <Icon name="chevron-right" size={12} color={colors.mutedForeground} />
                </View>
              </Pressable>
              <Pressable style={styles.quickActionCard}>
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

            <Text style={styles.sectionTitle}>Osiągnięcia</Text>
            <View style={styles.achievementsContainer}>
              {achievements.map((ach, i) => (
                <View key={i} style={[styles.achievementCard, ach.unlocked ? styles.achievementUnlocked : styles.achievementLocked]}>
                  <View style={[styles.achievementIconWrapper, ach.unlocked ? styles.iconUnlocked : styles.iconLocked]}>
                    <Icon name="award" size={24} color={ach.unlocked ? '#fff' : colors.mutedForeground} />
                  </View>
                  <Text style={ach.unlocked ? styles.achievementTitleUnlocked : styles.achievementTitleLocked}>{ach.name}</Text>
                  <Text style={ach.unlocked ? styles.achievementDescUnlocked : styles.achievementDescLocked}>{ach.description}</Text>
                </View>
              ))}
            </View>

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
      </ScrollView>
    </SafeAreaView>
  );
}
