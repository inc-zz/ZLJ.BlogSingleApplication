import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { FaCalendar, FaEye, FaHeart, FaComment, FaArrowLeft } from 'react-icons/fa';
import { getUserHomepage, getUserArticles } from '../utils/api';
import { Table, App } from 'antd';

const UserProfile = () => {
  const { username } = useParams();
  const navigate = useNavigate();
  const { message } = App.useApp();
  const [userProfile, setUserProfile] = useState(null);
  const [userArticles, setUserArticles] = useState([]);
  const [loading, setLoading] = useState(true);
  const [tableLoading, setTableLoading] = useState(false);
  const [pagination, setPagination] = useState({
    current: 1,
    pageSize: 20,
    total: 0
  });

  // 格式化日期，处理T分隔符
  const formatDate = (dateString) => {
    if (!dateString) return '-';
    return dateString.replace('T', ' ');
  };

  // 加载用户主页信息
  useEffect(() => {
    const fetchUserProfile = async () => {
      setLoading(true);
      try {
        const response = await getUserHomepage(username);
        if (response.success && response.data) {
          const profileData = {
            account: response.data.account,
            summary: response.data.summary || '这个人很懒，什么也没留下',
            createdAt: formatDate(response.data.CreatedAt),
            tags: response.data.tags ? response.data.tags.split(',').filter(t => t.trim()) : [],
            stats: {
              articles: response.data.articleCount || 0,
              followers: response.data.finsCount || 0,
              totalViews: response.data.viewCount || 0,
              totalLikes: response.data.likeCount || 0
            },
            avatar: `https://api.dicebear.com/7.x/avataaars/svg?seed=${username}`
          };
          setUserProfile(profileData);
        } else {
          message.error(response.message || '获取用户信息失败');
        }
      } catch (error) {
        console.error('获取用户主页异常:', error);
        message.error('获取用户信息失败');
      } finally {
        setLoading(false);
      }
    };

    if (username) {
      fetchUserProfile();
    }
  }, [username, message]);

  // 加载用户文章列表
  const fetchUserArticles = async (page = 1, pageSize = 20) => {
    setTableLoading(true);
    try {
      const response = await getUserArticles(username, page, pageSize);
      if (response.success && response.items) {
        setUserArticles(response.items);
        setPagination({
          current: response.pageIndex || page,
          pageSize: response.pageSize || pageSize,
          total: response.total || 0
        });
      } else {
        message.error(response.message || '获取文章列表失败');
        setUserArticles([]);
      }
    } catch (error) {
      console.error('获取用户文章异常:', error);
      message.error('获取文章列表失败');
      setUserArticles([]);
    } finally {
      setTableLoading(false);
    }
  };

  // 初始加载文章
  useEffect(() => {
    if (username) {
      fetchUserArticles(1, 20);
    }
  }, [username]);

  // 处理表格分页变化
  const handleTableChange = (pagination) => {
    fetchUserArticles(pagination.current, pagination.pageSize);
  };

  // 点击标题跳转到文章详情
  const handleTitleClick = (articleId) => {
    navigate(`/article/${articleId}`);
  };

  // 表格列定义
  const columns = [
    {
      title: '序号',
      key: 'index',
      width: 60,
      align: 'center',
      render: (text, record, index) => {
        return (pagination.current - 1) * pagination.pageSize + index + 1;
      }
    },
    {
      title: '文章标题',
      dataIndex: 'title',
      key: 'title',
      width: 250,
      render: (text, record) => (
        <a 
          onClick={() => handleTitleClick(record.id)} 
          style={{ color: '#ff9800', cursor: 'pointer' }}
        >
          {text}
        </a>
      )
    },
    {
      title: '标签',
      dataIndex: 'tags',
      key: 'tags',
      width: 150,
      render: (tags) => (
        <div style={{ display: 'flex', flexWrap: 'wrap', gap: '4px' }}>
          {tags && tags.split('，').map((tag, index) => (
            <span 
              key={index}
              style={{
                padding: '2px 8px',
                background: '#f5f5f5',
                borderRadius: '4px',
                fontSize: '12px',
                color: '#666'
              }}
            >
              {tag}
            </span>
          ))}
        </div>
      )
    },
    {
      title: '简介',
      dataIndex: 'summary',
      key: 'summary',
      ellipsis: true,
      render: (text) => (
        <span style={{ color: '#666' }}>{text}</span>
      )
    },
    {
      title: '分类',
      dataIndex: 'categoryName',
      key: 'categoryName',
      width: 100,
      align: 'center',
      render: (text) => (
        <span style={{
          padding: '3px 10px',
          background: 'rgba(255, 152, 0, 0.1)',
          color: '#ff9800',
          borderRadius: '4px',
          fontSize: '12px',
          fontWeight: 500
        }}>
          {text}
        </span>
      )
    },
    {
      title: '阅读量',
      dataIndex: 'viewCount',
      key: 'viewCount',
      width: 90,
      align: 'center',
      render: (count) => (
        <span style={{ display: 'flex', alignItems: 'center', justifyContent: 'center', gap: '4px' }}>
          <FaEye style={{ fontSize: '12px', color: '#999' }} />
          {count}
        </span>
      )
    },
    {
      title: '点赞数',
      dataIndex: 'likeCount',
      key: 'likeCount',
      width: 90,
      align: 'center',
      render: (count) => (
        <span style={{ display: 'flex', alignItems: 'center', justifyContent: 'center', gap: '4px' }}>
          <FaHeart style={{ fontSize: '12px', color: '#999' }} />
          {count}
        </span>
      )
    },
    {
      title: '评论数',
      dataIndex: 'commentCount',
      key: 'commentCount',
      width: 90,
      align: 'center',
      render: (count) => (
        <span style={{ display: 'flex', alignItems: 'center', justifyContent: 'center', gap: '4px' }}>
          <FaComment style={{ fontSize: '12px', color: '#999' }} />
          {count}
        </span>
      )
    },
    {
      title: '创建时间',
      dataIndex: 'createdAt',
      key: 'createdAt',
      width: 150,
      align: 'center',
      render: (text) => formatDate(text)
    }
  ];

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
                alt={userProfile.account} 
                className="profile-avatar"
              />
              {/* Tech Stack below avatar */}
              {userProfile.tags && userProfile.tags.length > 0 && (
                <div className="profile-tech-stack">
                  {userProfile.tags.map((tag, index) => (
                    <span key={index} className="tech-badge">
                      {tag}
                    </span>
                  ))}
                </div>
              )}
            </div>
            
            {/* User info section */}
            <div className="profile-info">
              <h1 className="profile-name">{userProfile.account}</h1>
              <p className="profile-username">@{username}</p>
              <p className="profile-bio">{userProfile.summary}</p>
              
              {/* Join Date */}
              <div className="profile-meta">
                <FaCalendar className="meta-icon" />
                <span>注册于 {userProfile.createdAt}</span>
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
              <div className="stat-label">粉丝</div>
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
            最近文章 ({pagination.total})
          </h2>
          
          <Table
            columns={columns}
            dataSource={userArticles}
            rowKey="id"
            loading={tableLoading}
            pagination={{
              current: pagination.current,
              pageSize: pagination.pageSize,
              total: pagination.total,
              showSizeChanger: true,
              showTotal: (total) => `共 ${total} 条`,
              pageSizeOptions: ['10', '20', '50', '100']
            }}
            onChange={handleTableChange}
            bordered
            size="middle"
            scroll={{ x: 1200 }}
          />
        </div>
      </div>
    </div>
  );
};

export default UserProfile;
