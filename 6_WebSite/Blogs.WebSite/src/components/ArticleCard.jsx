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

  return (
    <div className="article-card" onClick={handleCardClick}>
      {/* Header: Category badge in top-right */}
      <div className="article-card-header">
        <div className="category-badge-wrapper">
          <span className="tech-badge">
            {article.categoryName}
          </span>
        </div>
      </div>

      {/* Body: Title and Summary */}
      <div className="article-card-body">
        <h3 className="article-title">{article.title}</h3>
        <p className="article-summary">{article.summary}</p>
      </div>

      {/* Footer: Tags on left, User info on right */}
      <div className="article-card-footer">
        <div className="article-tags">
          {article.tags.slice(0, 3).map((tag, index) => (
            <span key={index} className="article-tag">
              {tag}
            </span>
          ))}
        </div>
        
        <div className="article-author-info">
          <img 
            src={article.authorAvatar} 
            alt={article.author} 
            className="author-avatar-small clickable"
            onClick={handleUserClick}
            title="查看用户主页"
          />
          <div className="author-meta-compact">
            <span 
              className="author-name clickable" 
              onClick={handleUserClick}
              title="查看用户主页"
            >
              {article.author}
            </span>
            <span className="publish-date">{article.createdAt}</span>
          </div>
        </div>
      </div>

      {/* Stats bar */}
      <div className="article-stats-bar">
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
