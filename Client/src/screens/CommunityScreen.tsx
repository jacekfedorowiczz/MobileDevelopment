// src/screens/CommunityScreen.tsx
import React, { useState, useEffect, useCallback } from 'react';
import { View, Text, StyleSheet, FlatList, Image, Pressable, ActivityIndicator, RefreshControl } from 'react-native';
import { useSafeAreaInsets } from 'react-native-safe-area-context';
import Icon from 'react-native-vector-icons/Feather';
import { Spacing } from '../theme/theme';
import { useTheme } from '../context/ThemeContext';
import { PostService, Post } from '../api/PostService';

const articles = [
  { id: 1, title: "Jak poprawnie wykonywać martwy ciąg?", excerpt: "Poznaj kluczowe zasady techniki...", imageUrl: "https://images.unsplash.com/photo-1534438327276-14e5300c3a48?crop=entropy&cs=tinysrgb&fit=max&fm=jpg&q=80&w=400", readTime: "5 min" },
  { id: 2, title: "Białko po treningu - mit czy konieczność?", excerpt: "Rozwiewamy wątpliwości na temat okna...", imageUrl: "https://images.unsplash.com/photo-1579722820308-d74e571900a9?crop=entropy&cs=tinysrgb&fit=max&fm=jpg&q=80&w=400", readTime: "3 min" },
];

const trendingTopics = [
  { name: "Trening push-pull-legs", posts: 45 },
  { name: "Dieta ketogeniczna", posts: 32 },
  { name: "Suplementacja", posts: 28 },
];

