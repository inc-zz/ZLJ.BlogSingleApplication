import React from 'react';
import { useNavigate } from 'react-router-dom';

const NotFound = () => {
  const navigate = useNavigate();

  const handleGoHome = () => {
    navigate('/');
  };

  const handleGoBack = () => {
    navigate(-1);
  };

  return (
    <div className="notfound-container">
      <div className="notfound-content">
        <div className="notfound-text">
          <h1 className="notfound-message">你访问的资源飞走了</h1>
          <div className="notfound-code">404</div>
        </div>
        
        <div className="notfound-actions">
          <span className="link-home" onClick={handleGoHome}>
            返回首页
          </span>
          <span className="link-separator">|</span>
          <span className="link-back" onClick={handleGoBack}>
            返回上一页
          </span>
        </div>
        
        {/* Decorative elements */}
        <div className="floating-elements">
          <div className="floating-circle circle-1"></div>
          <div className="floating-circle circle-2"></div>
          <div className="floating-circle circle-3"></div>
          <div className="floating-square square-1"></div>
          <div className="floating-square square-2"></div>
        </div>
      </div>
    </div>
  );
};

export default NotFound;
