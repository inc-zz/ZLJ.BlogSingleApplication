import React, { useState, useEffect } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import { useTheme } from '../context/ThemeContext';
import { FaMoon, FaSun } from 'react-icons/fa';

const Header = () => {
  const navigate = useNavigate();
  const { isAuthenticated, user } = useAuth();
  const { theme, toggleTheme } = useTheme();
  const [scrolled, setScrolled] = useState(false);

  useEffect(() => {
    const handleScroll = () => {
      setScrolled(window.scrollY > 60);
    };

    window.addEventListener('scroll', handleScroll);
    return () => window.removeEventListener('scroll', handleScroll);
  }, []);

  return (
    <header className={`header ${scrolled ? 'header-scrolled' : ''}`}>
      <div className="container header-content">
        <Link to="/" className="logo">
          <span className="logo-icon">📝</span>
          <span className="logo-text">知识分享平台</span>
        </Link>

        <nav className="header-nav">
          <Link to="/" className="nav-link">
            首页
          </Link>
          {isAuthenticated && (
            <Link to="/editor" className="nav-link">
              写文章
            </Link>
          )}
        </nav>

        <div className="header-actions">
          {/* 主题切换按钮 */}
          <button 
            className="btn-theme-toggle"
            onClick={toggleTheme}
            title={theme === 'light' ? '切换到暗黑模式' : '切换到明亮模式'}
          >
            {theme === 'light' ? <FaMoon /> : <FaSun />}
          </button>
          
          {isAuthenticated ? (
            <div 
              className="user-menu"
              onClick={() => navigate('/profile')}
            >
              <img 
                src={user.avatar} 
                alt={user.nickname} 
                className="user-avatar"
              />
              <span className="user-name">{user.nickname}</span>
            </div>
          ) : (
            <button 
              className="btn-login-header"
              onClick={() => navigate('/login')}
            >
              登录
            </button>
          )}
        </div>
      </div>
    </header>
  );
};

export default Header;
