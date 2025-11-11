import React, { useState, useEffect, useRef, useCallback } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import { getCategories, getArticleList, getRecommendedArticles, getOpenSourceProjects, getTags } from '../utils/api';
import { 
  techStacks, 
  techTags 
} from '../utils/mockData';
import ArticleCard from '../components/ArticleCard';
import { FaSearch } from 'react-icons/fa';
import { App } from 'antd';

const Home = () => {
  const navigate = useNavigate();
  const { isAuthenticated } = useAuth();
  const { message } = App.useApp();
  const [selectedCategoryId, setSelectedCategoryId] = useState(null);
  const [selectedTag, setSelectedTag] = useState(null);
  const [searchQuery, setSearchQuery] = useState('');
  const [scrolled, setScrolled] = useState(false);
  const [categories, setCategories] = useState([]);
  const [isLoadingCategories, setIsLoadingCategories] = useState(true);
  
  // 文章列表相关状态
  const [articles, setArticles] = useState([]);
  const [pageIndex, setPageIndex] = useState(1);
  const [pageSize] = useState(15);
  const [total, setTotal] = useState(0);
  const [isLoading, setIsLoading] = useState(false);
  const [hasMore, setHasMore] = useState(true);
  const loadingRef = useRef(false);
  const observerRef = useRef(null);
  
  // 推荐数据状态
  const [recommendedArticles, setRecommendedArticles] = useState([]);
  const [recommendedProjects, setRecommendedProjects] = useState([]);
  const [isLoadingRecommended, setIsLoadingRecommended] = useState(true);
  
  // 标签数据状态
  const [tags, setTags] = useState([]);
  const [isLoadingTags, setIsLoadingTags] = useState(true);

  // 加载分类数据
  useEffect(() => {
    let isCancelled = false;
    
    const fetchCategories = async () => {
      setIsLoadingCategories(true);
      try {
        const response = await getCategories(5);
        if (isCancelled) return;
        
        if (response.success && response.data) {
          setCategories(response.data);
        } else {
          console.error('获取分类失败:', response.message);
          // 如果获取失败，使用mock数据作为后备
          setCategories(techStacks.map(stack => ({
            id: stack.id,
            name: stack.name,
            articleCount: 0,
            description: null
          })));
        }
      } catch (error) {
        if (isCancelled) return;
        console.error('获取分类异常:', error);
        // 使用mock数据作为后备
        setCategories(techStacks.map(stack => ({
          id: stack.id,
          name: stack.name,
          articleCount: 0,
          description: null
        })));
      } finally {
        if (!isCancelled) {
          setIsLoadingCategories(false);
        }
      }
    };

    fetchCategories();
    
    return () => {
      isCancelled = true;
    };
  }, []);

  // 加载标签数据
  useEffect(() => {
    let isCancelled = false;
    
    const fetchTags = async () => {
      setIsLoadingTags(true);
      try {
        const response = await getTags(30);
        if (isCancelled) return;
        
        if (response.success && response.data) {
          setTags(response.data);
        } else {
          console.error('获取标签失败:', response.message);
          // 如果获取失败，使用mock数据作为后备
          setTags(techTags.map((tag, index) => ({
            id: index + 1,
            name: tag,
            usageCount: 0,
            styleColor: null
          })));
        }
      } catch (error) {
        if (isCancelled) return;
        console.error('获取标签异常:', error);
        // 使用mock数据作为后备
        setTags(techTags.map((tag, index) => ({
          id: index + 1,
          name: tag,
          usageCount: 0,
          styleColor: null
        })));
      } finally {
        if (!isCancelled) {
          setIsLoadingTags(false);
        }
      }
    };

    fetchTags();
    
    return () => {
      isCancelled = true;
    };
  }, []);

  // 加载文章列表
  const loadArticles = useCallback(async (page = 1, isNewSearch = false) => {
    if (loadingRef.current) return;
    if (!isNewSearch && !hasMore) return;
    
    loadingRef.current = true;
    setIsLoading(true);

    try {
      const params = {
        PageIndex: page,
        PageSize: pageSize,
        CategoryId: selectedCategoryId,
        TagId: selectedTag,  // 暂未对接
        Where: searchQuery
      };

      const response = await getArticleList(params);
      
      if (response.success && response.items) {
        const newArticles = response.items.map(item => ({
          id: item.id,
          title: item.title,
          summary: item.summary,
          author: item.createdBy,
          // 随机生成头像
          authorAvatar: `https://api.dicebear.com/7.x/avataaars/svg?seed=${item.createdBy}`,
          likes: item.likeCount,
          views: item.viewCount,
          comments: item.commentCount,
          techStack: getCategoryTechStack(item.categoryName),
          tags: item.tags ? item.tags.split('，') : [],
          createdAt: formatDate(item.createdAt),
          categoryName: item.categoryName,
          coverImage: item.coverImage
        }));

        if (isNewSearch) {
          setArticles(newArticles);
        } else {
          setArticles(prev => [...prev, ...newArticles]);
        }

        setTotal(response.total);
        setPageIndex(page);
        
        // 判断是否还有更多数据
        // 如果返回的数据量小于分页数量，或者总条数小于等于当前页*每页条数，则没有更多数据
        const hasMoreData = newArticles.length >= pageSize && 
                           response.total > (page * pageSize);
        setHasMore(hasMoreData);
      } else {
        console.error('获取文章列表失败:', response.message);
        if (isNewSearch) {
          setArticles([]);
        }
        setHasMore(false);
      }
    } catch (error) {
      console.error('获取文章列表异常:', error);
      if (isNewSearch) {
        setArticles([]);
      }
      setHasMore(false);
    } finally {
      setIsLoading(false);
      loadingRef.current = false;
    }
  }, [selectedCategoryId, selectedTag, searchQuery, pageSize, hasMore]);

  // 根据分类名称获取techStack类型
  const getCategoryTechStack = (categoryName) => {
    const categoryMap = {
      'C#': 'backend',
      'Vue': 'frontend',
      'React': 'frontend',
      'Docker': 'server',
      'Nginx': 'server',
      'MySQL': 'database',
      'Redis': 'database',
      '大模型': 'ai',
      'AI': 'ai'
    };
    return categoryMap[categoryName] || 'frontend';
  };

  // 格式化日期
  const formatDate = (dateString) => {
    const date = new Date(dateString);
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
  };

  // 初始加载文章
  useEffect(() => {
    setArticles([]);
    setPageIndex(1);
    setHasMore(true);
    loadArticles(1, true);
  }, [selectedCategoryId, selectedTag, searchQuery]);

  // 加载推荐数据
  useEffect(() => {
    let isCancelled = false;
    
    const loadRecommendedData = async () => {
      setIsLoadingRecommended(true);
      try {
        // 并行加载推荐文章和推荐项目
        const [articlesRes, projectsRes] = await Promise.all([
          getRecommendedArticles(10),
          getOpenSourceProjects(10)
        ]);

        if (isCancelled) return;

        // 处理推荐文章
        if (articlesRes.success && articlesRes.data) {
          setRecommendedArticles(articlesRes.data.map(item => ({
            id: item.id,
            title: item.title,
            summary: item.summary,
            url: item.url,
            tags: item.tags,
            views: Math.floor(Math.random() * 5000) + 100,  // 模拟浏览量
            likes: Math.floor(Math.random() * 500) + 10      // 模拟点赞数
          })));
        }

        // 处理推荐项目
        if (projectsRes.success && projectsRes.data) {
          setRecommendedProjects(projectsRes.data.map(item => ({
            id: item.id,
            name: item.title,
            description: item.summary,
            url: item.url,
            tags: item.tags,
            stars: Math.floor(Math.random() * 10000) + 100  // 模拟 star 数
          })));
        }
      } catch (error) {
        if (isCancelled) return;
        console.error('加载推荐数据失败:', error);
      } finally {
        if (!isCancelled) {
          setIsLoadingRecommended(false);
        }
      }
    };

    loadRecommendedData();
    
    return () => {
      isCancelled = true;
    };
  }, []);

  // 无限滚动监听
  useEffect(() => {
    const handleScroll = () => {
      setScrolled(window.scrollY > 60);

      // 检查是否接近底部（离底部500px时加载）
      const scrollHeight = document.documentElement.scrollHeight;
      const scrollTop = window.scrollY;
      const clientHeight = window.innerHeight;
      
      if (scrollHeight - scrollTop - clientHeight < 500) {
        if (!loadingRef.current && hasMore && !isLoading) {
          loadArticles(pageIndex + 1, false);
        }
      }
    };

    window.addEventListener('scroll', handleScroll);
    return () => window.removeEventListener('scroll', handleScroll);
  }, [pageIndex, hasMore, isLoading, loadArticles]);

  // 搜索功能
  const handleSearch = () => {
    if (searchQuery.trim()) {
      // 重置并重新加载
      setArticles([]);
      setPageIndex(1);
      setHasMore(true);
      loadArticles(1, true);
    }
  };

  // 清除搜索
  const handleClearSearch = () => {
    setSearchQuery('');
  };

  // 处理分类切换
  const handleCategoryClick = (categoryId) => {
    setSelectedCategoryId(categoryId);
  };

  const handlePublish = () => {
    if (!isAuthenticated) {
      message.warning('请先登录后再发布文章');
      navigate('/login');
      return;
    }
    navigate('/editor');
  };

  const handleTagClick = (tagId) => {
    setSelectedTag(tagId === selectedTag ? null : tagId);
  };

  return (
    <div className="home-container">
      {/* 顶部技术栈导航 + 搜索栏合并 */}
      <div className={`tech-nav-wrapper ${scrolled ? 'tech-nav-sticky' : ''}`}>
        <div className="container">
          <nav className="tech-nav">
            <button
              className={`tech-nav-item ${!selectedCategoryId ? 'active' : ''}`}
              onClick={() => handleCategoryClick(null)}
              title="查看所有分类"
            >
              全部
            </button>
            {isLoadingCategories ? (
              <span className="tech-nav-item loading">加载中...</span>
            ) : (
              categories.map(category => (
                <button
                  key={category.id}
                  className={`tech-nav-item ${selectedCategoryId === category.id ? 'active' : ''}`}
                  onClick={() => handleCategoryClick(category.id)}
                  title={category.description || category.name}
                  alt={category.description || category.name}
                >
                  {category.name}
                  {category.articleCount > 0 && (
                    <span className="category-count">({category.articleCount})</span>
                  )}
                </button>
              ))
            )}
          </nav>
          
          {/* 搜索栏 - 移至技术导航栏底部 */}
          <div className="search-container">
            <div className="search-input-group">
              <FaSearch className="search-icon" />
              <input
                type="text"
                className="search-input"
                placeholder="搜索文章、作者..."
                value={searchQuery}
                onChange={(e) => setSearchQuery(e.target.value)}
                onKeyPress={(e) => e.key === 'Enter' && handleSearch()}
              />
              {searchQuery && (
                <button className="btn-clear-search" onClick={handleClearSearch}>
                  ×
                </button>
              )}
            </div>
            <button 
              className="btn-search" 
              onClick={handleSearch}
              disabled={isLoading || !searchQuery.trim()}
            >
              {isLoading ? '搜索中...' : '搜索'}
            </button>
            <button className="btn-publish" onClick={handlePublish}>
              <span className="publish-icon">+</span>
              发布文章
            </button>
          </div>
        </div>
      </div>

      {/* 主要内容区域 */}
      <div className="container home-content">
        {/* 左侧文章列表 */}
        <div className="articles-section">
          <div className="articles-list">
            {articles.map(article => (
              <ArticleCard key={article.id} article={article} />
            ))}
          </div>
          
          {/* 加载状态和无更多数据提示 */}
          {isLoading && (
            <div className="loading-more">
              <div className="loading-spinner"></div>
              <span>加载中...</span>
            </div>
          )}
          
          {!isLoading && !hasMore && articles.length > 0 && (
            <div className="no-more-data">
              没有更多数据了~
            </div>
          )}
          
          {!isLoading && articles.length === 0 && (
            <div className="no-articles">
              <p>暂无文章</p>
            </div>
          )}
        </div>

        {/* 右侧边栏 */}
        <aside className="sidebar">
          {/* 推荐文章 */}
          <div className="sidebar-card">
            <h3 className="sidebar-title">推荐文章</h3>
            {isLoadingRecommended ? (
              <div className="sidebar-loading">加载中...</div>
            ) : (
              <div className="recommended-list">
                {recommendedArticles.map(article => (
                  <div 
                    key={article.id} 
                    className="recommended-item"
                    onClick={() => {
                      if (article.url) {
                        window.open(article.url, '_blank');
                      }
                    }}
                    style={{ cursor: article.url ? 'pointer' : 'default' }}
                  >
                    <h4>{article.title}</h4>
                    {article.summary && (
                      <p className="recommended-summary">{article.summary}</p>
                    )}
                    <div className="recommended-meta">
                      <span className="views">👁 {article.views}</span>
                      <span className="likes">❤ {article.likes}</span>
                    </div>
                  </div>
                ))}
              </div>
            )}
          </div>

          {/* 推荐开源项目 */}
          <div className="sidebar-card">
            <h3 className="sidebar-title">推荐开源项目</h3>
            {isLoadingRecommended ? (
              <div className="sidebar-loading">加载中...</div>
            ) : (
              <div className="projects-list">
                {recommendedProjects.map(project => (
                  <div 
                    key={project.id} 
                    className="project-item"
                    onClick={() => {
                      if (project.url) {
                        window.open(project.url, '_blank');
                      }
                    }}
                    style={{ cursor: project.url ? 'pointer' : 'default' }}
                  >
                    <h4>{project.name}</h4>
                    <p>{project.description}</p>
                    <div className="project-meta">
                      <span className="stars">⭐ {project.stars}</span>
                    </div>
                  </div>
                ))}
              </div>
            )}
          </div>

          {/* 技术标签 */}
          <div className="sidebar-card">
            <h3 className="sidebar-title">技术标签</h3>
            <div className="tags-cloud">
              {isLoadingTags ? (
                <div className="sidebar-loading">加载中...</div>
              ) : (
                tags.map(tag => (
                  <button
                    key={tag.id}
                    className={`tag-item ${selectedTag === tag.id ? 'active' : ''}`}
                    onClick={() => handleTagClick(tag.id)}
                  >
                    {tag.name}
                  </button>
                ))
              )}
            </div>
          </div>
        </aside>
      </div>
    </div>
  );
};

export default Home;