export default function CommunityScreen() {
  const { colors, isDark } = useTheme();
  const insets = useSafeAreaInsets();

  // State
  const [posts, setPosts] = useState<Post[]>([]);
  const [page, setPage] = useState(1);
  const [loading, setLoading] = useState(false);
  const [refreshing, setRefreshing] = useState(false);
  const [hasMore, setHasMore] = useState(true);

  const fetchPosts = useCallback(async (pageNumber: number, isRefresh = false) => {
    setLoading(true);
    try {
      const response = await PostService.getPostsAsync(pageNumber, 5);

      if (response.succeeded && response.data) {
        const newPosts = response.data.items;
        setPosts(prev => isRefresh ? newPosts : [...prev, ...newPosts]);
        setHasMore(response.data.hasNextPage);
      }
    } catch (error) {
      console.log('API Error (simulation mode):', error);
      // SYMULACJA DANYCH
      if (pageNumber === 1 || isRefresh) {
        setPosts(mockPosts);
        setHasMore(true);
      } else if (pageNumber < 4) {
        setPosts(prev => [...prev, ...mockPosts.map(p => ({ ...p, id: p.id + pageNumber * 10 }))]);
      } else {
        setHasMore(false);
      }
    } finally {
      setLoading(false);
      setRefreshing(false);
    }
  }, []);

  useEffect(() => {
    fetchPosts(1);
  }, [fetchPosts]);

  const onRefresh = () => {
    setRefreshing(true);
    setPage(1);
    fetchPosts(1, true);
  };

  const loadMore = () => {
    if (hasMore && !loading) {
      const nextPage = page + 1;
      setPage(nextPage);
      fetchPosts(nextPage);
    }
  };

  const renderPost = ({ item }: { item: Post }) => (
    <View style={[styles.postCard, { backgroundColor: colors.secondary, borderColor: colors.border }]}>
      <View style={styles.postHeader}>
        <Image source={{ uri: item.avatarUrl }} style={[styles.avatar, { borderColor: colors.border }]} />
        <View>
          <Text style={[styles.postAuthor, { color: colors.foreground }]}>{item.author} <Text style={[styles.postTime, { color: colors.mutedForeground }]}>· {item.time}</Text></Text>
          <View style={[styles.postCategoryBadge, { backgroundColor: isDark ? '#1e293b' : '#e0e7ff' }]}>
            <Text style={[styles.postCategoryText, { color: isDark ? '#60a5fa' : '#4f46e5' }]}>{item.category}</Text>
          </View>
        </View>
      </View>
      <Text style={[styles.postContent, { color: colors.foreground }]}>{item.content}</Text>
      {item.imageUrl && (
        <Image source={{ uri: item.imageUrl }} style={styles.postImage} />
      )}
      <View style={[styles.postFooter, { borderColor: colors.border }]}>
        <Pressable style={styles.actionButton}>
          <Icon name="heart" size={20} color={colors.mutedForeground} />
          <Text style={[styles.actionText, { color: colors.mutedForeground }]}>{item.likes}</Text>
        </Pressable>
        <Pressable style={styles.actionButton}>
          <Icon name="message-circle" size={20} color={colors.mutedForeground} />
          <Text style={[styles.actionText, { color: colors.mutedForeground }]}>{item.comments}</Text>
        </Pressable>
        <Pressable style={[styles.actionButton, { marginLeft: 'auto' }]}>
          <Icon name="share-2" size={20} color={colors.mutedForeground} />
        </Pressable>
      </View>
    </View>
  );

  const ListHeader = () => (
    <View>
      <View style={[styles.headerContainer, { marginTop: insets.top + Spacing.sm }]}>
        <View>
          <Text style={[styles.title, { color: colors.foreground }]}>Społeczność</Text>
          <Text style={[styles.subtitle, { color: colors.mutedForeground }]}>Dziel się postępami i inspiruj innych</Text>
        </View>
        <Pressable style={styles.addButton}>
          <Icon name="plus" size={24} color="#ffffff" />
        </Pressable>
      </View>

      <View style={[styles.trendingBox, { backgroundColor: colors.secondary }]}>
        <View style={styles.trendingHeader}>
          <Icon name="trending-up" size={16} color="#2563eb" />
          <Text style={[styles.trendingTitle, { color: colors.foreground }]}>Popularne tematy</Text>
        </View>
        {trendingTopics.map((topic, i) => (
          <View key={i} style={[styles.trendingItem, { borderColor: colors.border }]}>
            <Text style={[styles.trendingName, { color: colors.foreground }]}>{topic.name}</Text>
            <Text style={[styles.trendingPosts, { color: colors.mutedForeground }]}>{topic.posts} postów</Text>
          </View>
        ))}
      </View>

      <View style={styles.sectionHeader}>
        <Icon name="book-open" size={20} color="#2563eb" />
        <Text style={[styles.sectionTitle, { color: colors.foreground }]}>Porady i artykuły</Text>
      </View>

      <FlatList
        horizontal
        showsHorizontalScrollIndicator={false}
        style={styles.carousel}
        data={articles}
        keyExtractor={item => item.id.toString()}
        renderItem={({ item }) => (
          <View style={[styles.articleCard, { backgroundColor: colors.secondary, borderColor: colors.border }]}>
            <Image source={{ uri: item.imageUrl }} style={styles.articleImage} />
            <View style={styles.articleContent}>
              <Text style={[styles.articleTitle, { color: colors.foreground }]} numberOfLines={2}>{item.title}</Text>
              <Text style={[styles.articleExcerpt, { color: colors.mutedForeground }]} numberOfLines={2}>{item.excerpt}</Text>
              <View style={styles.articleFooter}>
                <Icon name="clock" size={12} color="#2563eb" />
                <Text style={styles.articleTime}>{item.readTime}</Text>
              </View>
            </View>
          </View>
        )}
      />

      <Text style={[styles.sectionTitleLarge, { color: colors.foreground }]}>Najnowsze wpisy</Text>
    </View>
  );

  return (
    <View style={[styles.container, { backgroundColor: colors.background }]}>
      <FlatList
        data={posts}
        renderItem={renderPost}
        keyExtractor={(item) => item.id.toString()}
        ListHeaderComponent={ListHeader}
        contentContainerStyle={styles.scrollContent}
        onEndReached={loadMore}
        onEndReachedThreshold={0.5}
        refreshControl={
          <RefreshControl refreshing={refreshing} onRefresh={onRefresh} tintColor="#2563eb" />
        }
        ListFooterComponent={() => (
          loading && !refreshing ? (
            <ActivityIndicator size="small" color="#2563eb" style={{ marginVertical: Spacing.md }} />
          ) : null
        )}
      />
    </View>
  );
}

