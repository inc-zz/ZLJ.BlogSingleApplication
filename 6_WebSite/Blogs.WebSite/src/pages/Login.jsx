import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import { login as loginApi } from '../utils/authApi';
import SliderCaptcha from '../components/SliderCaptcha';
import TermsModal from '../components/TermsModal';

const Login = () => {
  const navigate = useNavigate();
  const { login } = useAuth();
  
  const [formData, setFormData] = useState({
    account: '',
    password: '',
    captcha: '1001', // 默认验证码，实际上应该是滑块验证的结果
  });
  
  const [isSliderVerified, setIsSliderVerified] = useState(false);
  const [agreedToTerms, setAgreedToTerms] = useState(false);
  const [error, setError] = useState('');
  const [isLoading, setIsLoading] = useState(false);
  const [showTermsModal, setShowTermsModal] = useState(false);
  const [termsType, setTermsType] = useState('terms');
  const [showCaptcha, setShowCaptcha] = useState(false);

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: value,
    }));
    setError('');
  };

  const handleCaptchaSuccess = () => {
    setIsSliderVerified(true);
    // 验证成功后，更新captcha值
    setFormData(prev => ({
      ...prev,
      captcha: '1001' // 滑块验证成功后的标识
    }));
    setError('');
    // 验证成功后隐藏验证码区域
    setTimeout(() => {
      setShowCaptcha(false);
    }, 800);
  };

  const handleCaptchaFail = () => {
    setIsSliderVerified(false);
    setError('验证失败，请重试');
  };

  const handleTermsClick = (e, type) => {
    e.preventDefault();
    setTermsType(type);
    setShowTermsModal(true);
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');

    // 验证用户名
    if (!formData.account.trim()) {
      setError('请输入用户名');
      return;
    }

    // 验证密码
    if (!formData.password.trim()) {
      setError('请输入密码');
      return;
    }

    //验证滑块
    if (!isSliderVerified) {
      setError('请完成拼图验证');
      return;
    }

    // 验证协议勾选
    if (!agreedToTerms) {
      setError('请同意用户协议');
      return;
    }

    setIsLoading(true);

    try {
      // 调用实际API
      const result = await loginApi({
        account: formData.account,
        password: formData.password,
        captcha: formData.captcha
      });

      if (result.success) {
        // 登录成功，更新上下文
        const { userInfo } = result.data;
        
        login({
          username: userInfo.account,
          nickname: userInfo.account, // 可以根据实际情况调整
          email: userInfo.email,
          avatar: `https://api.dicebear.com/7.x/avataaars/svg?seed=${userInfo.account}`,
          phoneNumber: userInfo.phoneNumber,
          description: userInfo.description,
          lastLoginTime: userInfo.lastLoginTime
        });

        // 跳转到首页
        navigate('/');
      } else {
        // 登录失败
        setError(result.message || '登录失败，请重试');
      }
    } catch (error) {
      console.error('Login error:', error);
      setError('登录请求失败，请检查网络连接');
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="login-container">
      <div className="login-card">
        <div className="login-header">
          <h1>欢迎登录</h1>
          <p>知识分享平台</p>
        </div>

        <form onSubmit={handleSubmit} className="login-form">
          {error && (
            <div className="error-message">
              {error}
            </div>
          )}

          <div className="form-row">
            <label htmlFor="account">用户名</label>
            <input
              type="text"
              id="account"
              name="account"
              value={formData.account}
              onChange={handleInputChange}
              placeholder="请输入用户名"
              autoComplete="username"
            />
          </div>

          <div className="form-row">
            <label htmlFor="password">密码</label>
            <input
              type="password"
              id="password"
              name="password"
              value={formData.password}
              onChange={handleInputChange}
              placeholder="123456"
              autoComplete="current-password"
            />
          </div>

          <div 
            className="captcha-hover-area"
            onMouseEnter={() => !isSliderVerified && setShowCaptcha(true)}
            onMouseLeave={() => !isSliderVerified && setShowCaptcha(false)}
          >
            <div className="captcha-placeholder">
              {!isSliderVerified ? '鼠标移至此处进行验证' : '✓ 验证成功'}
            </div>
            {showCaptcha && !isSliderVerified && (
              <div className="captcha-dropdown">
                <SliderCaptcha 
                  onSuccess={handleCaptchaSuccess}
                  onFail={handleCaptchaFail}
                />
              </div>
            )}
          </div>

          <div className="checkbox-group">
            <label className="checkbox-label">
              <input
                type="checkbox"
                checked={agreedToTerms}
                onChange={(e) => setAgreedToTerms(e.target.checked)}
              />
              <span>
                我已阅读并同意
                <a href="#terms" onClick={(e) => handleTermsClick(e, 'terms')}>
                  《用户协议》
                </a>
                和
                <a href="#privacy" onClick={(e) => handleTermsClick(e, 'privacy')}>
                  《隐私政策》
                </a>
              </span>
            </label>
          </div>

          <button
            type="submit"
            className="btn-login"
            disabled={isLoading}
          >
            {isLoading ? '登录中...' : '登录'}
          </button>
          
          <div className="register-link">
            还没有账号？
            <a href="#register" onClick={(e) => { e.preventDefault(); navigate('/register'); }}>
              立即注册
            </a>
          </div>
        </form>
      </div>

      <div className="login-background">
        <div className="bg-circle circle-1"></div>
        <div className="bg-circle circle-2"></div>
        <div className="bg-circle circle-3"></div>
      </div>

      {/* 用户协议弹窗 */}
      <TermsModal 
        isOpen={showTermsModal}
        onClose={() => setShowTermsModal(false)}
        type={termsType}
      />
    </div>
  );
};

export default Login;
