import React, { useState, useEffect, useRef } from 'react'; 
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import { FaEdit, FaTrash, FaSignOutAlt, FaCamera, FaPlus, FaTimes, FaLink, FaEye, FaEyeSlash } from 'react-icons/fa';
import { uploadImage, getMyArticles, deleteArticle, toggleArticleVisibility } from '../utils/api';
import { App } from 'antd';

// 主流技术栈数据
const TECH_SKILLS = {
  '前端框架': ['React', 'Vue', 'Svelte', 'Qwik', 'Htmx'],
  '前端构建工具': ['Vite', 'Rspack', 'Turbopack', 'Webpack', 'esbuild'],
  '前端高阶概念': ['Server Components', '边缘渲染', 'AI辅助开发', 'Micro-Frontends', 'Web Components'],
  'Java后端': ['Spring Boot', 'Quarkus', 'Micronaut', 'Vert.x'],
  'Node.js后端': ['NestJS', 'Fastify', 'Express', 'Koa', 'Hono'],
  'Python后端': ['FastAPI', 'Django', 'Flask', 'Tornado'],
  '架构模式': ['微服务', 'Serverless', '云原生', 'Event-Driven', 'CQRS'],
  '数据库': ['MySQL', 'PostgreSQL', 'MongoDB', 'Redis', 'Elasticsearch'],
  '分布式数据库': ['TiDB', 'CockroachDB', 'Cassandra', 'ScyllaDB'],
  '云原生数据库': ['Amazon Aurora', 'Google Spanner', 'Azure Cosmos DB'],
  '容器化': ['Docker', 'Podman', 'containerd'],
  '容器编排': ['Kubernetes', 'Docker Swarm', 'Nomad'],
  '基础设施即代码': ['Terraform', 'Pulumi', 'CloudFormation', 'Ansible'],
  '运维模式': ['GitOps', 'AIOps', 'DevSecOps', 'SRE', 'Platform Engineering'],
  '监控与可观测': ['Prometheus', 'Grafana', 'ELK Stack', 'Datadog', 'New Relic'],
};

