import React from 'react';
import { useNavigate } from 'react-router-dom';
import { FaEye, FaHeart, FaComment } from 'react-icons/fa';

const ArticleCard = ({ article }) => {
  const navigate = useNavigate();

  const handleCardClick = () => {
    navigate(`/article/${article.id}`);
  };

  // Navigate to user profile
  const handleUserClick = (e) => {
    e.stopPropagation(); // Prevent card click
    // Convert author name to username format (simplified)
    const username = article.authorAvatar.split('seed=')[1] || 'user';
    navigate(`/user/${username}`);
  };

  // Get user's tech stack based on article tech stack
  const getUserTechStack = () => {
    const techStackMap = {
      'server': ['Docker', 'Nginx', 'Linux'],
      'backend': ['C#', 'Spring', 'Node.js'],
      'database': ['MySQL', 'Redis', 'MongoDB'],
      'testing': ['Jest', 'Mocha', 'Cypress'],
      'frontend': ['React', 'Vue', 'TypeScript']
    };
    return techStackMap[article.techStack] || ['Full Stack'];
  };

  const userTechStack = getUserTechStack();

  return (
    <div className="article-card" onClick={handleCardClick}>
      {/* Left-Right Layout: Avatar on left, user info on right */}
      <div className="article-card-header">
        <img 
          src={article.authorAvatar} 
          alt={article.author} 
          className="author-avatar clickable"
          onClick={handleUserClick}
          title="查看用户主页"
        />
        <div className="author-info">
          <div className="author-meta">
            <span 
              className="author-name clickable" 
              onClick={handleUserClick}
              title="查看用户主页"
            >
              {article.author}
            </span>
            <span className="publish-date">{article.createdAt}</span>
          </div>
          <div className="user-tech-stack">
              <span className="tech-badge">
                {article.categoryName}
              </span>
          </div>
        </div>
      </div>

      <div className="article-card-body">
        <h3 className="article-title">{article.title}</h3>
        <p className="article-summary">{article.summary}</p>
        
        <div className="article-tags">
          {article.tags.map((tag, index) => (
            <span key={index} className="article-tag">
              {tag}
            </span>
          ))}
        </div>
      </div>

      <div className="article-card-footer">
        <div className="article-stats">
          <span className="stat-item">
            <FaEye className="stat-icon" />
            {article.views}
          </span>
          <span className="stat-item">
            <FaHeart className="stat-icon" />
            {article.likes}
          </span>
          <span className="stat-item">
            <FaComment className="stat-icon" />
            {article.comments}
          </span>
        </div>
      </div>
    </div>
  );
};

export default ArticleCard;
