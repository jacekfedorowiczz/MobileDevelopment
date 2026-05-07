// src/screens/CommunityScreen.tsx
import React from 'react';
import { View, Text, StyleSheet, ScrollView, Image, Pressable } from 'react-native';
import Icon from 'react-native-vector-icons/Feather';
import { Colors, Spacing } from '../theme/theme';

const articles = [
  {
    id: 1,
    title: "Jak poprawnie wykonywać martwy ciąg? Kompletny poradnik",
    excerpt: "Poznaj kluczowe zasady techniki, które uchronią Cię przed kontuzją...",
    imageUrl: "https://images.unsplash.com/photo-1534438327276-14e5300c3a48?crop=entropy&cs=tinysrgb&fit=max&fm=jpg&q=80&w=400",
    readTime: "5 min czytania",
  },
  {
    id: 2,
    title: "Białko po treningu - mit czy konieczność?",
    excerpt: "Rozwiewamy wątpliwości na temat okna anabolicznego...",
    imageUrl: "https://images.unsplash.com/photo-1579722820308-d74e571900a9?crop=entropy&cs=tinysrgb&fit=max&fm=jpg&q=80&w=400",
    readTime: "3 min czytania",
  },
];

const posts = [
  {
    id: 1,
    author: "Anna Nowak",
    avatarUrl: "https://images.unsplash.com/photo-1555431471-91280869572b?crop=entropy&cs=tinysrgb&fit=max&fm=jpg&q=80&w=100",
    time: "2 godz. temu",
    content: "Właśnie pobiłam swój rekord w przysiadach! 140kg x 3 💪 Ciężka praca popłaca!",
    imageUrl: "https://images.unsplash.com/photo-1750698544894-1f012e37e5e6?crop=entropy&cs=tinysrgb&fit=max&fm=jpg&q=80&w=600",
    likes: 24,
    comments: 5,
    category: "Osiągnięcie",
  },
  {
    id: 2,
    author: "Michał Wiśniewski",
    avatarUrl: "https://images.unsplash.com/photo-1613145997970-db84a7975fbb?crop=entropy&cs=tinysrgb&fit=max&fm=jpg&q=80&w=100",
    time: "5 godz. temu",
    content: "Najlepsze ćwiczenia na triceps? Dzielcie się swoimi ulubionymi 💪",
    imageUrl: null,
    likes: 18,
    comments: 12,
    category: "Pytanie",
  },
];

const trendingTopics = [
  { name: "Trening push-pull-legs", posts: 45 },
  { name: "Dieta ketogeniczna", posts: 32 },
  { name: "Suplementacja", posts: 28 },
];