const Profile = () => {
  const navigate = useNavigate();
  const { user, isAuthenticated, isLoading, logout, updateUser } = useAuth();
  const fileInputRef = useRef(null);
  const { message, modal } = App.useApp();
  
  const [isEditing, setIsEditing] = useState(false);
  const [formData, setFormData] = useState({
    nickname: '',
    email: '',
    avatar: '',
    bio: '',
    skills: [],
    projectLinks: [{ name: '', url: '' }],
    preferences: {
      theme: 'light',
      emailNotifications: true,
      showEmail: false,
    },
  });
  const [myArticles, setMyArticles] = useState([]);
  const [uploading, setUploading] = useState(false);
  const [skillSearchTerm, setSkillSearchTerm] = useState('');
  const [showSkillDropdown, setShowSkillDropdown] = useState(false);
  const [loadingArticles, setLoadingArticles] = useState(false);
  const [totalArticles, setTotalArticles] = useState(0);
  const [pageIndex] = useState(1);
  const [pageSize] = useState(20);
  const [searchKeyword, setSearchKeyword] = useState('');

  // 加载用户文章
  const fetchMyArticles = async () => {
    setLoadingArticles(true);
    try {
      const response = await getMyArticles(pageIndex, pageSize, searchKeyword);
      console.log('API Response:', response);
      console.log('Response items:', response.items);
      console.log('Response success:', response.success);
      
      if (response.success && response.items) {
        setMyArticles(response.items);
        setTotalArticles(response.total);
        console.log('Articles set to state:', response.items);
      } else { 
        message.error('获取文章列表失败');
      }
    } catch (error) {
      console.error('获取文章列表异常:', error);
      message.error('获取文章列表失败');
    } finally {
      setLoadingArticles(false);
    }
  };

  useEffect(() => {
    // 等待认证状态加载完成
    if (isLoading) return;
    
    if (!isAuthenticated) {
      message.warning('请先登录');
      navigate('/login');
      return;
    }

    if (user) {
      setFormData({
        nickname: user.nickname || '',
        email: user.email || '',
        avatar: user.avatar || '',
        bio: user.bio || '',
        skills: user.skills || [],
        projectLinks: user.projectLinks || [{ name: '', url: '' }],
        preferences: user.preferences || {
          theme: 'light',
          emailNotifications: true,
          showEmail: false,
        },
      });
    }

    // 加载用户文章
    fetchMyArticles();
  }, [isAuthenticated, isLoading, user, navigate, pageIndex, searchKeyword]);

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: value,
    }));
  };

  const handleSaveProfile = () => {
    if (!formData.nickname.trim()) {
      message.warning('昵称不能为空');
      return;
    }

    if (!formData.email.trim()) {
      message.warning('邮箱不能为空');
      return;
    }

    updateUser(formData);
    setIsEditing(false);
    message.success('个人资料已更新');
  };

  const handleCancelEdit = () => {
    setFormData({
      nickname: user.nickname || '',
      email: user.email || '',
      avatar: user.avatar || '',
      bio: user.bio || '',
      skills: user.skills || [],
      projectLinks: user.projectLinks || [{ name: '', url: '' }],
      preferences: user.preferences || {
        theme: 'light',
        emailNotifications: true,
        showEmail: false,
      },
    });
    setIsEditing(false);
    setShowSkillDropdown(false);
  };

  const handleEditArticle = (articleId) => {
    navigate(`/editor/${articleId}`);
  };

  const handleDeleteArticle = (articleId, articleTitle) => {
    modal.confirm({
      title: '确认删除',
      content: `确定要删除文章「${articleTitle}」吗？此操作不可恢复！`,
      okText: '确定',
      okType: 'danger',
      cancelText: '取消',
      onOk: async () => {
        try {
          const response = await deleteArticle(articleId);
          if (response.success) {
            message.success('文章已删除');
            // 重新加载文章列表
            fetchMyArticles();
          } else {
            message.error(response.message || '删除失败');
          }
        } catch (error) {
          if (error.response?.status === 401) {
            message.error('登录已过期，请重新登录');
            navigate('/login');
          } else {
            message.error('删除失败，请重试');
          }
        }
      }
    });
  };

  // 隐藏/显示文章
  const handleToggleVisibility = (articleId, isHidden, articleTitle) => {
    const actionText = isHidden ? '显示' : '隐藏';
    
    modal.confirm({
      title: `确认${actionText}文章`,
      content: `确定要${actionText}文章「${articleTitle}」吗？`,
      okText: '确定',
      cancelText: '取消',
      onOk: async () => {
        try {
          const response = await toggleArticleVisibility(articleId, !isHidden);
          if (response.success) {
            message.success(`文章已${actionText}`);
            // 重新加载文章列表
            fetchMyArticles();
          } else {
            message.error(response.message || `${actionText}失败`);
          }
        } catch (error) {
          if (error.response?.status === 401) {
            message.error('登录已过期，请重新登录');
            navigate('/login');
          } else {
            message.error(`${actionText}失败，请重试`);
          }
        }
      }
    });
  };

  const handleLogout = () => {
    modal.confirm({
      title: '确认退出',
      content: '确定要退出登录吗？',
      okText: '确定',
      cancelText: '取消',
      onOk: () => {
        logout();
        navigate('/login');
      }
    });
  };

  const handleAvatarClick = () => {
    if (isEditing) {
      fileInputRef.current?.click();
    }
  };

  const handleAvatarUpload = async (e) => {
    const file = e.target.files?.[0];
    if (!file) return;

    // 验证文件类型
    if (!file.type.startsWith('image/')) {
      message.warning('请选择图片文件');
      return;
    }

    // 验证文件大小（最大2MB）
    if (file.size > 2 * 1024 * 1024) {
      message.warning('图片大小不能超过2MB');
      return;
    }

    setUploading(true);
    try {
      const result = await uploadImage(file);
      if (result.success) {
        setFormData(prev => ({
          ...prev,
          avatar: result.data.url
        }));
      } else {
        message.error('上传失败，请重试');
      }
    } catch (error) {
      console.error('Upload error:', error);
      message.error('上传失败，请重试');
    } finally {
      setUploading(false);
    }
  };

  // 技能管理
  const handleAddSkill = (skill) => {
    if (!formData.skills.includes(skill)) {
      setFormData(prev => ({
        ...prev,
        skills: [...prev.skills, skill]
      }));
    }
    setSkillSearchTerm('');
    setShowSkillDropdown(false);
  };

  const handleRemoveSkill = (skillToRemove) => {
    setFormData(prev => ({
      ...prev,
      skills: prev.skills.filter(skill => skill !== skillToRemove)
    }));
  };

  // 项目链接管理
  const handleAddProjectLink = () => {
    setFormData(prev => ({
      ...prev,
      projectLinks: [...prev.projectLinks, { name: '', url: '' }]
    }));
  };

  const handleRemoveProjectLink = (index) => {
    setFormData(prev => ({
      ...prev,
      projectLinks: prev.projectLinks.filter((_, i) => i !== index)
    }));
  };

  const handleProjectLinkChange = (index, field, value) => {
    setFormData(prev => ({
      ...prev,
      projectLinks: prev.projectLinks.map((link, i) => 
        i === index ? { ...link, [field]: value } : link
      )
    }));
  };

  // 偏好设置
  const handlePreferenceChange = (key, value) => {
    setFormData(prev => ({
      ...prev,
      preferences: {
        ...prev.preferences,
        [key]: value
      }
    }));
  };

  // 过滤技能列表
  const getFilteredSkills = () => {
    const allSkills = Object.values(TECH_SKILLS).flat();
    if (!skillSearchTerm) return allSkills;
    return allSkills.filter(skill => 
      skill.toLowerCase().includes(skillSearchTerm.toLowerCase())
    );
  };

  if (!user) {
    return <div className="loading-container">加载中...</div>;
  }

  return (
    <div className="profile-container">
      <div className="container">
        <div className="profile-content">
          {/* 个人信息卡片 */}
          <div className="profile-card">
            <div className="profile-header">
              <h2>个人中心</h2>
              <button className="btn-logout" onClick={handleLogout}>
                <FaSignOutAlt />
                退出登录
              </button>
            </div>

            <div className="profile-info">
              <div className="avatar-section">
                <div className="avatar-wrapper" onClick={handleAvatarClick}>
                  <img 
                    src={formData.avatar || user.avatar} 
                    alt={formData.nickname || user.nickname}
                    className="profile-avatar"
                  />
                  {isEditing && (
                    <div className="avatar-overlay">
                      <FaCamera className="camera-icon" />
                      <span>{uploading ? '上传中...' : '上传头像'}</span>
                    </div>
                  )}
                </div>
                {isEditing && (
                  <input
                    ref={fileInputRef}
                    type="file"
                    accept="image/*"
                    onChange={handleAvatarUpload}
                    style={{ display: 'none' }}
                  />
                )}
              </div>

              <div className="info-section">
                {isEditing ? (
                  <>
                    <div className="form-group">
                      <label>昵称</label>
                      <input
                        type="text"
                        name="nickname"
                        value={formData.nickname}
                        onChange={handleInputChange}
                        placeholder="请输入昵称"
                      />
                    </div>
                    <div className="form-group">
                      <label>邮箱</label>
                      <input
                        type="email"
                        name="email"
                        value={formData.email}
                        onChange={handleInputChange}
                        placeholder="请输入邮箱"
                      />
                    </div>
                    <div className="form-group">
                      <label>个人简介</label>
                      <textarea
                        name="bio"
                        value={formData.bio}
                        onChange={handleInputChange}
                        placeholder="介绍一下你自己..."
                        rows="3"
                      />
                    </div>

                    {/* 技能选择 */}
                    <div className="form-group">
                      <label>技能展示</label>
                      <div className="skills-input-wrapper">
                        <input
                          type="text"
                          value={skillSearchTerm}
                          onChange={(e) => {
                            setSkillSearchTerm(e.target.value);
                            setShowSkillDropdown(true);
                          }}
                          onFocus={() => setShowSkillDropdown(true)}
                          placeholder="搜索或选择技能..."
                        />
                        {showSkillDropdown && (
                          <div className="skills-dropdown">
                            <div className="skills-dropdown-header">
                              <span>选择技能</span>
                              <button 
                                type="button"
                                onClick={() => setShowSkillDropdown(false)}
                              >
                                <FaTimes />
                              </button>
                            </div>
                            {Object.entries(TECH_SKILLS).map(([category, skills]) => {
                              const filteredSkills = skills.filter(skill => 
                                !skillSearchTerm || skill.toLowerCase().includes(skillSearchTerm.toLowerCase())
                              );
                              if (filteredSkills.length === 0) return null;
                              
                              return (
                                <div key={category} className="skill-category">
                                  <div className="category-name">{category}</div>
                                  <div className="category-skills">
                                    {filteredSkills.map(skill => (
                                      <button
                                        key={skill}
                                        type="button"
                                        className={`skill-option ${formData.skills.includes(skill) ? 'selected' : ''}`}
                                        onClick={() => handleAddSkill(skill)}
                                      >
                                        {skill}
                                      </button>
                                    ))}
                                  </div>
                                </div>
                              );
                            })}
                          </div>
                        )}
                      </div>
                      <div className="selected-skills">
                        {formData.skills.map(skill => (
                          <span key={skill} className="skill-tag">
                            {skill}
                            <button 
                              type="button"
                              onClick={() => handleRemoveSkill(skill)}
                            >
                              <FaTimes />
                            </button>
                          </span>
                        ))}
                      </div>
                    </div>

                    {/* 项目链接 */}
                    <div className="form-group">
                      <label>个人项目链接</label>
                      {formData.projectLinks.map((link, index) => (
                        <div key={index} className="project-link-input">
                          <input
                            type="text"
                            placeholder="项目名称"
                            value={link.name}
                            onChange={(e) => handleProjectLinkChange(index, 'name', e.target.value)}
                          />
                          <input
                            type="url"
                            placeholder="项目链接 (https://...)"
                            value={link.url}
                            onChange={(e) => handleProjectLinkChange(index, 'url', e.target.value)}
                          />
                          {formData.projectLinks.length > 1 && (
                            <button 
                              type="button"
                              className="btn-remove-link"
                              onClick={() => handleRemoveProjectLink(index)}
                            >
                              <FaTimes />
                            </button>
                          )}
                        </div>
                      ))}
                      <button 
                        type="button"
                        className="btn-add-link"
                        onClick={handleAddProjectLink}
                      >
                        <FaPlus /> 添加项目链接
                      </button>
                    </div>

                    {/* 偏好设置 */}
                    <div className="form-group">
                      <label>个人偏好</label>
                      <div className="preferences-group">
                        <div className="preference-item">
                          <label className="checkbox-label">
                            <input
                              type="checkbox"
                              checked={formData.preferences.emailNotifications}
                              onChange={(e) => handlePreferenceChange('emailNotifications', e.target.checked)}
                            />
                            <span>接收邮件通知</span>
                          </label>
                        </div>
                        <div className="preference-item">
                          <label className="checkbox-label">
                            <input
                              type="checkbox"
                              checked={formData.preferences.showEmail}
                              onChange={(e) => handlePreferenceChange('showEmail', e.target.checked)}
                            />
                            <span>公开显示邮箱</span>
                          </label>
                        </div>
                      </div>
                    </div>

                    <div className="edit-actions">
                      <button className="btn-save" onClick={handleSaveProfile}>
                        保存
                      </button>
                      <button className="btn-cancel" onClick={handleCancelEdit}>
                        取消
                      </button>
                    </div>
                  </>
                ) : (
                  <>
                    <div className="info-item">
                      <label>昵称</label>
                      <span>{user.nickname}</span>
                    </div>
                    <div className="info-item">
                      <label>用户名</label>
                      <span>{user.username}</span>
                    </div>
                    <div className="info-item">
                      <label>邮箱</label>
                      <span>{user.email}</span>
                    </div>
                    {user.bio && (
                      <div className="info-item">
                        <label>个人简介</label>
                        <span>{user.bio}</span>
                      </div>
                    )}
                    {user.skills && user.skills.length > 0 && (
                      <div className="info-item">
                        <label>技能</label>
                        <div className="skills-display">
                          {user.skills.map(skill => (
                            <span key={skill} className="skill-badge">{skill}</span>
                          ))}
                        </div>
                      </div>
                    )}
                    {user.projectLinks && user.projectLinks.some(link => link.name && link.url) && (
                      <div className="info-item">
                        <label>项目链接</label>
                        <div className="project-links-display">
                          {user.projectLinks
                            .filter(link => link.name && link.url)
                            .map((link, index) => (
                            <a 
                              key={index} 
                              href={link.url} 
                              target="_blank" 
                              rel="noopener noreferrer"
                              className="project-link"
                            >
                              <FaLink /> {link.name}
                            </a>
                          ))}
                        </div>
                      </div>
                    )}
                    <button className="btn-edit-profile" onClick={() => setIsEditing(true)}>
                      <FaEdit />
                      编辑资料
                    </button>
                  </>
                )}
              </div>
            </div>
          </div>

          {/* 我的文章 */}
          <div className="articles-card">
            <div className="articles-header">
              <h3>我的文章 ({totalArticles})</h3>
              <div className="articles-actions">
                <input
                  type="text"
                  className="search-input"
                  placeholder="搜索文章..."
                  value={searchKeyword}
                  onChange={(e) => setSearchKeyword(e.target.value)}
                />
                <button className="btn-new-article" onClick={() => navigate('/editor')}>
                  + 写文章
                </button>
              </div>
            </div>

            {loadingArticles ? (
              <div className="loading-articles">加载中...</div>
            ) : myArticles.length > 0 ? (
              <div className="articles-table">
                <div className="table-header">
                  <div className="col-title">标题</div>
                  <div className="col-category">分类</div>
                  <div className="col-tags">标签</div>
                  <div className="col-stats">统计</div>
                  <div className="col-date">发布时间</div>
                  <div className="col-actions">操作</div>
                </div>
                <div className="table-body">
                  {myArticles.map(article => (
                    <div key={article.id} className="table-row">
                      <div className="col-title">
                        <div 
                          className="article-title-link"
                          onClick={() => navigate(`/article/${article.id}`)}
                        >
                          {article.title}
                        </div>
                        {article.summary && (
                          <div className="article-summary">{article.summary}</div>
                        )}
                      </div>
                      <div className="col-category">
                        <span className="category-badge">{article.categoryName}</span>
                      </div>
                      <div className="col-tags">
                        {article.tags ? (
                          <span className="tags-text">{article.tags}</span>
                        ) : (
                          <span className="no-tags">-</span>
                        )}
                      </div>
                      <div className="col-stats">
                        <div className="stats-group">
                          <span className="stat-item">
                            <FaEye className="stat-icon" />
                            {article.viewCount || 0}
                          </span>
                          <span className="stat-item">
                            ❤ {article.likeCount || 0}
                          </span>
                          <span className="stat-item">
                            💬 {article.commentCount || 0}
                          </span>
                        </div>
                      </div>
                      <div className="col-date">
                        {new Date(article.createdAt).toLocaleDateString('zh-CN', {
                          year: 'numeric',
                          month: '2-digit',
                          day: '2-digit'
                        })}
                      </div>
                      <div className="col-actions">
                        <button 
                          className="icon-btn icon-edit"
                          onClick={() => handleEditArticle(article.id)}
                          title="编辑文章"
                        >
                          <FaEdit />
                        </button>
                        <button 
                          className="icon-btn icon-hide"
                          onClick={() => handleToggleVisibility(article.id, article.isHidden, article.title)}
                          title={article.isHidden ? '显示文章' : '隐藏文章'}
                        >
                          {article.isHidden ? <FaEyeSlash /> : <FaEye />}
                        </button>
                        <button 
                          className="icon-btn icon-delete"
                          onClick={() => handleDeleteArticle(article.id, article.title)}
                          title="删除文章"
                        >
                          <FaTrash />
                        </button>
                      </div>
                    </div>
                  ))}
                </div>
              </div>
            ) : (
              <div className="empty-state">
                <p>还没有发布文章</p>
                <button className="btn-start-writing" onClick={() => navigate('/editor')}>
                  开始写作
                </button>
              </div>
            )}
          </div>
        </div>
      </div>
    </div>
  );
};

export default Profile;
