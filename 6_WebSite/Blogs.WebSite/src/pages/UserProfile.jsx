import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { FaCalendar, FaEye, FaHeart, FaComment, FaArrowLeft } from 'react-icons/fa';
import ArticleCard from '../components/ArticleCard';

const UserProfile = () => {
  const { username } = useParams();
  const navigate = useNavigate();
  const [userProfile, setUserProfile] = useState(null);
  const [userArticles, setUserArticles] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    // Simulate API call to fetch user profile and articles
    const fetchUserProfile = async () => {
      setLoading(true);
      
      // Simulate 300ms delay
      setTimeout(() => {
        // Mock user profile data
        const mockProfile = {
          username: username,
          displayName: username === 'server1' ? '运维小王' : 
                       username === 'backend1' ? 'Java架构师' :
                       username === 'db1' ? 'DBA专家' :
                       username === 'test1' ? '测试工程师' :
                       username === 'frontend1' ? '前端开发者' : username,
          avatar: `https://api.dicebear.com/7.x/avataaars/svg?seed=${username}`,
          bio: '热爱技术，专注于分享高质量的技术文章和最佳实践。持续学习，不断进步。',
          joinDate: '2024-01-15',
          techStack: username.startsWith('server') ? ['Docker', 'Nginx', 'Linux', 'Kubernetes'] :
                     username.startsWith('backend') ? ['Java', 'Spring Boot', 'Node.js', 'Microservices'] :
                     username.startsWith('db') ? ['MySQL', 'Redis', 'MongoDB', 'PostgreSQL'] :
                     username.startsWith('test') ? ['Jest', 'Mocha', 'Cypress', 'Selenium'] :
                     ['React', 'Vue', 'TypeScript', 'Webpack'],
          stats: {
            articles: 12,
            followers: 256,
            totalViews: 15234,
            totalLikes: 1523
          }
        };
        
        // Mock user articles (filtered by author)
        const mockArticles = getMockArticlesByAuthor(mockProfile.displayName);
        
        setUserProfile(mockProfile);
        setUserArticles(mockArticles);
        setLoading(false);
      }, 300);
    };

    fetchUserProfile();
  }, [username]);

  // Get mock articles by author
  const getMockArticlesByAuthor = (author) => {
    // This would normally come from API
    // For now, return mock data based on author name
    const allMockArticles = [
      {
        id: 1,
        title: 'Docker容器化部署最佳实践',
        summary: '详细介绍Docker在生产环境中的部署方案，包括镜像优化、多阶段构建、安全配置等实战技巧...',
        author: '运维小王',
        authorAvatar: 'https://api.dicebear.com/7.x/avataaars/svg?seed=server1',
        likes: 245,
        views: 1523,
        comments: 32,
        techStack: 'server',
        tags: ['Docker', 'DevOps', '容器化'],
        createdAt: '2025-10-15',
      },
      {
        id: 101,
        title: 'Kubernetes生产环境实战指南',
        summary: 'K8s在生产环境中的完整部署流程，包括集群规划、节点配置、网络设置、监控告警等...',
        author: '运维小王',
        authorAvatar: 'https://api.dicebear.com/7.x/avataaars/svg?seed=server1',
        likes: 189,
        views: 987,
        comments: 24,
        techStack: 'server',
        tags: ['Kubernetes', 'K8s', '运维'],
        createdAt: '2025-10-12',
      },
      {
        id: 4,
        title: 'Spring Boot微服务架构设计',
        summary: '基于Spring Boot构建微服务系统，涵盖服务拆分、服务治理、配置中心等核心内容...',
        author: 'Java架构师',
        authorAvatar: 'https://api.dicebear.com/7.x/avataaars/svg?seed=backend1',
        likes: 456,
        views: 3210,
        comments: 67,
        techStack: 'backend',
        tags: ['Spring Boot', '微服务', 'Java'],
        createdAt: '2025-10-16',
      },
      {
        id: 102,
        title: 'Java性能优化实战经验',
        summary: '分享Java应用性能优化的实战经验，包括JVM调优、内存管理、并发优化等技巧...',
        author: 'Java架构师',
        authorAvatar: 'https://api.dicebear.com/7.x/avataaars/svg?seed=backend1',
        likes: 312,
        views: 2145,
        comments: 45,
        techStack: 'backend',
        tags: ['Java', '性能优化', 'JVM'],
        createdAt: '2025-10-10',
      },
      {
        id: 7,
        title: 'MySQL索引优化深度解析',
        summary: 'MySQL索引的底层原理、优化策略、常见陷阱，帮助你写出高性能SQL...',
        author: 'DBA专家',
        authorAvatar: 'https://api.dicebear.com/7.x/avataaars/svg?seed=db1',
        likes: 512,
        views: 4321,
        comments: 89,
        techStack: 'database',
        tags: ['MySQL', '索引优化', '数据库'],
        createdAt: '2025-10-17',
      },
      {
        id: 10,
        title: 'Jest单元测试完全指南',
        summary: '从零开始学习Jest，包括基础用法、Mock、异步测试、覆盖率分析等...',
        author: '测试工程师',
        authorAvatar: 'https://api.dicebear.com/7.x/avataaars/svg?seed=test1',
        likes: 267,
        views: 1893,
        comments: 35,
        techStack: 'testing',
        tags: ['Jest', '单元测试', 'JavaScript'],
        createdAt: '2025-10-16',
      },
    ];

    return allMockArticles.filter(article => article.author === author);
  };

  if (loading) {
    return (
      <div className="user-profile-container">
        <div className="loading">加载中...</div>
      </div>
    );
  }

  if (!userProfile) {
    return (
      <div className="user-profile-container">
        <div className="error">用户不存在</div>
      </div>
    );
  }

  return (
    <div className="user-profile-container">
      <div className="container">
        {/* Back button */}
        <button className="btn-back" onClick={() => navigate(-1)}>
          <FaArrowLeft />
          <span>返回</span>
        </button>

        {/* User Profile Header */}
        <div className="profile-header">
          <div className="profile-main">
            {/* Avatar and tech stack section */}
            <div>
              <img 
                src={userProfile.avatar} 
                alt={userProfile.displayName} 
                className="profile-avatar"
              />
              {/* Tech Stack below avatar */}
              <div className="profile-tech-stack">
                {userProfile.techStack.map((tech, index) => (
                  <span key={index} className="tech-badge">
                    {tech}
                  </span>
                ))}
              </div>
            </div>
            
            {/* User info section */}
            <div className="profile-info">
              <h1 className="profile-name">{userProfile.displayName}</h1>
              <p className="profile-username">@{userProfile.username}</p>
              <p className="profile-bio">{userProfile.bio}</p>
              
              {/* Join Date */}
              <div className="profile-meta">
                <FaCalendar className="meta-icon" />
                <span>加入于 {userProfile.joinDate}</span>
              </div>
            </div>
          </div>

          {/* Stats on separate row */}
          <div className="profile-stats">
            <div className="stat-box">
              <div className="stat-value">{userProfile.stats.articles}</div>
              <div className="stat-label">文章</div>
            </div>
            <div className="stat-box">
              <div className="stat-value">{userProfile.stats.followers}</div>
              <div className="stat-label">关注者</div>
            </div>
            <div className="stat-box">
              <div className="stat-value">{userProfile.stats.totalViews}</div>
              <div className="stat-label">总阅读</div>
            </div>
            <div className="stat-box">
              <div className="stat-value">{userProfile.stats.totalLikes}</div>
              <div className="stat-label">总点赞</div>
            </div>
          </div>
        </div>

        {/* User Articles Section */}
        <div className="profile-content">
          <h2 className="section-title">
            最近文章 ({userArticles.length})
          </h2>
          
          {userArticles.length > 0 ? (
            <div className="articles-grid">
              {userArticles.map(article => (
                <ArticleCard key={article.id} article={article} />
              ))}
            </div>
          ) : (
            <div className="no-articles">
              <p>该用户还没有发布文章</p>
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default UserProfile;
