// src/screens/ExerciseDetailScreen.tsx
import React from 'react';
import { View, Text, StyleSheet, ScrollView, Image, Pressable } from 'react-native';
import Icon from 'react-native-vector-icons/Feather';
import { Colors, Spacing } from '../theme/theme';

export default function ExerciseDetailScreen({ navigation }: any) {
  return (
    <View style={styles.container}>
      <ScrollView contentContainerStyle={styles.scrollContent} bounces={false}>
        <View style={styles.heroContainer}>
          <Image
            source={{ uri: "https://images.unsplash.com/photo-1585484764802-387ea30e8432?crop=entropy&cs=tinysrgb&fit=max&fm=jpg&q=80&w=800" }}
            style={styles.heroImage}
          />
          <View style={styles.overlay} />
          
          <Pressable onPress={() => navigation.goBack()} style={styles.backButton}>
            <Icon name="arrow-left" size={24} color="#fff" />
          </Pressable>
          <Pressable style={styles.starButton}>
            <Icon name="star" size={24} color="#fff" />
          </Pressable>

          <View style={styles.playButtonContainer}>
            <Pressable style={styles.playButton}>
              <Icon name="play-circle" size={32} color="#fff" />
            </Pressable>
          </View>
        </View>

        <View style={styles.content}>
          <View style={styles.titleSection}>
            <Text style={styles.title}>Przysiady ze sztangą</Text>
            <View style={styles.subtitleRow}>
              <Icon name="clipboard" size={16} color="#2563eb" />
              <Text style={styles.subtitle}>Partia: Nogi</Text>
            </View>
          </View>

          <View style={styles.tagsContainer}>
            <View style={styles.tag}>
              <Icon name="tag" size={12} color="#2563eb" style={styles.tagIcon} />
              <Text style={styles.tagText}>Średniozaawansowany</Text>
            </View>
            <View style={styles.tag}>
              <Icon name="tag" size={12} color="#2563eb" style={styles.tagIcon} />
              <Text style={styles.tagText}>Sztanga</Text>
            </View>
            <View style={styles.tag}>
              <Icon name="tag" size={12} color="#2563eb" style={styles.tagIcon} />
              <Text style={styles.tagText}>Siła</Text>
            </View>
          </View>

          <View style={styles.descriptionCard}>
            <View style={styles.descriptionHeader}>
              <View style={styles.infoIconWrapper}>
                <Icon name="info" size={16} color="#2563eb" />
              </View>
              <Text style={styles.descriptionTitle}>Opis ćwiczenia</Text>
            </View>
            <Text style={styles.descriptionText}>
              Przysiad ze sztangą na plecach to jedno z najlepszych ćwiczeń wielostawowych, angażujące do pracy niemal całe ciało.
              Główny nacisk kładziony jest na mięśnie czworogłowe ud, pośladki oraz mięśnie głębokie brzucha (core).
              {"\n\n"}
              Pamiętaj o utrzymaniu prostych pleców, napiętym brzuchu oraz prowadzeniu kolan na zewnątrz podczas wykonywania każdego powtórzenia. Ruch powinien być kontrolowany we wszystkich fazach.
            </Text>
          </View>
        </View>
      </ScrollView>
    </View>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1, backgroundColor: Colors.background },
  scrollContent: { paddingBottom: Spacing.xl },
  heroContainer: { width: '100%', height: 300, position: 'relative', backgroundColor: Colors.secondary },
  heroImage: { width: '100%', height: '100%', resizeMode: 'cover' },
  overlay: { ...StyleSheet.absoluteFillObject, backgroundColor: 'rgba(0,0,0,0.3)' },
  backButton: { position: 'absolute', top: 40, left: 24, width: 44, height: 44, borderRadius: 12, backgroundColor: 'rgba(0,0,0,0.3)', alignItems: 'center', justifyContent: 'center' },
  starButton: { position: 'absolute', top: 40, right: 24, width: 44, height: 44, borderRadius: 12, backgroundColor: 'rgba(0,0,0,0.3)', alignItems: 'center', justifyContent: 'center' },
  playButtonContainer: { ...StyleSheet.absoluteFillObject, alignItems: 'center', justifyContent: 'center' },
  playButton: { width: 64, height: 64, borderRadius: 32, backgroundColor: '#2563eb', alignItems: 'center', justifyContent: 'center', shadowColor: '#2563eb', shadowOpacity: 0.4, shadowRadius: 10, elevation: 5 },
  content: { paddingHorizontal: Spacing.lg, marginTop: -20, backgroundColor: Colors.background, borderTopLeftRadius: 24, borderTopRightRadius: 24, paddingTop: Spacing.xl },
  titleSection: { marginBottom: Spacing.lg },
  title: { fontSize: 32, fontWeight: 'bold', color: Colors.foreground, marginBottom: 8 },
  subtitleRow: { flexDirection: 'row', alignItems: 'center' },
  subtitle: { fontSize: 14, fontWeight: '500', color: Colors.mutedForeground, marginLeft: 6 },
  tagsContainer: { flexDirection: 'row', flexWrap: 'wrap', gap: 8, marginBottom: Spacing.xl },
  tag: { flexDirection: 'row', alignItems: 'center', backgroundColor: Colors.secondary, paddingHorizontal: 12, paddingVertical: 8, borderRadius: 8, borderWidth: 1, borderColor: Colors.border },
  tagIcon: { marginRight: 6 },
  tagText: { fontSize: 12, fontWeight: '500', color: Colors.foreground },
  descriptionCard: { backgroundColor: Colors.secondary, borderRadius: 16, padding: Spacing.lg, borderWidth: 1, borderColor: Colors.border },
  descriptionHeader: { flexDirection: 'row', alignItems: 'center', marginBottom: Spacing.md },
  infoIconWrapper: { width: 32, height: 32, borderRadius: 16, backgroundColor: 'rgba(37, 99, 235, 0.1)', alignItems: 'center', justifyContent: 'center', marginRight: 8 },
  descriptionTitle: { fontSize: 18, fontWeight: '600', color: Colors.foreground },
  descriptionText: { fontSize: 14, color: Colors.foreground, opacity: 0.8, lineHeight: 22 },
});