export default function CommunityScreen() {
  return (
    <ScrollView style={styles.container} contentContainerStyle={styles.scrollContent}>
      <View style={styles.header}>
        <View>
          <Text style={styles.title}>Społeczność</Text>
          <Text style={styles.subtitle}>Dziel się postępami i inspiruj innych</Text>
        </View>
        <Pressable style={styles.addButton}>
          <Icon name="plus" size={24} color="#ffffff" />
        </Pressable>
      </View>

      <View style={styles.trendingBox}>
        <View style={styles.trendingHeader}>
          <Icon name="trending-up" size={16} color={Colors.primary} />
          <Text style={styles.trendingTitle}>Popularne tematy</Text>
        </View>
        {trendingTopics.map((topic, i) => (
          <View key={i} style={styles.trendingItem}>
            <Text style={styles.trendingName}>{topic.name}</Text>
            <Text style={styles.trendingPosts}>{topic.posts} postów</Text>
          </View>
        ))}
      </View>

      <View style={styles.sectionHeader}>
        <Icon name="book-open" size={20} color={Colors.primary} />
        <Text style={styles.sectionTitle}>Porady i artykuły</Text>
      </View>
      
      <ScrollView horizontal showsHorizontalScrollIndicator={false} style={styles.carousel}>
        {articles.map((article) => (
          <View key={article.id} style={styles.articleCard}>
            <Image source={{ uri: article.imageUrl }} style={styles.articleImage} />
            <View style={styles.articleContent}>
              <Text style={styles.articleTitle} numberOfLines={2}>{article.title}</Text>
              <Text style={styles.articleExcerpt} numberOfLines={2}>{article.excerpt}</Text>
              <View style={styles.articleFooter}>
                <Icon name="clock" size={12} color={Colors.primary} />
                <Text style={styles.articleTime}>{article.readTime}</Text>
              </View>
            </View>
          </View>
        ))}
      </ScrollView>

      <Text style={styles.sectionTitleLarge}>Najnowsze wpisy</Text>
      {posts.map((post) => (
        <View key={post.id} style={styles.postCard}>
          <View style={styles.postHeader}>
            <Image source={{ uri: post.avatarUrl }} style={styles.avatar} />
            <View>
              <Text style={styles.postAuthor}>{post.author} <Text style={styles.postTime}>· {post.time}</Text></Text>
              <View style={styles.postCategoryBadge}>
                <Text style={styles.postCategoryText}>{post.category}</Text>
              </View>
            </View>
          </View>
          <Text style={styles.postContent}>{post.content}</Text>
          {post.imageUrl && (
            <Image source={{ uri: post.imageUrl }} style={styles.postImage} />
          )}
          <View style={styles.postFooter}>
            <Pressable style={styles.actionButton}>
              <Icon name="heart" size={20} color={Colors.mutedForeground} />
              <Text style={styles.actionText}>{post.likes}</Text>
            </Pressable>
            <Pressable style={styles.actionButton}>
              <Icon name="message-circle" size={20} color={Colors.mutedForeground} />
              <Text style={styles.actionText}>{post.comments}</Text>
            </Pressable>
            <Pressable style={[styles.actionButton, { marginLeft: 'auto' }]}>
              <Icon name="share-2" size={20} color={Colors.mutedForeground} />
            </Pressable>
          </View>
        </View>
      ))}
    </ScrollView>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1, backgroundColor: Colors.background },
  scrollContent: { padding: Spacing.md, paddingBottom: Spacing.xl },
  header: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center', marginBottom: Spacing.lg, marginTop: Spacing.md },
  title: { fontSize: 28, fontWeight: 'bold', color: Colors.foreground },
  subtitle: { fontSize: 14, color: Colors.mutedForeground },
  addButton: { width: 48, height: 48, borderRadius: 12, backgroundColor: Colors.primary, alignItems: 'center', justifyContent: 'center' },
  trendingBox: { backgroundColor: Colors.secondary, padding: Spacing.md, borderRadius: 16, marginBottom: Spacing.lg },
  trendingHeader: { flexDirection: 'row', alignItems: 'center', marginBottom: Spacing.sm },
  trendingTitle: { fontSize: 14, fontWeight: '600', color: Colors.foreground, marginLeft: Spacing.sm },
  trendingItem: { flexDirection: 'row', justifyContent: 'space-between', paddingVertical: Spacing.sm, borderBottomWidth: 1, borderColor: Colors.border },
  trendingName: { fontSize: 14, color: Colors.foreground },
  trendingPosts: { fontSize: 12, color: Colors.mutedForeground },
  sectionHeader: { flexDirection: 'row', alignItems: 'center', marginBottom: Spacing.sm },
  sectionTitle: { fontSize: 18, fontWeight: '600', color: Colors.foreground, marginLeft: Spacing.sm },
  sectionTitleLarge: { fontSize: 18, fontWeight: '600', color: Colors.foreground, marginBottom: Spacing.md, marginTop: Spacing.md },
  carousel: { marginBottom: Spacing.lg, marginHorizontal: -Spacing.md, paddingHorizontal: Spacing.md },
  articleCard: { width: 260, backgroundColor: Colors.secondary, borderRadius: 16, overflow: 'hidden', marginRight: Spacing.md, borderWidth: 1, borderColor: Colors.border },
  articleImage: { width: '100%', height: 120 },
  articleContent: { padding: Spacing.md },
  articleTitle: { fontSize: 14, fontWeight: '600', color: Colors.foreground, marginBottom: 4 },
  articleExcerpt: { fontSize: 12, color: Colors.mutedForeground, marginBottom: 8 },
  articleFooter: { flexDirection: 'row', alignItems: 'center' },
  articleTime: { fontSize: 11, color: Colors.primary, marginLeft: 4, fontWeight: '500' },
  postCard: { backgroundColor: Colors.secondary, borderRadius: 16, overflow: 'hidden', borderWidth: 1, borderColor: Colors.border, marginBottom: Spacing.md },
  postHeader: { flexDirection: 'row', padding: Spacing.md, alignItems: 'center' },
  avatar: { width: 40, height: 40, borderRadius: 20, marginRight: Spacing.sm, borderWidth: 1, borderColor: Colors.border },
  postAuthor: { fontSize: 14, fontWeight: '600', color: Colors.foreground },
  postTime: { fontSize: 12, color: Colors.mutedForeground, fontWeight: 'normal' },
  postCategoryBadge: { backgroundColor: '#e0e7ff', paddingHorizontal: 6, paddingVertical: 2, borderRadius: 6, alignSelf: 'flex-start', marginTop: 2 },
  postCategoryText: { fontSize: 11, color: '#4f46e5', fontWeight: '500' },
  postContent: { fontSize: 14, color: Colors.foreground, paddingHorizontal: Spacing.md, marginBottom: Spacing.md, lineHeight: 20 },
  postImage: { width: '100%', height: 220, marginBottom: Spacing.sm },
  postFooter: { flexDirection: 'row', paddingHorizontal: Spacing.md, paddingVertical: Spacing.sm, borderTopWidth: 1, borderColor: Colors.border },
  actionButton: { flexDirection: 'row', alignItems: 'center', marginRight: Spacing.lg, paddingVertical: Spacing.xs },
  actionText: { fontSize: 14, color: Colors.mutedForeground, marginLeft: 6, fontWeight: '500' },
});
