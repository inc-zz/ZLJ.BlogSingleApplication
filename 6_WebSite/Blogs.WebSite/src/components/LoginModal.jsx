import React from 'react';
import { useNavigate } from 'react-router-dom';

const LoginModal = ({ onClose }) => {
  const navigate = useNavigate();

  const handleLogin = () => {
    onClose();
    navigate('/login');
  };

  return (
    <div className="modal-overlay" onClick={onClose}>
      <div className="modal-content" onClick={(e) => e.stopPropagation()}>
        <button className="modal-close" onClick={onClose}>×</button>
        
        <div className="modal-body">
          <div className="modal-icon">🔒</div>
          <h2>需要登录</h2>
          <p>登录后即可点赞、评论、收藏文章</p>
          
          <div className="modal-actions">
            <button className="btn-modal-primary" onClick={handleLogin}>
              去登录
            </button>
            <button className="btn-modal-secondary" onClick={onClose}>
              取消
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default LoginModal;