const mockPosts: Post[] = [
  { id: 1, author: "Anna Nowak", avatarUrl: "https://images.unsplash.com/photo-1555431471-91280869572b?crop=entropy&cs=tinysrgb&fit=max&fm=jpg&q=80&w=100", time: "2 godz. temu", content: "Właśnie pobiłam swój rekord w przysiadach! 140kg x 3 💪 Ciężka praca popłaca!", imageUrl: "https://images.unsplash.com/photo-1750698544894-1f012e37e5e6?crop=entropy&cs=tinysrgb&fit=max&fm=jpg&q=80&w=600", likes: 24, comments: 5, category: "Osiągnięcie" },
  { id: 2, author: "Michał Wiśniewski", avatarUrl: "https://images.unsplash.com/photo-1613145997970-db84a7975fbb?crop=entropy&cs=tinysrgb&fit=max&fm=jpg&q=80&w=100", time: "5 godz. temu", content: "Najlepsze ćwiczenia na triceps? Dzielcie się swoimi ulubionymi 💪", imageUrl: null, likes: 18, comments: 12, category: "Pytanie" },
];

const styles = StyleSheet.create({
  container: { flex: 1 },
  scrollContent: { padding: Spacing.md, paddingBottom: Spacing.xl },
  headerContainer: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center', marginBottom: Spacing.lg, marginTop: Spacing.md },
  title: { fontSize: 28, fontWeight: 'bold' },
  subtitle: { fontSize: 14 },
  addButton: { width: 48, height: 48, borderRadius: 12, backgroundColor: '#2563eb', alignItems: 'center', justifyContent: 'center' },
  trendingBox: { padding: Spacing.md, borderRadius: 16, marginBottom: Spacing.lg },
  trendingHeader: { flexDirection: 'row', alignItems: 'center', marginBottom: Spacing.sm },
  trendingTitle: { fontSize: 14, fontWeight: '600', marginLeft: Spacing.sm },
  trendingItem: { flexDirection: 'row', justifyContent: 'space-between', paddingVertical: Spacing.sm, borderBottomWidth: 1 },
  trendingName: { fontSize: 14 },
  trendingPosts: { fontSize: 12 },
  sectionHeader: { flexDirection: 'row', alignItems: 'center', marginBottom: Spacing.sm },
  sectionTitle: { fontSize: 18, fontWeight: '600', marginLeft: Spacing.sm },
  sectionTitleLarge: { fontSize: 18, fontWeight: '600', marginBottom: Spacing.md, marginTop: Spacing.md },
  carousel: { marginBottom: Spacing.lg, marginHorizontal: -Spacing.md, paddingHorizontal: Spacing.md },
  articleCard: { width: 260, borderRadius: 16, overflow: 'hidden', marginRight: Spacing.md, borderWidth: 1 },
  articleImage: { width: '100%', height: 120 },
  articleContent: { padding: Spacing.md },
  articleTitle: { fontSize: 14, fontWeight: '600', marginBottom: 4 },
  articleExcerpt: { fontSize: 12, marginBottom: 8 },
  articleFooter: { flexDirection: 'row', alignItems: 'center' },
  articleTime: { fontSize: 11, color: '#2563eb', marginLeft: 4, fontWeight: '500' },
  postCard: { borderRadius: 16, overflow: 'hidden', borderWidth: 1, marginBottom: Spacing.md },
  postHeader: { flexDirection: 'row', padding: Spacing.md, alignItems: 'center' },
  avatar: { width: 40, height: 40, borderRadius: 20, marginRight: Spacing.sm, borderWidth: 1 },
  postAuthor: { fontSize: 14, fontWeight: '600' },
  postTime: { fontSize: 12, fontWeight: 'normal' },
  postCategoryBadge: { paddingHorizontal: 6, paddingVertical: 2, borderRadius: 6, alignSelf: 'flex-start', marginTop: 2 },
  postCategoryText: { fontSize: 11, fontWeight: '500' },
  postContent: { fontSize: 14, paddingHorizontal: Spacing.md, marginBottom: Spacing.md, lineHeight: 20 },
  postImage: { width: '100%', height: 220, marginBottom: Spacing.sm },
  postFooter: { flexDirection: 'row', paddingHorizontal: Spacing.md, paddingVertical: Spacing.sm, borderTopWidth: 1 },
  actionButton: { flexDirection: 'row', alignItems: 'center', marginRight: Spacing.lg, paddingVertical: Spacing.xs },
  actionText: { fontSize: 14, marginLeft: 6, fontWeight: '500' },
});
